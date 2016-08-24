/**
 * $File: JCS_SceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_SceneManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_SceneManager instance = null;

        [SerializeField]
        private JCS_SwitchSceneType mType = JCS_SwitchSceneType.BLACK_SCREEN;

        //----------------------
        // Private Variables
        private bool mSwitchSceneEffect = false;
        private string mNextSceneName = "";

        // Async loading scene operation. (thread)
        private AsyncOperation mAsyncOperation = null;

        [Header("** Black Screen Settings **")]
        [SerializeField] private JCS_BlackScreen mJCSBlackScreen = null;

        [Header("** White Screen Settings **")]
        [Tooltip("Do u need the white screen in ur game?")]
        [SerializeField] private bool mPopWhiteScreen = false;
        [SerializeField] private JCS_WhiteScreen mJCSWhiteScreen = null;

        [Header("** General Screen Settings **")]
        [SerializeField] private float mFadeInTime = 1.0f;
        [SerializeField] private float mFadeOutTime = 1.0f;

        // fade the sound while switching the scene.
        private JCS_FadeSound mJCSFadeSound = null;

        // Scene in the game so is dynamic instead of Unity's scene system's scene
        // Unity 自帶就有Scene這個物件. 在這個Unity Scene裡面自己宣告Scene
        // 比起Unity的Scene比較動態, 因為自己宣告的操控比較多.
        private JCS_DynamicScene mDynamicScene = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_DynamicScene GetDynamicScene() { return this.mDynamicScene; }
        public void SetDynamicScene(JCS_DynamicScene ds) { this.mDynamicScene = ds; }
        public void SetJCSBlackScreen(JCS_BlackScreen bs) { this.mJCSBlackScreen = bs; }
        public void SetJCSWhiteScreen(JCS_WhiteScreen ws) { this.mJCSWhiteScreen = ws; }
        public JCS_WhiteScreen GetJCSWhiteScreen() { return this.mJCSWhiteScreen; }
        private JCS_BlackScreen GetJCSBlackScreen() { return this.mJCSBlackScreen; }
        public float SceneFadeInTime { get { return this.mFadeInTime; } set { this.mFadeInTime = value; } }
        public float SceneFadeOutTime { get { return this.mFadeOutTime; } set { this.mFadeOutTime = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;

            switch (mType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    JCS_ButtonFunctions.PopJCSBlackScreen();
                    break;
            }

            // Pop white screen depends on game needs.
            if (mPopWhiteScreen)
                JCS_ButtonFunctions.PopJCSWhiteScreen();

#if (UNITY_EDITOR)
            // add the tool in editor mode.
            this.gameObject.AddComponent<ReadSceneNames>();
#endif
        }

        private void Start()
        {
            mJCSBlackScreen.FadeOut(mFadeOutTime);

            if (JCS_SoundSettings.instance.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
            {
                mJCSFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();
                mJCSFadeSound.SetAudioSource(JCS_SoundManager.instance.GetBackgroundMusic());
                mJCSFadeSound.FadeIn(JCS_GameSettings.GetBGM_Volume(), mFadeOutTime);
            }

            // the game is loaded start the game agian
            JCS_GameManager.instance.GAME_PAUSE = false;
        }

        private void Update()
        {

            if (!mSwitchSceneEffect)
                return;

            if (mJCSBlackScreen.IsFadeIn())
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

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// Load the scene with default fade in time
        /// </summary>
        /// <param name="sceneName"> scene name to load </param>
        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, mFadeInTime);
        }
        /// <summary>
        /// Load scene with self-define fade in time.
        /// </summary>
        /// <param name="sceneName"> scene name to load </param>
        /// <param name="fadeInTime"> time to fade in </param>
        public void LoadScene(string sceneName, float fadeInTime)
        {
#if (UNITY_EDITOR)
            // only do this in Editor Mode, 
            // this help level designer to do their job.
            if (!ReadSceneNames.CheckSceneAvailable(sceneName))
            {
                JCS_GameErrors.JcsReminders(this,
                    "Scene [" + sceneName + "] u want to load is not in the Build Setting...");

                return;
            }
#endif

            // if is loading already, dont load it agian
            if (mSwitchSceneEffect)
                return;

            // set the next scene name
            this.mNextSceneName = sceneName;

            JCS_GameSettings gs = JCS_GameSettings.instance;
            if (gs.SAVE_ON_SWITCH_SCENE &&
                gs.SAVE_GAME_DATA_FUNC != null)
            {
                // do the saving.
                gs.SAVE_GAME_DATA_FUNC.Invoke();
            }

            // preload the scene
            mAsyncOperation = SceneManager.LoadSceneAsync(mNextSceneName);
            mAsyncOperation.allowSceneActivation = false;

            // move to the last child in order
            // to render the black screen in front of 
            // any UI's GUI
            mJCSBlackScreen.MoveToTheLastChild();

            // start fading in (black screen)
            mJCSBlackScreen.FadeIn(fadeInTime);

            // start fading sound
            if (JCS_SoundSettings.instance.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
            {
                mJCSFadeSound.SetAudioSource(JCS_SoundManager.instance.GetBackgroundMusic());

                // fade out sound to zero
                mJCSFadeSound.FadeOut(0, fadeInTime);
            }

            // start check to switch scene or not
            mSwitchSceneEffect = true;

            // Pause the game, until the scene is loaded
            JCS_GameManager.instance.GAME_PAUSE = true;
        }

        /// <summary>
        /// Check is loading the scene or not.
        /// </summary>
        /// <returns> return result. </returns>
        public bool IsSwitchingScene()
        {
            return mSwitchSceneEffect;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
