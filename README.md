# WindRigger

WindRigger is a simple audio effect to increase spatial awareness for head mounted displays by mimicking the difference in sound between your left ear and right ear depending on which is oriented to the wind.

## Usage:

* add the WindRig prefab as a child of the part of your camera rig that tracks neck rotation
* attach select a GameObject to identify the windSource (it probably makes sense to use an Empty)
* adjust the audio effects in SetAudioEffects as needed, debug visually by linking DrawLine to effects
