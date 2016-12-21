/**
 * $File: JCS_VideoPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_VideoPlayer
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_VideoPlayer) **")]

        [Tooltip("")]
        [SerializeField]
        private MovieTexture mMovieTexture = null;

        [Tooltip("")]
        [SerializeField]
        private AudioSource mAudioSource = null;

        [Tooltip("After play once the moive load the next scene?")]
        [SerializeField]
        private bool mLoadSceneAfterPlay = true;

        [Tooltip("")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulateScene";

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool loop
        {
            get
            {
                if (mMovieTexture == null)
                    return false;

                return (mMovieTexture.loop && mAudioSource.loop);
            }
            set
            {
                if (mMovieTexture == null)
                    return;

                mMovieTexture.loop = value;
                mAudioSource.loop = value;
            }
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAudioSource = this.GetComponent<AudioSource>();

            UpdateUnityData();

            LocalMainTexture = mMovieTexture;

            if (mMovieTexture != null)
                mAudioSource.clip = mMovieTexture.audioClip;

            Play();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            // check if done playing 
            // the movie and load the next scene.
            LoadNextScene();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.P))
                Play();
            if (JCS_Input.GetKeyDown(KeyCode.O))
                Pause();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Play the video.
        /// </summary>
        public void Play()
        {
            if (mMovieTexture == null)
                return;

            mMovieTexture.Play();
            mAudioSource.Play();
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void Pause()
        {
            if (mMovieTexture == null)
                return;

            mMovieTexture.Pause();
            mAudioSource.Pause();
        }

        /// <summary>
        /// Stop playing video.
        /// </summary>
        public void Stop()
        {
            if (mMovieTexture == null)
                return;

            mMovieTexture.Stop();
            mAudioSource.Stop();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Check if the moive dont playing, 
        /// load the next scene.
        /// </summary>
        private void LoadNextScene()
        {
            if (!mLoadSceneAfterPlay)
                return;

            if (mMovieTexture == null)
                return;

            // if still playing
            if (mMovieTexture.isPlaying)
                return;

            // load the scene.
            JCS_SceneManager.instance.LoadScene(mSceneName);

            // unload the movie
            mMovieTexture = null;
        }

    }
}
