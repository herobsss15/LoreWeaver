import { Room, RoomEvent, VideoPresets } from '/vendor/livekit-client/livekit-client.esm.mjs';

// Single active call per browser tab - there's never more than one VideoCall
// component mounted at a time, so module-scoped state is enough.
let room;

function attach(participant, publication, container) {
    const track = publication.track;
    if (!track) return null;
    const el = track.attach();
    el.dataset.trackSid = publication.trackSid;
    el.dataset.identity = participant.identity;
    container.appendChild(el);
    return el;
}

function detach(publication, container) {
    const track = publication.track;
    if (!track) return;
    track.detach().forEach((el) => {
        if (container && el.classList.contains('pinned')) {
            container.classList.remove('has-pinned');
        }
        el.remove();
    });
}

// Click a remote video tile to make it the large "spotlight" tile with the
// rest as a filmstrip - only one pinned at a time, click again to unpin.
function togglePin(tile, container) {
    const wasPinned = tile.classList.contains('pinned');
    container.querySelectorAll('.lk-tile.pinned').forEach((t) => t.classList.remove('pinned'));
    tile.classList.toggle('pinned', !wasPinned);
    container.classList.toggle('has-pinned', !wasPinned);
}

function attachRemote(participant, publication, container) {
    const el = attach(participant, publication, container);
    if (el && publication.kind === 'video') {
        el.classList.add('lk-tile');
        el.addEventListener('click', () => togglePin(el, container));
    }
}

async function populateDeviceSelect(select, kind, activeRoom) {
    if (!select) return;
    const devices = await Room.getLocalDevices(kind);
    const activeId = activeRoom?.getActiveDevice(kind);
    const label = kind === 'videoinput' ? 'Câmera' : 'Microfone';

    select.innerHTML = '';
    devices.forEach((d, i) => {
        const option = document.createElement('option');
        option.value = d.deviceId;
        option.textContent = d.label || `${label} ${i + 1}`;
        if (d.deviceId === activeId || (!activeId && i === 0)) option.selected = true;
        select.appendChild(option);
    });
}

export async function connect(url, token, remoteContainerId, localContainerId, cameraSelectId, micSelectId, dotNetRef) {
    const remoteContainer = document.getElementById(remoteContainerId);
    const localContainer = document.getElementById(localContainerId);
    const cameraSelect = document.getElementById(cameraSelectId);
    const micSelect = document.getElementById(micSelectId);

    room = new Room({
        adaptiveStream: true,
        dynacast: true,
        publishDefaults: {
            simulcast: true,
            videoSimulcastLayers: [VideoPresets.h360, VideoPresets.h180],
            screenShareEncoding: VideoPresets.h1080.encoding,
            screenShareSimulcastLayers: [VideoPresets.h720, VideoPresets.h360],
        },
    });

    room
        .on(RoomEvent.TrackSubscribed, (_track, publication, participant) => attachRemote(participant, publication, remoteContainer))
        .on(RoomEvent.TrackUnsubscribed, (_track, publication) => detach(publication, remoteContainer))
        .on(RoomEvent.LocalTrackPublished, (publication) => attach(room.localParticipant, publication, localContainer))
        .on(RoomEvent.LocalTrackUnpublished, (publication) => detach(publication, localContainer))
        .on(RoomEvent.Disconnected, () => dotNetRef.invokeMethodAsync('OnDisconnected'))
        .on(RoomEvent.MediaDevicesChanged, () => {
            populateDeviceSelect(cameraSelect, 'videoinput', room);
            populateDeviceSelect(micSelect, 'audioinput', room);
        })
        .on(RoomEvent.ActiveDeviceChanged, (kind, deviceId) => {
            const select = kind === 'videoinput' ? cameraSelect : kind === 'audioinput' ? micSelect : null;
            if (select) select.value = deviceId;
        });

    cameraSelect?.addEventListener('change', () => room.switchActiveDevice('videoinput', cameraSelect.value));
    micSelect?.addEventListener('change', () => room.switchActiveDevice('audioinput', micSelect.value));

    await room.connect(url, token);
    await room.localParticipant.setCameraEnabled(true);
    await room.localParticipant.setMicrophoneEnabled(true);

    // Only reliably labeled once permission is granted, which the two calls
    // above just did - populating any earlier risks a second, redundant
    // getUserMedia permission race with those.
    await populateDeviceSelect(cameraSelect, 'videoinput', room);
    await populateDeviceSelect(micSelect, 'audioinput', room);
}

export function setCameraEnabled(enabled) {
    return room?.localParticipant.setCameraEnabled(enabled);
}

export function setMicrophoneEnabled(enabled) {
    return room?.localParticipant.setMicrophoneEnabled(enabled);
}

export function setScreenShareEnabled(enabled) {
    return room?.localParticipant.setScreenShareEnabled(enabled);
}

export async function disconnect() {
    if (!room) return;
    await room.disconnect();
    room = undefined;
}
