import { Room, RoomEvent, VideoPresets } from '/vendor/livekit-client/livekit-client.esm.mjs';

// Single active call per browser tab - there's never more than one VideoCall
// component mounted at a time, so module-scoped state is enough.
let room;

function attach(participant, publication, container) {
    const track = publication.track;
    if (!track) return;
    const el = track.attach();
    el.dataset.trackSid = publication.trackSid;
    el.dataset.identity = participant.identity;
    container.appendChild(el);
}

function detach(publication) {
    const track = publication.track;
    if (!track) return;
    track.detach().forEach((el) => el.remove());
}

export async function connect(url, token, remoteContainerId, localContainerId, dotNetRef) {
    const remoteContainer = document.getElementById(remoteContainerId);
    const localContainer = document.getElementById(localContainerId);

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
        .on(RoomEvent.TrackSubscribed, (_track, publication, participant) => attach(participant, publication, remoteContainer))
        .on(RoomEvent.TrackUnsubscribed, (_track, publication) => detach(publication))
        .on(RoomEvent.LocalTrackPublished, (publication) => attach(room.localParticipant, publication, localContainer))
        .on(RoomEvent.LocalTrackUnpublished, (publication) => detach(publication))
        .on(RoomEvent.Disconnected, () => dotNetRef.invokeMethodAsync('OnDisconnected'));

    await room.connect(url, token);
    await room.localParticipant.setCameraEnabled(true);
    await room.localParticipant.setMicrophoneEnabled(true);
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
