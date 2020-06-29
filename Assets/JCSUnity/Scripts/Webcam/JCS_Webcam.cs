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

        [Header("** Check Variables (JCS_Webcam) **")]

        [Tooltip("Record down the screen width.")]
        [SerializeField]
        private int mWebcamWidth = 0;

        [Tooltip("Record down the screen height.")]
        [SerializeField]
        private int mWebcamHeight = 0;

        [Header("** Runtime Variables (JCS_Webcam) **")]

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

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when taking the screenshot.")]
        [SerializeField]
        private AudioClip mTakePhotoSound = null;

        /* Setter & Getter */

        public bool isPlaying { get { return this.mWebCamTexture.isPlaying; } }
        public float ResumeTime { get { return this.mResumeTime; } set { this.mResumeTime = value; } }
        public float DelayTime { get { return this.mDelayTime; } set { this.mDelayTime = value; } }
        public KeyCode TakePicKey { get { return this.mTakePicKey; } set { this.mTakePicKey = value; } }
        public string SavePath { get { return this.mSavePath; } set { this.mSavePath = value; } }
        public string SaveExtension { get { return this.mSaveExtension; } set { this.mSaveExtension = value; } }
        public AudioClip TakePhotoSound { get { return this.mTakePhotoSound; } set { this.mTakePhotoSound = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mSoundPlayer == null)
                mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

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

        public void Stop() { if (mWebCamTexture != null) this.mWebCamTexture.Stop(); }
        public void Play() { this.mWebCamTexture.Play(); }
        public void Pause() { this.mWebCamTexture.Pause(); }

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
            mWebcamWidth = scs.STANDARD_SCREEN_WIDTH;
            mWebcamHeight = scs.STANDARD_SCREEN_HEIGHT;

            mDeviceName = devices[0].name;
            mWebCamTexture = new WebCamTexture(mDeviceName, mWebcamWidth, mWebcamHeight, 12);
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

            System.IO.File.WriteAllBytes(fullPath + mCaptureCounter.ToString() + ".png", snap.EncodeToPNG());
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
            mSoundPlayer.PlayOneShot(mTakePhotoSound);
        }

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
        public override void UpdateUnityData()
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer = this.GetComponent<Renderer>();

                    // set texture to webcam texture (Mesh)
                    this.mRenderer.material.mainTexture = mWebCamTexture;
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    this.mRectTransform = this.GetComponent<RectTransform>();

                    // set texture to webcam texture (UI)
                    this.mImage.material.mainTexture = mWebCamTexture;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();

                    // set texture to webcam texture (Sprite)
                    this.mSpriteRenderer.material.mainTexture = mWebCamTexture;
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
            if (!mDetectDevice)
            {
                JCS_Debug.LogError("No webcam detected in the current devices");
                return;
            }

            float xRatio = (float)mWebcamWidth / (float)mWebCamTexture.width;
            float width = mWebcamWidth;
            float height = (float)mWebCamTexture.height * xRatio;
            this.GetRectTransform().sizeDelta = new Vector2(width, height);

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
