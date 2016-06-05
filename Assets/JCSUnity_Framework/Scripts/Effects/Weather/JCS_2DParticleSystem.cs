/**
 * $File: JCS_2DParticleSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Simulate the Rain effect
    /// </summary>
    public class JCS_2DParticleSystem
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Initialize Variables **")]
        [SerializeField] private uint mNumOfParticle = 50;

        private JCS_Vector<JCS_WeatherParticle> mParticles = null;
        private int mLastAvaliableIndex = 0;

        [Header("** Runtime Variables **")]
        [Tooltip("Particle u want to spawn")]
        [SerializeField] private JCS_WeatherParticle mParticle = null;

        [SerializeField] private int mOrderLayer = 15;

        [SerializeField]
        [Range(0, 100)] private int mDensity = 5;
        [SerializeField]
        [Range(-50, 50)] private float mWindSpeed = 0;


        [SerializeField] [Range(0, 1000)]
        private float mRandPos = 0;
        [SerializeField] [Range(0, 50)]
        private float mRandAngle = 0;

        private float mSequenceTimer = 0;
        private float mTimeAParticle = 0.5f;

        [SerializeField] private bool mFreezeX = false;
        [SerializeField] private bool mFreezeY = true;
        [SerializeField] private bool mFreezeZ = true;
        private Vector3 mFreezePos = Vector3.zero;

        //-- Thread
        private JCS_Vector<int> mThread = null;
        private JCS_Vector<float> mTimers = null;           // timer per thread
        private JCS_Vector<int> mParticleCount = null;         // how many shoot should process per thread
        private JCS_Vector<int> mParticleCounter = null;         // counter per thread


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mParticles = new JCS_Vector<JCS_WeatherParticle>();

            SpawnParticles();


            mThread = new JCS_Vector<int>();
            mTimers = new JCS_Vector<float>();
            mParticleCount = new JCS_Vector<int>();
            mParticleCounter = new JCS_Vector<int>();

            mFreezePos = this.transform.position;
        }

        private void Update()
        {
            ProccessSequences();

            DoParticle();

            Freeze();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void Freeze()
        {
            Vector3 newPos = this.transform.position;

            if (mFreezeX)
                newPos.x = mFreezePos.x;
            if (mFreezeY)
                newPos.y = mFreezePos.y;
            if (mFreezeZ)
                newPos.z = mFreezePos.z;

            this.transform.position = newPos;
        }
        private void DoParticle()
        {
            if (mParticle == null)
                return;

            mSequenceTimer += Time.deltaTime;

            float time = JCS_UsefualFunctions.JCS_FloatRange(0.5f, 1.0f);

            if (time < mSequenceTimer)
            {
                // reset timer
                mSequenceTimer = 0;

                // do a sequence
                DoSequenceParticle(mDensity);
            }
        }
        private void DoSequenceParticle(int density)
        {
            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);
            mParticleCount.push(density);
            mParticleCounter.push(0);
        }
        private void SpawnParticles()
        {
            for (int index = 0;
                index < mNumOfParticle;
                ++index)
            {
                JCS_WeatherParticle trans = (JCS_WeatherParticle)JCS_UsefualFunctions.SpawnGameObject(mParticle);
                mParticles.push(trans);

                // disable the object
                trans.gameObject.SetActive(false);

                // set parent
                trans.transform.SetParent(this.transform);
            }
        }
        private JCS_WeatherParticle SearchAvaliableParticles()
        {
            if (mNumOfParticle == 0)
            {
                JCS_GameErrors.JcsErrors("JCS_2DParticleSystem", -1, "No Particle assign...");
                return null;
            }

            for (int index = mLastAvaliableIndex;
                index < mParticles.length;
                ++index)
            {
                JCS_WeatherParticle particle = mParticles.at(index);
                bool isActive = particle.gameObject.active;
                if (isActive == false)
                {
                    particle.gameObject.SetActive(true);
                    mLastAvaliableIndex = index;
                    return particle;
                }
            }

            // if we get here mean we cycle once but we
            // did not spawn a text!
            // so reset the spawn pos and 
            // try to search agian until we find one!
            mLastAvaliableIndex = 0;

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            return SearchAvaliableParticles();
        }

        private void ProccessSequences()
        {
            for (int processIndex = 0;
                processIndex < mThread.length;
                ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }

        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += Time.deltaTime;

            // check if we can do the particle or not
            if (mTimeAParticle < newTimer)
            {
                int totalParticleCount = mParticleCount.at(processIndex);
                int currentParticleCount = mParticleCounter.at(processIndex);
                if (currentParticleCount == totalParticleCount)
                {
                    // Remove Thread.
                    EndProcessSequence(processIndex);
                    return;
                }

                // find a particle
                JCS_WeatherParticle particle = SearchAvaliableParticles();

                Vector3 spawnPos = this.transform.position;
                if (particle == null)
                {
                    // exit function.
                    return;
                }

                // set position to the same position
                Vector3 newPos = this.transform.position;
                newPos.x = JCS_UsefualFunctions.JCS_FloatRange(-mRandPos, mRandPos);
                particle.transform.position = newPos;

                // set rotation depend on wind speed
                Vector3 newRot = particle.transform.localEulerAngles;
                newRot.z = mWindSpeed;
                newRot.z += JCS_UsefualFunctions.JCS_FloatRange(-mRandAngle, mRandAngle);
                particle.transform.localEulerAngles = newRot;


                ++currentParticleCount;

                // update new count, in order 
                // to spawn next bullet
                mParticleCounter.set(processIndex, currentParticleCount);
                newTimer = 0;
            }

            // update timer
            mTimers.set(processIndex, newTimer);
        }
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mParticleCount.slice(processIndex);
            mParticleCounter.slice(processIndex);
        }

    }
}
