/**
 * $File: JCS_DamageTextPool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace JCSUnity
{
    public delegate void SpawnSequenceAction(int[] damage, Vector2[] pos, float timePerSpawn, int totalSpawn);

    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DamageTextPool
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables **")]
        // number to handle and spawn at the beginning
        [SerializeField] private int mNumberOfHandle = 10;

        // optimize
        private int mLastSpawnPos = 0;

        // type of the damage text the object going to handle!
        [Header("NOTE: Plz put the whole set of Damage Text here!")]
        [SerializeField] private JCS_DamageText mDamagetText = null;
        private JCS_Vector<JCS_DamageText> mDamageTexts = null;

        [Header("** Runtime Variables **")]
        [SerializeField] private float mSpacingPerText = 1;
        [SerializeField] private float mTimePerSpawn = 0.1f;
        private float mTimePerSpawnTimer = 0;

        // Audio
        [Header("** Sound **")]
        [SerializeField] private AudioClip mHitSound = null;
        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("** Zigge Right Left Effect (In Sequence)**")]
        [SerializeField] private bool mZiggeEffect = true;
        [SerializeField] private float mRightAlign = 0.2f;
        [SerializeField] private float mLeftAlign = 0.2f;

        private JCS_Vector<int> mSequenceThread = null;
        // Data we need to let Sequence Thread process!
        private JCS_Vector<int[]> mSequenceDamageData = null;
        private JCS_Vector<Vector2[]> mSequencePosData = null;
        private JCS_Vector<float> mSequenceSpanwTimer = null;
        // IMPORTANT: index of number we want to call to spawn the damage text!
        private JCS_Vector<int> mSequenceSpawnCount = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int GetNumberOfHandle() { return this.mNumberOfHandle; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // spawn the pool
            InitDamageTextToArray();

            // spawn all the sequence
            mSequenceThread = new JCS_Vector<int>();
            mSequenceDamageData = new JCS_Vector<int[]>();
            mSequencePosData = new JCS_Vector<Vector2[]>();
            mSequenceSpanwTimer = new JCS_Vector<float>();
            mSequenceSpawnCount = new JCS_Vector<int>();
        }

        private void Update()
        {
            ProccessSequences();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Helper function to spawn the damaget text
        /// so scripter will not have to do all the drug code over and
        /// over agian!
        /// </summary>
        /// <param name="minDamage"> minimum of damage we produce </param>
        /// <param name="maxDamage"> maximum of damage we produce </param>
        /// <param name="pos"> position of damage text we spawn </param>
        /// <param name="hit"> how many damage text we spawn </param>
        /// <returns> data we produced </returns>
        public int[] DamageTextSpawnerSimple(int minDamage, int maxDamage, Vector2 pos, int hit)
        {
            if (minDamage > maxDamage)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool",   "min damage cannot be higher or equal to the max damage!");
                return null;
            }

            if (minDamage < 0 || maxDamage < 0)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool",   "Min or Max damage cannot be lower than 0!");
                return null;
            }

            if (hit <= 0)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool",   "Hit count should not be equal or lower than 0!");
                return null;
            }


            int[] damages = new int[hit];

            for (int index = 0;
                index < hit;
                ++index)
            {
                int dm = Random.Range(minDamage, maxDamage);
                damages[index] = dm;
            }

            SpawnDamagetTexts(damages, Vector2.zero);

            // return the damages we just create!
            return damages;
        }
        public void SpawnDamagetTexts(int[] damage, Vector2 pos)
        {
            Vector2[] poses = new Vector2[damage.Length];
            for (int index = 0;
                index < poses.Length;
                ++index)
            {
                poses[index] = pos;
            }
            SpawnDamagetTexts(damage, poses);
        }
        public void SpawnDamagetTexts(int[] damage, Vector2[] pos)
        {
            if (damage.Length != pos.Length)
            {
                JCS_GameErrors.JcsErrors("JCS_DamageTextPool",   "Wrong pair size!");
                return;
            }

            if (mZiggeEffect)
            {
                for (int count = 0;
                    count < pos.Length;
                    ++count)
                {
                    if ((count % 2) == 0)
                        pos[count].x += mRightAlign;
                    else
                        pos[count].x -= mLeftAlign;

                    pos[count].y += mSpacingPerText * count;
                }
            }

            // add thread!
            mSequenceThread.push(mSequenceThread.length);

            // update data to memory
            mSequenceDamageData.push(damage);
            mSequencePosData.push(pos);

            // simply add a timer!
            mSequenceSpanwTimer.push(0);

            // always start with the first index
            mSequenceSpawnCount.push(0);

        }
        /// <summary>
        /// Spawn One Damage Text
        /// </summary>
        /// <param name="damage"> damage number </param>
        /// <param name="pos"> spawn position </param>
        public void SpawnDamageTextFromPool(int damage, Vector2 pos, bool secondSearch = false)
        {
            if (mNumberOfHandle == 0)
                return;

            JCS_DamageText dt;

            for (int index = mLastSpawnPos;
                index < mNumberOfHandle;
                ++index)
            {
                dt = mDamageTexts.at(index);
                // if not active, meaning we can spawn the text
                if (!dt.isActive())
                {
                    dt.SpawnDamageText(damage, pos);

                    // Hit Sound is the part of sfx sound
                    PlayHitSound();

                    // set the last spawn count
                    mLastSpawnPos = index;
                    return;
                }

            }

            // if we get here mean we cycle once but we
            // did not spawn a text!
            // so reset the spawn pos and 
            // try to search agian until we find one!
            mLastSpawnPos = 0;

            // if function call the second time, 
            // and try to call the third time, 
            // exit the function call.
            // so prevent "stack overflow 
            // search/infinite function call".
            // IMPORTANT(JenChieh): it wont spawn damage text this time, 
            // if this happens.
            if (secondSearch)
            {
#if (UNITY_EDITOR)
                if (JCS_GameSettings.instance.DEBUG_MODE)
                {
                    JCS_GameErrors.JcsWarnings(this,
                        "Prevent, stack overflow function call.");
                }
#endif
                return;
            }

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            SpawnDamageTextFromPool(damage, pos, true);
        }
        

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void InitDamageTextToArray()
        {
            mDamageTexts = new JCS_Vector<JCS_DamageText>(mNumberOfHandle);

            if (mDamagetText == null)
                return;

            for (int count = 0;
                count < mNumberOfHandle;
                ++count)
            {
                // spawn a new game object, 
                // and get the component
                JCS_DamageText dt = (JCS_DamageText)JCS_Utility.SpawnGameObject(mDamagetText);

                // add to array
                mDamageTexts.set(count, dt);

                // set parent
                dt.transform.SetParent(this.transform);
            }

        }

        /// <summary>
        /// The actual thread is doing here.
        /// </summary>
        /// <param name="processIndex"> the unique id of the thread </param>
        /// <param name="damage"> memory data thread needed </param>
        /// <param name="pos"> memory data thread needed </param>
        /// <param name="timer"> memory data thread needed </param>
        private void Sequence(int processIndex, int[] damage, Vector2[] pos, float timer)
        {
            float newTimer = timer;

            newTimer += Time.deltaTime;

            if (mTimePerSpawn < newTimer)
            {
                int count = mSequenceSpawnCount.at(processIndex);
                if (count == damage.Length)
                {
                    // done the sequence, do delete the process(thread)
                    EndProcessSequence(processIndex);
                    return;
                }

                SpawnDamageTextFromPool(damage[count], pos[count]);

                ++count;
                // update new count, in order 
                // to spawn next damage text
                mSequenceSpawnCount.set(processIndex, count);
                newTimer = 0;
            }


            // update timer
            mSequenceSpanwTimer.set(processIndex, newTimer);
        }
        /// <summary>
        /// Loop through the thread and process each thread per frame, 
        /// in addition there is no priority of this thread system.
        /// 
        /// So it will calculate the data with O(n) time complexity in average.
        /// with no first and last!
        /// </summary>
        private void ProccessSequences()
        {
            for (int process = 0;
                process < mSequenceThread.length;
                ++process)
            {
                // pass in all the data wee need in order to process the data
                Sequence(process, 
                    mSequenceDamageData.at(process),
                    mSequencePosData.at(process),
                    mSequenceSpanwTimer.at(process));
            }
        }
        private void EndProcessSequence(int processIndex)
        {
            mSequenceThread.slice(processIndex);
            mSequenceDamageData.slice(processIndex);
            mSequencePosData.slice(processIndex);
            mSequenceSpanwTimer.slice(processIndex);
            mSequenceSpawnCount.slice(processIndex);
        }
        /// <summary>
        /// Hit Sound is the part of sfx sound
        /// </summary>
        private void PlayHitSound()
        {
            if (mHitSound != null)
                mSoundPlayer.PlayOneShot(mHitSound, JCS_GameSettings.GetSFXSound_Volume());
        }

    }
}
