# JCS_ParticleSystem

Particle System thats spawns `JCS_Particle` object.

## Variables

| Name                         | Description                                                                |
|:-----------------------------|:---------------------------------------------------------------------------|
| mNumOfParticle               | Number of particle this particle system hold.                              |
| mParticle                    | Particle you want to spawn.                                                |
| mActive                      | Is the particle engine active?                                             |
| mActiveThread                | Active thread pool?                                                        |
| mOrderLayer                  | What layer should this be render?                                          |
| mDensity                     | How much do it range?                                                      |
| mWindSpeed                   | How much to tilt the particle?                                             |
| mRandPosX                    | Randomize the X position.                                                  |
| mRandPosY                    | Randomize the Y position.                                                  |
| mRandPosZ                    | Randomize the Z position.                                                  |
| mRandAngleX                  | Randomize the X rotation.                                                  |
| mRandAngleY                  | Randomize the Y rotation.                                                  |
| mRandAngleZ                  | Randomize the Z rotation.                                                  |
| mAlwaysTheSameScale          | Apply the scale always the same.                                           |
| mRandScaleX                  | Randomize the X scale.                                                     |
| mRandScaleY                  | Randomize the Y scale.                                                     |
| mRandScaleZ                  | Randomize the Z scale.                                                     |
| mFreezeX                     | Freeze the x axis.                                                         |
| mFreezeY                     | Freeze the y axis.                                                         |
| mFreezeZ                     | Freeze the z axis.                                                         |
| mDoShotImmediately           | Do not process the particle by thread, by main thread.                     |
| mSetChild                    | Set the particles as child?                                                |
| mSetToSamePositionWhenActive | Everytime active the particle set the position to this transform position. |

## Functions

| Name           | Description                                           |
|:---------------|:------------------------------------------------------|
| StartActive    | Start the paritcle engine.                            |
| StopActive     | Stop the paritcle engine.                             |
| PlayOneShot    | Active some particle in certain number current frame. |
| IsParticleEnd  | Check if no particle running?                         |
| SpawnParticles | Spawn all particle base on the count.                 |
