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
#if (UNITY_STANDALONE)      // for PC only
    /// <summary>
    /// Play a video and jump into a scene.
    /// 
    /// PC version.
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

        [Tooltip("Moive playback.")]
        [SerializeField]
        private MovieTexture mMovieTexture = null;

        [Tooltip("")]
        [SerializeField]
        private AudioSource mAudioSource = null;

        [Tooltip("After play once the moive load the next scene?")]
        [SerializeField]
        private bool mLoadSceneAfterPlay = true;

        [Tooltip("Next scene will be loaded.")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulateScene";


        [Header("- Delay Playback")]

        [Tooltip("How long it delay before to play the clip.")]
        [SerializeField]
        [Range(0, 3000)]
        private float mDelayPlayTime = 0;

        // timer to delay to play the clip.
        private float mDelayPlayTimer = 0;

        // check if the clip played?
        private bool mClipPlayed = false;

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
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoDelayPlayClip();

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

            if (mMovieTexture.isPlaying)
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
            // if the clip have not played, skip.
            if (!mClipPlayed)
                return;

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

        /// <summary>
        /// Delay amount of time then play the video
        /// </summary>
        private void DoDelayPlayClip()
        {
            if (mClipPlayed)
                return;

            mDelayPlayTimer += Time.deltaTime;

            if (mDelayPlayTimer < mDelayPlayTime)
                return;

            // play the video
            Play();

            mClipPlayed = true;
        }

    }
#else                       // for phone and other device.
    /// <summary>
    /// Play a video and jump into a scene.
    /// 
    /// Mobile version.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_VideoPlayer
        : JCS_UnityObject
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables (JCS_VideoPlayer) **")]

        [Tooltip("Full path of the clip")]
        [SerializeField]
        private string mFullPath = "";

        [Tooltip("")]
        [SerializeField]
        private AudioSource mAudioSource = null;


        [Header("** Runtime Variables (JCS_VideoPlayer) **")]

        [Tooltip("Moive playback.")]
        [SerializeField]
        private string mMovieName = "";

        [Tooltip("Extension of the movie. If there is one.")]
        [SerializeField]
        private string mMovieExtension = ".mp4";
        
        [Tooltip("Type of input handle during clip playing.")]
        [SerializeField]
        private FullScreenMovieControlMode mFullScreenMovieControlMode = FullScreenMovieControlMode.Full;

        [Tooltip("Background color.")]
        [SerializeField]
        private Color mBGColor = Color.black;

        [Tooltip("Next scene will be loaded.")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulateScene";

        [Header("- Delay Playback")]

        [Tooltip("How long it delay before to play the clip.")]
        [SerializeField]
        [Range(0, 3000)]
        private float mDelayPlayTime = 0;

        // timer to delay to play the clip.
        private float mDelayPlayTimer = 0;

        // check if the clip played?
        private bool mClipPlayed = false;

        [Header("*Usage: plz fill the clip time until next scene.")]

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField] [Range(0, 59)]
        private float mClipHour = 0;

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField] [Range(0, 59)]
        private float mClipMinute = 0;

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField] [Range(0, 59)]
        private float mClipSecond = 0;

        // real time = mClipHour * 3600 + mClipMinute * 60 + mClipSecond
        private float mLoadNextSceneTime = 1;

        // timer to load next scene
        private float mLoadNextSceneTimer = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool loop
        {
            get
            {
                JCS_Debug.LogError(
                    this, "Loop does not work in Andriod Platform...");

                return false;
            }
            set
            {
                JCS_Debug.LogError(
                    this, "Loop does not work in Andriod Platform...");
            }
        }
        public AudioSource AudioSource { get { return this.mAudioSource; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAudioSource = this.GetComponent<AudioSource>();

            UpdateUnityData();

            // load the next scene.
            mLoadNextSceneTime = 
                (mClipHour * 3600) + 
                (mClipMinute * 60) + 
                mClipSecond;
        }


        private void Update()
        {
            DoDelayPlayClip();

            LoadNextScene();
        }

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
            // get the full path of the clip
            mFullPath =
                mMovieName + 
                mMovieExtension;

            // play the screen movie.
            Handheld.PlayFullScreenMovie(
               mFullPath,
               mBGColor,
               mFullScreenMovieControlMode);
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void Pause()
        {
            JCS_Debug.LogError(
                    this, "Pause does not work in Andriod Platform...");
        }

        /// <summary>
        /// Stop playing video.
        /// </summary>
        public void Stop()
        {
            JCS_Debug.LogError(
                    this, "Stop does not work in Andriod Platform...");
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
            if (mClipPlayed)
                return;

            // start the load scene timer.
            mLoadNextSceneTimer += Time.deltaTime;

            // check if the clips ends.
            if (mLoadNextSceneTimer < mLoadNextSceneTime)
                return;

            // load the scene.
            JCS_SceneManager.instance.LoadScene(mSceneName);
        }

        /// <summary>
        /// Delay amount of time then play the video
        /// </summary>
        private void DoDelayPlayClip()
        {
            if (mClipPlayed)
                return;

            mDelayPlayTimer += Time.deltaTime;

            if (mDelayPlayTimer < mDelayPlayTime)
                return;

            // play the video
            Play();

            mClipPlayed = true;
        }

    }
#endif
}
