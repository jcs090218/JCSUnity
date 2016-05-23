/**
 * $File: JCS_FadeObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    public class JCS_FadeObject 
        : JCS_UnityObject
    {


        private JCS_FadeType mFadeType = JCS_FadeType.FADE_IN;  // defaul as visible

        private float mAlpha = 1.0f;
        [SerializeField] private float mFadeTime = 1.0f;
        [SerializeField] private bool mOverriteFade = false;

        private Color mRecordColor;

        private bool mEffect = false;
        private bool mVisible = true;

        public float FadeTime { get { return this.mFadeTime; } set { this.mFadeTime = value; } }
        public float GetAlpha() { return this.mAlpha; }

        public bool IsFadeIn()
        {
            return (this.mAlpha >= 1.0f);
        }

        public bool IsFadeOut()
        {
            return (this.mAlpha <= 0.0f);
        }


        private void Awake()
        {
            UpdateUnityData();
        }

        private void Update()
        {

            if (GetObjectType() == JCS_UnityObjectType.GAME_OBJECT &&
                JCS_ApplicationManager.APP_PAUSE)
                return;

            if (!mEffect)
                return;


            switch (mFadeType)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        // Fade out effect complete
                        if (mAlpha < 0.0f)
                        {
                            switch (GetObjectType())
                            {
                                case JCS_UnityObjectType.GAME_OBJECT:
                                    this.gameObject.SetActive(false);
                                    break;
                                case JCS_UnityObjectType.UI:
                                    mImage.enabled = false;
                                    break;
                            }

                            mEffect = false;
                            return;
                        }

                        mAlpha -= Time.deltaTime / mFadeTime;
                    }
                    break;

                case JCS_FadeType.FADE_IN:
                    {
                        // Fade in effect complete
                        if (mAlpha > 1.0f)
                        {
                            mEffect = false;
                            return;
                        }

                        mAlpha += Time.deltaTime / mFadeTime;
                    }
                    break;
            }

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer.material.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
            }

        }

        public void FadeOut() { FadeOut(mFadeTime); }
        public void FadeIn() { FadeIn(mFadeTime); }
        public void FadeOut(float time) { FadeEffect(JCS_FadeType.FADE_OUT, time); }
        public void FadeIn(float time) { this.FadeEffect(JCS_FadeType.FADE_IN, time); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"> Type to fade </param>
        /// <param name="time"> time to fade in/out, in seconds </param>
        private void FadeEffect(JCS_FadeType type, float time)
        {
            if (!mOverriteFade)
            {
                // Check is already fade out or fade in!
                if ((mVisible && type == JCS_FadeType.FADE_IN) ||
                    (!mVisible && type == JCS_FadeType.FADE_OUT))
                    return;
            }

            // enable the effect component
            switch (GetObjectType())
            {
                // enable the shader
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.gameObject.SetActive(true);
                    break;
                // enable "Image" component
                case JCS_UnityObjectType.UI:
                    mImage.enabled = true;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    mSpriteRenderer.enabled = true;
                    break;
            }

            switch (type)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        mAlpha = 1.0f;
                        this.mVisible = false;
                    }
                    break;
                case JCS_FadeType.FADE_IN:
                    {
                        mAlpha = 0.0f;

                        
                        this.mVisible = true;
                    }
                    break;
            }

            this.mFadeTime = time;
            this.mFadeType = type;
            this.mEffect = true;
        }
        public override void UpdateUnityData()
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer = this.GetComponent<Renderer>();
                    this.mRecordColor = this.transform.GetComponent<Renderer>().material.color;
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    this.mRecordColor = this.transform.GetComponent<Image>().color;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
                    this.mRecordColor = this.mSpriteRenderer.color;
                    break;
            }
        }

    }
}
