/**
 * $File: JCS_Webcam.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;

namespace JCSUnity
{
    public class JCS_Webcam : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mDetectDevice = true;

        private string mDeviceName = "";
        private WebCamTexture mWebCamTexture = null;
        private int mCaptureCounter = 0;

        [Header("** Image Path **")]
        [SerializeField] private string mSavePath = "/JCS_GameData/WebcamShot/"; //Change the path here!

        [Header("** Resolution **")]
        [SerializeField] private int mWebcamResolutionWidth = 1920;
        [SerializeField] private int mWebcamResolutionHeight = 1080;


        [Header("** Effect **")]
        [SerializeField] private bool mSplash = true;
        private bool mSplashEffectTrigger = false;
        [SerializeField] private float mDelay = 1.0f;
        private float mDelayTimer = 0;

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
            this.GetComponent<Renderer>().material.mainTexture = mWebCamTexture;
            mWebCamTexture.Play();
        }

        private void Update()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)

            if (JCS_Input.GetKeyDown(KeyCode.T))
            {
                TakeSnapshotWebcam();
            }
#endif 

            if (!mSplashEffectTrigger)
                return;

            mDelayTimer += Time.deltaTime;

            if (mDelayTimer > mDelay)
            {
                mDelayTimer = 0;
                JCS_SceneManager.instance.GetJCSWhiteScreen().FadeOut();
                mSplashEffectTrigger = false;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void TakeSnapshotWebcam()
        {
            // No device detected!!
            // cannot take snap shot without the device!!
            if (!mDetectDevice)
            {
                JCS_GameErrors.JcsErrors("JCS_Webcam", -1, "No webcam detected in the current devices.");
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
                    JCS_ButtonFunctions.PopJCSWhiteScreen();

                sm.GetJCSWhiteScreen().MoveToTheLastChild();
                sm.GetJCSWhiteScreen().FadeIn();

                // do the snap shot effect
                mSplashEffectTrigger = true;
            }

            // Stop the camera
            mWebCamTexture.Stop();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
