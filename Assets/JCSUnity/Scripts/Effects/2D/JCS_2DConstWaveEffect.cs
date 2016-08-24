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
    public class JCS_2DConstWaveEffect
        : MonoBehaviour
    {
        [Header("** Initialize Varialbes **")]
        [Tooltip("as play on awake!")]
        [SerializeField] private bool mEffect = false;
        [SerializeField] private float mWaveRestPosition = 0.0f;
        [SerializeField] private float mAmplitude = 0.1f;      // amplitude
        [SerializeField] private float mFrequency = 0.5f;      // 
        [SerializeField] private float mSampleRate = 20.0f;

        private float mTime = 0;

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }


        private void Update()
        {
            if (!mEffect)
            {
                mTime = 0;
                return;
            }

            Vector3 currentPos = this.transform.position;
            currentPos.y = 
                (mWaveRestPosition + this.transform.position.y) + 
                (mAmplitude * Mathf.Cos(mTime * Mathf.PI * mFrequency * 2) / mSampleRate);

            this.transform.position = currentPos;

            mTime += Time.deltaTime;
        }

    }
}
