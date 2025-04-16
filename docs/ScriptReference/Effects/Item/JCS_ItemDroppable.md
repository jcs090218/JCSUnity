# JCS_ItemDroppable

Effect make this object able to drop items.

## Variables

| Name                     | Description                                                                  |
|:-------------------------|:-----------------------------------------------------------------------------|
| mPossiblityDropAction    | Percentage possibility to drop items.                                        |
| mDropRate                | Multiplier drop rate.                                                        |
| mMinNumItemDrop          | Mininum item drops.                                                          |
| mMaxNumItemDrop          | Maxinum item drops.                                                          |
| mItemSet                 | Item drop information.                                                       |
| mIsGravity               | Make item effect by gravity.                                                 |
| mJumpForce               | How much the item push up to the air.                                        |
| mRandomizeJumpForce      | Randomize the jump force to each item.                                       |
| mRandomizeJumpForceForce | Add this force to item.                                                      |
| mRotateWhileDropping     | Does the item rotate while dropping.                                         |
| mRotateSpeed             | How fast the item rotates?                                                   |
| mSpreadEffect            | Spread the item while dropping.                                              |
| mSpreadGap               | How far between a item to the item next to this item.                        |
| mIncludeDepth            | Did the effect include 3 dimensional?                                        |
| mDestroyFadeOutEffect    | Does the item fade out when is destroyed?                                    |
| mDestroyTime             | When does the item get destroyed?                                            |
| mFadeTime                | How fast it fade out when get destroyed?                                     |
| mConstWaveEffect         | Do constant wave effect to all the items dropped.                            |
| mBounceBackfromWall      | Do the item bounce back from the wall after hit the wall or just stop there. |
| mDropSound               | Drop Sound.                                                                  |

## Functions

| Name      | Description               |
|:----------|:--------------------------|
| DropItems | Do once drop item action. |
