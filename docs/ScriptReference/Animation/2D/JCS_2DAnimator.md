# JCS_2DAnimator

Hold sequence of animations and play it by condition.

## Variables

| Name                     | Description                             |
|:-------------------------|:----------------------------------------|
| mAnimationTimeProduction | How fast the animation plays.           |
| mAnimations              | All the animations this animator holds. |
| m2DAnimDisplayHolder     | Hold animation displayed frame event.   |

## Functions

| Name                 | Description                                                 |
|:---------------------|:------------------------------------------------------------|
| DoAnimation          | Play the animation base on the animation ID.                |
| PlayOneShot          | Play one animation once and go back to original animation.  |
| IsInState            | Check what animation is currently playing by this animator. |
| PlayAnimationInFrame | Play the animation with current frame.                      |
| StopAnimationInFrame | Stop animation with current frame.                          |
