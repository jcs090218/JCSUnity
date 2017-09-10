/**
 * $File: JCS_2DConstWaveEffect.cs $
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
    /// Const wave effect.
    /// </summary>
    public class JCS_2DConstWaveEffect
        : MonoBehaviour
    {
        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Runtime Varialbes (JCS_2DConstWaveEffect) **")]

        [Tooltip("as play on awake!")]
        [SerializeField]
        private bool mEffect = false;

        [Tooltip("How much force goes up and down.")]
        [SerializeField]
        private float mAmplitude = 0.1f;

        [Tooltip("How fast the wave move up and down.")]
        [SerializeField]
        private float mFrequency = 2f;

        // time to run the sine/cosine wave.
        private float mTime = 0;

        [Tooltip("Which axis it move.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_Y;

        [Header("- Randomize Setting (JCS_2DConstWaveEffect)")]

        [Tooltip("Randomize a bit the amplitude value at start.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandomizeAmplitudeAtStart = 0.0f;

        [Tooltip("Randomize a bit the frequency value at start.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandomizeFrequencyAtStart = 0.0f;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            RandomizeAmplitude();
            RandomizeFrequency();
        }

        private void Update()
        {
            if (!mEffect)
            {
                mTime = 0;
                return;
            }

            Vector3 newPos = this.transform.position;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    {
                        newPos.x += (mAmplitude * Mathf.Cos(mTime * mFrequency)) * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        newPos.z += (mAmplitude * Mathf.Cos(mTime * mFrequency)) * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        newPos.y += (mAmplitude * (Mathf.Cos(mTime * mFrequency))) * Time.deltaTime;
                    }
                    break;
            }

            this.transform.position = newPos;

            mTime += Time.deltaTime;
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Randomize a bit of amplitude value.
        /// </summary>
        private void RandomizeAmplitude()
        {
            if (mRandomizeAmplitudeAtStart == 0.0f)
                return;

            mAmplitude += JCS_Random.Range(-mRandomizeAmplitudeAtStart, mRandomizeAmplitudeAtStart);
        }

        /// <summary>
        /// Randomize a bit of frequency value.
        /// </summary>
        private void RandomizeFrequency()
        {
            if (mRandomizeFrequencyAtStart == 0.0f)
                return;

            mFrequency += JCS_Random.Range(-mRandomizeFrequencyAtStart, mRandomizeFrequencyAtStart);
        }

    }
}
