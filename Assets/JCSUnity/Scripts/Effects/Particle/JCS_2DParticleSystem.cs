/**
 * $File: JCS_2DParticleSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Simulate the Rain effect
    /// </summary>
    [RequireComponent(typeof(JCS_EnvironmentSoundPlayer))]
    public class JCS_2DParticleSystem
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_2DParticleSystem) **")]

        [Tooltip("Number of particle this particle system hold.")]
        [SerializeField] [Range(50, 5000)]
        private uint mNumOfParticle = 50;

        private JCS_Vector<JCS_Particle> mParticles = null;
        private int mLastAvaliableIndex = 0;


        [Header("** Runtime Variables (JCS_2DParticleSystem) **")]

        [Tooltip("Particle u want to spawn")]
        [SerializeField]
        private JCS_Particle mParticle = null;

        [Tooltip("What layer should this be render?")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("How much do it range?")]
        [SerializeField] [Range(0, 1000)]
        private int mDensity = 5;

        [Tooltip("How much to tilt the particle?")]
        [SerializeField] [Range(-50, 50)]
        private float mWindSpeed = 0;

        [Tooltip("")]
        [SerializeField] [Range(0, 1000)]
        private float mRandPos = 0;

        [Tooltip("")]
        [SerializeField] [Range(0, 50)]
        private float mRandAngle = 0;

        private float mSequenceTimer = 0;
        private float mTimeAParticle = 0.5f;


        [Header("- Freeze Effect")]

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeY = true;

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeZ = true;

        private Vector3 mFreezePos = Vector3.zero;


        [Header("- Other Setting")]

        [Tooltip("Set the particles as child?")]
        [SerializeField]
        private bool mSetChild = true;

        [Tooltip("Everytime active the particle set the position to this transform position.")]
        [SerializeField]
        private bool mSetToSamePositionWhenActive = true;

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
        public int OrderLayer { get { return this.mOrderLayer; } }
        public JCS_Vector<JCS_Particle> GetParticles() { return this.mParticles; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mParticles = new JCS_Vector<JCS_Particle>();

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

        /// <summary>
        /// Freee the paritcle engine position?
        /// </summary>
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

        /// <summary>
        /// Do the particle algorithm here.
        /// </summary>
        private void DoParticle()
        {
            if (mParticle == null)
                return;

            mSequenceTimer += Time.deltaTime;

            float time = JCS_Utility.JCS_FloatRange(0.5f, 1.0f);

            if (time < mSequenceTimer)
            {
                // reset timer
                mSequenceTimer = 0;

                // do a sequence
                DoSequenceParticle(mDensity);
            }
        }

        /// <summary>
        /// Spawn a thread in order to get process.
        /// </summary>
        /// <param name="density"> density for the particle effect. </param>
        private void DoSequenceParticle(int density)
        {
            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);
            mParticleCount.push(density);
            mParticleCounter.push(0);
        }

        /// <summary>
        /// Spawn all particle base on the count.
        /// </summary>
        private void SpawnParticles()
        {
            if (mParticle == null)
            {
                JCS_Debug.JcsErrors(
                    this, "No particle assign!");

                return;
            }

            for (int index = 0;
                index < mNumOfParticle;
                ++index)
            {
                JCS_Particle trans = (JCS_Particle)JCS_Utility.SpawnGameObject(mParticle);
                mParticles.push(trans);

                // disable the object
                trans.gameObject.SetActive(false);

                if (mSetChild)
                {
                    // set parent
                    trans.transform.SetParent(this.transform);
                }
            }
        }

        /// <summary>
        /// Find the avaliable particle in the list.
        /// </summary>
        /// <returns> non-active paritcle. </returns>
        private JCS_Particle SearchAvaliableParticles(bool sec = false)
        {
            if (mNumOfParticle <= 0)
            {
                JCS_Debug.JcsErrors(
                    this, "Number of particle cannot lower or equal to zero...");

                return null;
            }

            for (int index = mLastAvaliableIndex;
                index < mParticles.length;
                ++index)
            {
                JCS_Particle particle = mParticles.at(index);
                bool isActive = particle.gameObject.activeInHierarchy;
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

            // if second time still does not find, return null.
            // prevent stack overflow.
            if (sec)
                return null;

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            return SearchAvaliableParticles(true);
        }

        /// <summary>
        /// Loop through thread list and process it.
        /// </summary>
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

        /// <summary>
        /// Process the thread.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
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
                JCS_Particle particle = SearchAvaliableParticles();

                if (particle == null)
                {
                    // exit function.
                    return;
                }

                if (mSetToSamePositionWhenActive)
                {
                    // set position to the same position
                    Vector3 newPos = this.transform.position;
                    newPos.x = JCS_Utility.JCS_FloatRange(-mRandPos, mRandPos);
                    particle.transform.position = newPos;
                }

                // set rotation depend on wind speed
                Vector3 newRot = particle.transform.localEulerAngles;
                newRot.z = mWindSpeed;
                newRot.z += JCS_Utility.JCS_FloatRange(-mRandAngle, mRandAngle);
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

        /// <summary>
        /// Delete the thead when done processing.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mParticleCount.slice(processIndex);
            mParticleCounter.slice(processIndex);
        }

    }
}
