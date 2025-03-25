# JCS_2DAnimation

Handle frame by frame animation in the simple way.

## Variables

| Name                           | Description                                              |
|:-------------------------------|:---------------------------------------------------------|
| donePlayingAnimCallback        | Callback when done playing animation.                    |
| playFrameCallback              | Callback when play the frame.                            |
| mStartingFrame                 | Starting frame index.                                    |
| Active                         | Current animaion playing?                                |
| PlayOnAwake                    | Play the animation on awake time?                        |
| Loop                           | Does the animation loop?                                 |
| mTimeType                      | Type of the delta time.                                  |
| NullSprite                     | Sprite displayed when the animation stopped.             |
| NullSpriteAfterDonePlayingAnim | Set the sprite to null after done playing the animation. |
| FramePerSec                    | FPS for the animation to play.                           |
| mAnimFrames                    | Drag all the frame here, in order.                       |
| AnimationTimeProduction        | How fast the animation plays.                            |

## Functions

| Name           | Description                                                       |
|:---------------|:------------------------------------------------------------------|
| UpdateMaxFrame | Update the maxinum frame count from the animation frame sequence. |
| Play           | Play the animation in current frame.                              |
| Pause          | Pause the animation by frame?                                     |
| Stop           | Stop the animation, and set the sprite to null.                   |
| Replay         | Play animation from the start of the frame.                       |
| PlayFrame      | Set the current playing frame by index.                           |
| PlayNullFrame  | Play the animation as null frame.                                 |
