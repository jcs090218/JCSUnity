/**
 * $File: JCS_SceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{

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
        private AsyncOperation mAsyncOperation = null;

        [SerializeField] private JCS_BlackScreen mJCSBlackScreen = null;
        [SerializeField] private JCS_WhiteScreen mJCSWhiteScreen = null;

        [SerializeField] private float mFadeInTime = 1.0f;
        [SerializeField] private float mFadeOutTime = 1.0f;

        private JCS_FadeSound mJCSFadeSound = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
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
                mSwitchSceneEffect = false;

                // load the scene if is ready
                mAsyncOperation.allowSceneActivation = true;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void LoadScene(string sceneName)
        {
            // if is loading already, dont load it agian
            if (mSwitchSceneEffect)
                return;

            // set the next scene name
            this.mNextSceneName = sceneName;

            // preload the scene
            mAsyncOperation = SceneManager.LoadSceneAsync(mNextSceneName);
            mAsyncOperation.allowSceneActivation = false;

            // move to the last child in order
            // to render the black screen in front of 
            // any UI's GUI
            mJCSBlackScreen.MoveToTheLastChild();

            // start fading in (black screen)
            mJCSBlackScreen.FadeIn(mFadeInTime);

            // start fading sound
            if (JCS_SoundSettings.instance.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
            {
                mJCSFadeSound.SetAudioSource(JCS_SoundManager.instance.GetBackgroundMusic());

                // fade out sound to zero
                mJCSFadeSound.FadeOut(0, mFadeInTime);
            }

            // start check to switch scene or not
            mSwitchSceneEffect = true;

            // Pause the game, until the scene is loaded
            JCS_GameManager.instance.GAME_PAUSE = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
