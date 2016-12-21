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
    /// 
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_Webcam 
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mDetectDevice = true;

        private string mDeviceName = "";
        private WebCamTexture mWebCamTexture = null;
        private int mCaptureCounter = 0;

        [Header("** Runtime Variables **")]
        [SerializeField] private float mResumeTime = 0;
        private float mResumeTimer = 0;
        private bool mResumeTrigger = false;

        [Header("** Key Settings **")]
        [SerializeField] private KeyCode mTakePic = KeyCode.None;

        [Header("** Image Path **")]
        [Tooltip("Webcam u take will save into this path.")]
        [SerializeField] private string mSavePath = "/JCS_GameData/WebcamShot/"; //Change the path here!

        [Header("** Resolution **")]
        [SerializeField] private int mWebcamResolutionWidth = 1920;
        [SerializeField] private int mWebcamResolutionHeight = 1080;


        [Header("** Effect **")]
        [SerializeField] private bool mSplash = true;
        private bool mSplashEffectTrigger = false;
        [SerializeField] private float mDelay = 1.0f;
        private float mDelayTimer = 0;

        [Header("** Sound Settings **")]
        [SerializeField]
        private AudioClip mTakePhotoSound = null;
        private JCS_SoundPlayer mSoundPlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void Stop() { this.mWebCamTexture.Stop(); }
        public void Play() { this.mWebCamTexture.Play(); }
        public void Pause() { this.mWebCamTexture.Pause(); }
        public bool isPlaying { get { return this.mWebCamTexture.isPlaying; } }
        
        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            ActiveWebcam();
        }

        private void Update()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            ProcessInput();
#endif

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

                if (mDelayTimer > mDelay)
                {
                    mDelayTimer = 0;
                    JCS_SceneManager.instance.GetJCSWhiteScreen().FadeOut();
                    mSplashEffectTrigger = false;
                }
            }
        }

        private void OnApplicationQuit()
        {
            // force stop
            if (mWebCamTexture != null)
                Stop();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
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

            mDeviceName = devices[0].name;
            mWebCamTexture = new WebCamTexture(mDeviceName, mWebcamResolutionWidth, mWebcamResolutionHeight, 12);
            UpdateUnityData();

            mWebCamTexture.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TakeSnapshotWebcam()
        {
            // No device detected!!
            // cannot take snap shot without the device!!
            if (!mDetectDevice)
            {
                JCS_Debug.JcsErrors("JCS_Webcam",   "No webcam detected in the current devices.");
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
        /// 
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

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
#if (UNITY_STANDALONE || UNITY_EDITOR)
        private void ProcessInput()
        {
            if (JCS_Input.GetKeyDown(mTakePic))
            {
                TakeSnapshotWebcam();
            }
        }
#endif

    }
}
