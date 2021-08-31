# JCS_TweenPanel

Panel that can do tween effect.

## Variables

| Name           | Description                                          |
|:---------------|:-----------------------------------------------------|
| OverrideTween  | Override the tween animation while is still playing. |
| mActiveSound   | Sound played when active this panel.                 |
| mDeactiveSound | Sound plays when this panel is deactive.             |

## Functions

| Name     | Description                                                |
|:---------|:-----------------------------------------------------------|
| Active   | Tween to the taget position and play sound effect.         |
| Deactive | Tween back to the starting position and play sound effect. |

## Require Components

* JCS_TransformTweener
* JCS_SoundPlayer
