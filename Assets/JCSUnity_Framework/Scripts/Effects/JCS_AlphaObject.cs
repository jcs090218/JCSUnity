/**
 * $File: $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{
    public class JCS_AlphaObject : MonoBehaviour
    {
        enum FadeObjectType
        {
            GAME_OBJECT,
            UI
        };


        [SerializeField] private FadeObjectType mFadeObjectType = FadeObjectType.GAME_OBJECT;
        private JCS_FadeType mFadeType = JCS_FadeType.FADE_IN;  // defaul as visible

        private float mAlpha = 1.0f;
        [SerializeField] private float mFadeTime = 1.0f;
        [SerializeField] private bool mOverriteFade = false;

        private Color mRecordColor;
        private Image mImage = null;

        private bool mEffect = false;
        private bool mVisible = true;

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
            switch (mFadeObjectType)
            {
                case FadeObjectType.GAME_OBJECT:
                    
                    break;
                case FadeObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    break;
            }

            if (mFadeObjectType == FadeObjectType.GAME_OBJECT)
                this.mRecordColor = this.transform.GetComponent<Renderer>().material.color;
            else if (mFadeObjectType == FadeObjectType.UI)
                this.mRecordColor = this.transform.GetComponent<Image>().color;
        }

        private void Update()
        {

            if (mFadeObjectType == FadeObjectType.GAME_OBJECT &&
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
                            switch (mFadeObjectType)
                            {
                                case FadeObjectType.GAME_OBJECT:
                                    this.gameObject.SetActive(false);
                                    break;
                                case FadeObjectType.UI:
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

            switch (mFadeObjectType)
            {
                case FadeObjectType.GAME_OBJECT:
                    this.transform.GetComponent<Renderer>().material.color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
                case FadeObjectType.UI:
                    this.transform.GetComponent<Image>().color = new Color(mRecordColor.r, mRecordColor.g, mRecordColor.b, mAlpha);
                    break;
            }

        }

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
            switch (mFadeObjectType)
            {
                // enable the shader
                case FadeObjectType.GAME_OBJECT:
                    this.gameObject.SetActive(true);
                    break;
                // enable "Image" component
                case FadeObjectType.UI:
                    mImage.enabled = true;
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
    }
}
