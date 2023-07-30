﻿/**
 * $File: JCS_SceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manage scenes changes.
    /// </summary>
    public class JCS_SceneManager : JCS_Manager<JCS_SceneManager>
    {
        /* Variables */

        private bool mSwitchSceneEffect = false;
        private string mNextSceneName = "";

        // Async loading scene operation. (thread)
        private AsyncOperation mAsyncOperation = null;

        [Separator("Check Variables (JCS_SceneManager)")]

        [SerializeField]
        [ReadOnly]
        private JCS_BlackScreen mBlackScreen = null;

        [SerializeField]
        [ReadOnly]
        private JCS_BlackSlideScreen mBlackSlideScreen = null;

        [Tooltip("Which direction to fade slide.")]
        [SerializeField]
        [ReadOnly]
        private JCS_Align mAlign = JCS_Align.ALIGN_LEFT;

        [Tooltip("Do u need the white screen in ur game?")]
        [SerializeField]
        [ReadOnly]
        private bool mPopWhiteScreen = false;

        [SerializeField]
        [ReadOnly]
        private JCS_WhiteScreen mWhiteScreen = null;

        [Separator("Initialize Variables (JCS_SceneManager)")]

        [Tooltip("Type/Method to switch the scene.")]
        [SerializeField]
        private JCS_SwitchSceneType mSwitchSceneType = JCS_SwitchSceneType.BLACK_SCREEN;

        [Separator("Runtime Variables (JCS_SceneManager)")]

        [Header("- Screen")]

        [Tooltip("Do this scene using the specific setting.")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Fade in time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mSceneFadeInTime = 1.0f;

        [Tooltip("Fade out time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mSceneFadeOutTime = 1.0f;

        // fade the sound while switching the scene.
        private JCS_FadeSound mJCSFadeSound = null;

        // Scene in the game so is dynamic instead of Unity's scene system's scene
        // Unity 自帶就有Scene這個物件. 在這個Unity Scene裡面自己宣告Scene
        // 比起Unity的Scene比較動態, 因為自己宣告的操控比較多.
        private JCS_DynamicScene mDynamicScene = null;

        /* Setter & Getter */

        public JCS_SwitchSceneType SwitchSceneType { get { return this.mSwitchSceneType; } set { this.mSwitchSceneType = value; } }
        public JCS_DynamicScene GetDynamicScene() { return this.mDynamicScene; }
        public void SetDynamicScene(JCS_DynamicScene ds) { this.mDynamicScene = ds; }
        public void SetBlackScreen(JCS_BlackScreen bs) { this.mBlackScreen = bs; }
        public void SetBlackSlideScreen(JCS_BlackSlideScreen bs) { this.mBlackSlideScreen = bs; }
        public void SetWhiteScreen(JCS_WhiteScreen ws) { this.mWhiteScreen = ws; }
        public JCS_WhiteScreen GetWhiteScreen() { return this.mWhiteScreen; }
        public JCS_BlackScreen GetBlackScreen() { return this.mBlackScreen; }

        public bool OverrideSetting { get { return this.mOverrideSetting; } }
        public float SceneFadeInTime { get { return this.mSceneFadeInTime; } set { this.mSceneFadeInTime = value; } }
        public float SceneFadeOutTime { get { return this.mSceneFadeOutTime; } set { this.mSceneFadeOutTime = value; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        JCS_UtilityFunctions.PopJCSBlackScreen();
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        JCS_UtilityFunctions.PopJCSBlackSlideScreen();
                    }
                    break;
            }

            // Pop white screen depends on game needs.
            if (mPopWhiteScreen)
                JCS_UtilityFunctions.PopJCSWhiteScreen();

#if UNITY_EDITOR
            // add the tool in editor mode.
            this.gameObject.AddComponent<ReadSceneNames>();
#endif
        }

        private void Start()
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade out time base on  the scene setting
            // and scene manager specific.
            float fadeoutTime = ss.SceneFadeInTimeBaseOnSetting();

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        // get the current screen color.
                        mBlackScreen.LocalColor = ss.SCREEN_COLOR;

                        mBlackScreen.FadeOut(fadeoutTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        mBlackSlideScreen.StartSlideOut(mAlign, fadeoutTime);
                    }
                    break;
            }

            // Only need to fade BGM when BGM is not switch between scene.
            var soundS = JCS_SoundSettings.instance;
            var soundM = JCS_SoundManager.instance;

            if (!soundS.KEEP_BGM_SWITCH_SCENE)
            {
                if (soundS.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
                {
                    // get the component.
                    if (mJCSFadeSound == null)
                        mJCSFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();

                    // set the background audio source.
                    mJCSFadeSound.SetAudioSource(
                        soundM.GetBGMAudioSource());

                    // active the fade sound in effect.
                    mJCSFadeSound.FadeIn(
                        soundS.GetBGM_Volume(),
                        /* Fade in the sound base on the setting. */
                        soundS.GetSoundFadeInTimeBaseOnSetting());
                }
            }
            else
            {
                // If the keep bgm is true, we disable it once  everytime a
                // scene is loaded.
                // 
                // ATTENTION(jenchieh): This should be place for the last use
                // of the variable 'KEEP_BGM_SWITCH_SCENE'.
                soundS.KEEP_BGM_SWITCH_SCENE = false;
            }

            // the game is loaded start the game agian
            JCS_GameManager.instance.GAME_PAUSE = false;
        }

        private void Update()
        {
            DoSwitchScene();
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        public void LoadScene(string sceneName)
        {
            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = JCS_SceneSettings.instance.SceneFadeInTimeBaseOnSetting();

            // load scene and pass the value in.
            LoadScene(sceneName, fadeInTime);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="fadeInTime"> Time to fade in. </param>
        public void LoadScene(string sceneName, float fadeInTime)
        {
            LoadScene(sceneName, fadeInTime, false);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        public void LoadScene(string sceneName, Color screenColor)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            LoadScene(sceneName, fadeInTime, screenColor, false);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void LoadScene(string sceneName, bool keepBGM)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            LoadScene(sceneName, fadeInTime, ss.SCREEN_COLOR, keepBGM);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="fadeInTime"> Time to fade in. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void LoadScene(string sceneName, float fadeInTime, bool keepBGM)
        {
            var sceneS = JCS_SceneSettings.instance;

            LoadScene(sceneName, fadeInTime, sceneS.SCREEN_COLOR, keepBGM);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void LoadScene(string sceneName, Color screenColor, bool keepBGM)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            LoadScene(sceneName, fadeInTime, screenColor, keepBGM);
        }

        /// <summary>
        /// Load the target scene.
        /// </summary>
        /// <param name="sceneName"> Scene name to load. </param>
        /// <param name="fadeInTime"> Time to fade in. </param>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void LoadScene(string sceneName, float fadeInTime, Color screenColor, bool keepBGM)
        {
#if UNITY_EDITOR
            // only do this in Editor Mode, 
            // this help level designer to do their job.
            if (!ReadSceneNames.CheckSceneAvailable(sceneName))
            {
                JCS_Debug.LogReminder("Scene [" + sceneName + "] you want to load is not in the Build Setting");
                return;
            }
#endif

            // if is loading already, dont load it agian
            if (mSwitchSceneEffect)
                return;

            // set the next scene name
            this.mNextSceneName = sceneName;

            var gs = JCS_GameSettings.instance;

            if (gs.SAVE_ON_SWITCH_SCENE && gs.SAVE_GAME_DATA_FUNC != null)
            {
                // do the saving.
                gs.SAVE_GAME_DATA_FUNC.Invoke();
            }

            // preload the scene
            mAsyncOperation = SceneManager.LoadSceneAsync(mNextSceneName);
            mAsyncOperation.allowSceneActivation = false;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        // move to the last child in order
                        // to render the black screen in front of 
                        // any UI's GUI
                        mBlackScreen.MoveToTheLastChild();

                        // set the screen color.
                        // NOTE(jenchieh): always start with opacity the same 
                        // as previous.
                        screenColor.a = mBlackScreen.LocalColor.a;
                        mBlackScreen.LocalColor = screenColor;

                        // record down the screen color.
                        JCS_SceneSettings.instance.SCREEN_COLOR = screenColor;

                        // start fading in (black screen)
                        mBlackScreen.FadeIn(fadeInTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        mBlackSlideScreen.MoveToTheLastChild();

                        mBlackSlideScreen.StartSlideIn(mAlign, fadeInTime);
                    }
                    break;
            }

            var ss = JCS_SoundSettings.instance;

            ss.KEEP_BGM_SWITCH_SCENE = keepBGM;

            if (!keepBGM)
            {
                // start fading sound
                if (ss.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
                {
                    // get the component.
                    if (mJCSFadeSound == null)
                        mJCSFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();

                    mJCSFadeSound.SetAudioSource(JCS_SoundManager.instance.GetBGMAudioSource());

                    // fade out sound to zero
                    mJCSFadeSound.FadeOut(0, fadeInTime);
                }
            }

            // start check to switch scene or not
            mSwitchSceneEffect = true;
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        public void ReloadScene()
        {
            // NOTE(jenchieh): get the fade in time base on 
            // the scene setting and scene manager specific.
            float fadeInTime = JCS_SceneSettings.instance.SceneFadeInTimeBaseOnSetting();

            // load scene and pass the value in.
            ReloadScene(fadeInTime);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="fadeInTime"> Time to fade in. </param>
        public void ReloadScene(float fadeInTime)
        {
            ReloadScene(fadeInTime, false);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        public void ReloadScene(Color screenColor)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            ReloadScene(fadeInTime, screenColor, false);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void ReloadScene(bool keepBGM)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            ReloadScene(fadeInTime, ss.SCREEN_COLOR, keepBGM);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="fadeInTime"> Time to fade in. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void ReloadScene(float fadeInTime, bool keepBGM)
        {
            var sceneS = JCS_SceneSettings.instance;

            ReloadScene(fadeInTime, sceneS.SCREEN_COLOR, keepBGM);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void ReloadScene(Color screenColor, bool keepBGM)
        {
            var ss = JCS_SceneSettings.instance;

            // NOTE(jenchieh): get the fade in time base on  the scene setting
            // and scene manager specific.
            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();

            ReloadScene(fadeInTime, screenColor, keepBGM);
        }

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        /// <param name="fadeInTime"> Time to fade in. </param>
        /// <param name="screenColor"> Screen color to fade in/out. </param>
        /// <param name="keepBGM"> Set to true if keep background music playing. </param>
        public void ReloadScene(float fadeInTime, Color screenColor, bool keepBGM)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            LoadScene(sceneName, fadeInTime, screenColor, keepBGM);
        }

        /// <summary>
        /// Check is loading the scene or not.
        /// </summary>
        /// <returns> return result. </returns>
        public bool IsSwitchingScene()
        {
            return this.mSwitchSceneEffect;
        }

        /// <summary>
        /// Do the async switch scene.
        /// </summary>
        private void DoSwitchScene()
        {
            // check if during the switch scene?
            if (!mSwitchSceneEffect)
                return;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        if (mBlackScreen.IsFadeIn())
                        {
                            // No need this anymore, since we have the
                            // to clean up everything before we load the scene.
                            // we need this boolean to check weather the event can
                            // spawn new "GameObject" when "OnDestroy" function was 
                            // called in Unity.
                            //mSwitchSceneEffect = false;

                            // load the scene if is ready
                            mAsyncOperation.allowSceneActivation = true;
                        }
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        if (mBlackSlideScreen.IsDoneSliding())
                        {
                            // load the scene if is ready
                            mAsyncOperation.allowSceneActivation = true;
                        }
                    }
                    break;
            }
        }
    }
}
