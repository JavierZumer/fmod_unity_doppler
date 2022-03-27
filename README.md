# Enable Doppler for Unity and Fmod
This repo has an example of how to get Doppler working for kinematic objects that move independently from the physics engine.
I modify some of the Fmod plugin classes so kinematic velocities can be send to Unity even without a rigid body.
Modified classes:
- RuntimeManager
- RuntimeUtils
- StudioEventEmitter
- StudioListener
