/**
 * $File: JCS_Webcam.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Webcam object. Must have the texture on it in order to render.
    /// </summary>
    public class JCS_Webcam
        : JCS_UnityObject
    {
        /* Variables */

        private bool mDetectDevice = true;

        private string mDeviceName = "";

        private WebCamTexture mWebCamTexture = null;

        private int mCaptureCounter = 0;

        private float mResumeTimer = 0.0f;

        private bool mResumeTrigger = false;

        [Header("** Runtime Variables (JCS_Webcam) **")]

        [Tooltip("Manually preserve the size in scene.")]
        [SerializeField]
        private bool mManuallySetSize = false;

        [Tooltip("Make webcam maximize to the widest edge.")]
        [SerializeField]
        private bool mMustBeFullScreen = false;

        [Tooltip("FPS for webcam.")]
        [SerializeField]
        [Range(1, 120)]
        private int mFPS = 12;

        [Tooltip("After the screenshot is taken, how fast to resume the webcam.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mResumeTime = 1.0f;

        [Header("- Effect")]

        [Tooltip("Do the splash effect?")]
        [SerializeField]
        private bool mSplash = true;

        // Is the splash effect triggered?
        private bool mSplashEffectTrigger = false;

        [Tooltip("Delay time to fade out the splash screen.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mDelayTime = 1.0f;

        private float mDelayTimer = 0.0f;

#if (UNITY_STANDALONE || UNITY_EDITOR)
        [Header("- Keys")]

        [Tooltip("Key to take webcam image.")]
        [SerializeField]
        private KeyCode mTakePicKey = KeyCode.None;
#endif

        [Header("- Image")]

        [Tooltip("Image save path.")]
        [SerializeField]
        private string mSavePath = "/JCS_GameData/WebcamShot/"; //Change the path here!

        [Tooltip("Default save image file extension.")]
        [SerializeField]
        private string mSaveExtension = ".png";

        [Header("- Sound")]

        [Tooltip("Sound when taking the screenshot.")]
        [SerializeField]
        private AudioClip mTakePhotoSound = null;

        /* Setter & Getter */

        public bool ManuallySetSize { get { return this.mManuallySetSize; } set { this.mManuallySetSize = value; } }
        public bool MustBeFullScreen { get { return this.mMustBeFullScreen; } set { this.mMustBeFullScreen = value; } }
        public int FPS { get { return this.mFPS; } set { this.mFPS = value; } }
        public float ResumeTime { get { return this.mResumeTime; } set { this.mResumeTime = value; } }
        public float DelayTime { get { return this.mDelayTime; } set { this.mDelayTime = value; } }
#if (UNITY_STANDALONE || UNITY_EDITOR)
        public KeyCode TakePicKey { get { return this.mTakePicKey; } set { this.mTakePicKey = value; } }
#endif
        public string SavePath { get { return this.mSavePath; } set { this.mSavePath = value; } }
        public string SaveExtension { get { return this.mSaveExtension; } set { this.mSaveExtension = value; } }
        public bool isPlaying { get { return this.mWebCamTexture.isPlaying; } }
        public AudioClip TakePhotoSound { get { return this.mTakePhotoSound; } set { this.mTakePhotoSound = value; } }

        /* Functions */

        private void Start()
        {
            ActiveWebcam();
        }

        private void Update()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            ProcessInput();
#endif

            DoSplash();
            DoAspect();
        }

        private void OnApplicationQuit()
        {
            // force stop
            Stop();
        }

        private void OnDestroy()
        {
            Stop();
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Stop()
        {
            if (mWebCamTexture == null)
                return;

            this.mWebCamTexture.Stop();
            this.mWebCamTexture = null;

            SetWebcamTexture(null);
        }
        public void Play()
        {
            this.mWebCamTexture.Play();
        }
        public void Pause()
        {
            this.mWebCamTexture.Pause();
        }

        /// <summary>
        /// Active the webcam.
        /// </summary>
        public void ActiveWebcam()
        {
            WebCamDevice[] devices = WebCamTexture.devices;

            // check if there is webcam avaliable in the platform
            if (devices.Length == 0)
            {
                // no device detected!
                mDetectDevice = false;
                return;
            }

            var scs = JCS_ScreenSettings.instance;
            int screenWidth = scs.STANDARD_SCREEN_WIDTH;
            int screenHeight = scs.STANDARD_SCREEN_HEIGHT;

            mDeviceName = devices[0].name;
            mWebCamTexture = new WebCamTexture(mDeviceName, screenWidth, screenHeight, mFPS);
            UpdateUnityData();

            Play();
        }

        /// <summary>
        /// Take a snapshot and store image to data path.
        /// </summary>
        public void TakeSnapshotWebcam()
        {
            // No device detected!!
            // cannot take snap shot without the device!!
            if (!mDetectDevice)
            {
                JCS_Debug.LogError("No webcam detected in the current devices");
                return;
            }

            string fullPath = Application.dataPath + mSavePath;

            // if Directory does not exits, create it prevent error!
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            Texture2D snap = new Texture2D(mWebCamTexture.width, mWebCamTexture.height);
            snap.SetPixels(mWebCamTexture.GetPixels());
            snap.Apply();

            System.IO.File.WriteAllBytes(fullPath + mCaptureCounter.ToString() + mSaveExtension, snap.EncodeToPNG());
            ++mCaptureCounter;


            if (mSplash)
            {
                JCS_SceneManager sm = JCS_SceneManager.instance;

                if (sm.GetJCSWhiteScreen() == null)
                    JCS_UtilityFunctions.PopJCSWhiteScreen();

                sm.GetJCSWhiteScreen().FadeIn();

                // do the snap shot effect
                mSplashEffectTrigger = true;
            }

            // Stop the camera
            mWebCamTexture.Stop();

            // start the timer wait for resume
            mResumeTrigger = true;

            // play sound.
            {
                var soundm = JCS_SoundManager.instance;
                JCS_SoundPlayer sp = soundm.GetGlobalSoundPlayer();
                sp.PlayOneShot(mTakePhotoSound);
            }
        }

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
        public override void UpdateUnityData()
        {
            base.UpdateUnityData();

            SetWebcamTexture(mWebCamTexture);
        }

        /// <summary>
        /// Set the webcam texture.
        /// </summary>
        /// <param name="tex"> Texture you want to be set. </param>
        private void SetWebcamTexture(WebCamTexture tex)
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer.material.mainTexture = tex;
                    break;
            }
        }

#if (UNITY_STANDALONE || UNITY_EDITOR)
        /// <summary>
        /// Process the input.
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetKeyDown(mTakePicKey))
            {
                TakeSnapshotWebcam();
            }
        }
