#!/bin/sh
set -eu

sed \
  -e "s|__TURN_PUBLIC_HOST__|${TURN_PUBLIC_HOST}|g" \
  -e "s|__TURN_USERNAME__|${TURN_USERNAME}|g" \
  -e "s|__TURN_PASSWORD__|${TURN_PASSWORD}|g" \
  /template/livekit.yaml.template > /rendered/livekit.yaml

sed \
  -e "s|__TURN_PUBLIC_IP_MAPPING__|${TURN_PUBLIC_IP_MAPPING}|g" \
  -e "s|__TURN_USERNAME__|${TURN_USERNAME}|g" \
  -e "s|__TURN_PASSWORD__|${TURN_PASSWORD}|g" \
  /template/turnserver.conf.template > /rendered/turnserver.conf
