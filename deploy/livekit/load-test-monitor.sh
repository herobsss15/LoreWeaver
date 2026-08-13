#!/bin/sh
# Run ON the home server during a real load test: all 5 participants connected,
# every stream active (5x webcam+mic + 1 screen share). The point is to check
# whether the jitter observed at idle (~450ms peaks, measured before LiveKit was
# even running) actually shows up under real concurrent load - not to guess at it
# from a spec document.
#
# Usage: ./load-test-monitor.sh [network-interface] [mtr-target]
#   network-interface  defaults to the interface carrying the default route
#   mtr-target         optional host to also watch path jitter/loss against.
#                       There's no single "right" target when you're the SFU
#                       relaying for 5 peers - a participant's public IP, or the
#                       router's own WAN IP as a rough proxy, both work.
#
# Reference numbers (see project notes): real upload ~61 Mbps, estimated peak
# load ~25-30 Mbps with everything active - comfortable headroom on paper, so
# the open question is jitter/loss under load, not raw throughput.

set -eu

IFACE="${1:-$(ip route show default | awk '/default/ {print $5; exit}')}"
TARGET="${2:-}"

if [ -z "$IFACE" ]; then
  echo "Could not auto-detect the default network interface - pass it explicitly:" >&2
  echo "  ./load-test-monitor.sh eth0" >&2
  exit 1
fi

echo "Watching interface: $IFACE"
echo "Let this run for the full test - short samples miss the moments jitter actually spikes."
echo

if [ -n "$TARGET" ]; then
  echo "Also run this in a second terminal for path jitter/loss:"
  echo "  mtr --report-cycles 0 $TARGET"
  echo
fi

exec nload "$IFACE"
