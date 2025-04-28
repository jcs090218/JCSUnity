# JCS_SwingAttackAction

Action to do swing attack.

## Variables

| Name                   | Description                                                                      |
|:-----------------------|:---------------------------------------------------------------------------------|
| mAttackRange           | Collider to detect weather the enemy get hit or not.                             |
| mAbilityFormat         | Ability format in order to apply damage.                                         |
| mAtkAnim               | Animation while this action is active.                                           |
| mAtkAnimSpawnTrans     | Transform attack animation will spawn, default will be the collider attah above. |
| mOrderLayer            | What sorting layer you want the skill to be render?                              |
| mLoopTimes             | How many time you want the animation to loops?                                   |
| mTimeType              | Type of the delta time.                                                          |
| mAsSamePosition        | The same position as the spawn transform's position.                             |
| mAsSameRotation        | The same rotation as the spawn transform's rotation.                             |
| mAsSameScale           | The same scale as the spawn transform's scale.                                   |
| mAnimOffsetPosition    | Animation offset position value.                                                 |
| mAnimOffsetScale       | Animation offset scale value.                                                    |
| mAudioClip             | Sound to play for this action.                                                   |
| mApplyDamageTextAction | If you want the action apply damage text add apply this.                         |
| mKeyCode               | Key to active attack.                                                            |
| mSpeedLayer            | Speed layer.                                                                     |
| mTimeToActTime         | Time to active attack.                                                           |
| mActTime               | Time after attack, delay a while.                                                |

## Functions

| Name      | Description       |
|:----------|:------------------|
| Attack    | Start the action. |
| EndAttack | End the action.   |