#endif

        /// <summary>
        /// Do splash effect.
        /// </summary>
        private void DoSplash()
        {
            if (mResumeTrigger)
            {
                mResumeTimer += Time.deltaTime;

                if (mResumeTime < mResumeTimer)
                {
                    if (mWebCamTexture != null)
                        mWebCamTexture.Play();

                    // done resume.
                    mResumeTrigger = false;
                }
            }

            if (mSplashEffectTrigger)
            {
                mDelayTimer += Time.deltaTime;

                if (mDelayTimer > mDelayTime)
                {
                    mDelayTimer = 0;
                    JCS_SceneManager.instance.GetJCSWhiteScreen().FadeOut();
                    mSplashEffectTrigger = false;
                }
            }
        }

        /// <summary>
        /// Do aspect ratio calculation to webcam.
        /// </summary>
        private void DoAspect()
        {
            if (mWebCamTexture == null)
                return;

            if (mManuallySetSize)
                return;

            if (!mDetectDevice)
            {
                JCS_Debug.LogError("No webcam detected in the current devices");
                return;
            }

            if (GetRectTransform() != null)
            {
                var scs = JCS_ScreenSettings.instance;

                float screenWidth = scs.STANDARD_SCREEN_WIDTH;
                float screenHeight = scs.STANDARD_SCREEN_HEIGHT;

                float xRatio = screenWidth / (float)mWebCamTexture.width;
                float yRatio = screenHeight / (float)mWebCamTexture.height;

                float width = 0;
                float height = 0;

                bool mode = (mMustBeFullScreen) ? (screenWidth > screenHeight) : (screenWidth < screenHeight);

                if (mode)
                {
                    width = screenWidth;
                    height = (float)mWebCamTexture.height * xRatio;
                }
                else
                {
                    width = (float)mWebCamTexture.width * yRatio;
                    height = screenHeight;
                }

                this.GetRectTransform().sizeDelta = new Vector2(width, height);
            }

            float scaleY = mWebCamTexture.videoVerticallyMirrored ? -1f : 1f;
            {
                Vector3 newScale = this.LocalScale;
                newScale.y *= scaleY;
                this.LocalScale = newScale;
            }

            int orient = -this.mWebCamTexture.videoRotationAngle;
            this.LocalEulerAngles = new Vector3(0.0f, 0.0f, orient);
        }
    }
}
