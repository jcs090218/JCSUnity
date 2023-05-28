/**
 * $File: JCS_PauseManager.cs $
 * $Date: 2017-02-24 06:10:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// If you are working some kind of game that need the pause 
    /// screen, you definitly need the pause manager to add it onto 
    /// "JCS_Manager" transform in the Hierarchy.
    /// </summary>
    public class JCS_PauseManager : JCS_Manager<JCS_PauseManager>
    {
        /* Variables */

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_PauseManager) **")]

        [Tooltip("Test this module?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to toggle game pause/unpause.")]
        [SerializeField]
        private KeyCode mToggleGamePause = KeyCode.P;
#endif

        [Header("** Check Variables (JCS_PauseManager) **")]

        [Tooltip("List of pause action in the scene.")]
        [SerializeField]
        private List<JCS_PauseAction> mPauseActions = null;

        [Header("** Runtime Variables (JCS_PauseManager) **")]

        [Tooltip(@"Time to resize the pause action list, in seconds.
ATTENTION: this will take certain of performance depends on the pause 
object you have in the list.")]
        [SerializeField]
        [Range(1, 60)]
        private float mResizePauseActionListTime = 20;

        // resize timer.
        private float mResizePauseActionListTimer = 0;

        [Header("** Runtime Variables (JCS_PauseManager) **")]

        [Tooltip("The default time scale.")]
        [SerializeField]
        private float mDefaultTimeScale = 1.0f;

        private float mTargetTimeScale = 1.0f;

        [Header("- Asymptotic (JCS_PauseManager)")]

        [Tooltip("Pause and unpause with asymptotic transition.")]
        [SerializeField]
        private bool mAsymp = false;

        [Tooltip("How fast the asymptotic transition?")]
        [SerializeField]
        [Range(0.001f, 30.0f)]
        private float mFriction = 0.2f;

        /* Setter & Getter */

        public List<JCS_PauseAction> PausesActions { get { return this.mPauseActions; } }
        public float ResizePauseActionListTime { get { return this.mResizePauseActionListTime; } set { this.mResizePauseActionListTime = value; } }
        public float DefaultTimeScale { get { return this.mDefaultTimeScale; } set { this.mDefaultTimeScale = value; } }
        public bool Asymp { get { return this.mAsymp; } set { this.mAsymp = value; } }
        public float Friction { get { return this.mFriction; } set { this.mFriction = value; } }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
#if UNITY_EDITOR
            TestPauseGame();
#endif

            ResizePauseActionListPeriodically();
            DoAsymp();
        }

#if UNITY_EDITOR
        private void TestPauseGame()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mToggleGamePause))
            {
                var gm = JCS_GameManager.instance;

                gm.GAME_PAUSE = !gm.GAME_PAUSE;
            }
        }
#endif

        /// <summary>
        /// Add the pause action to the list of pause action list, 
        /// in order to get manage by this "pause manager".
        /// </summary>
        public void AddActionToList(JCS_PauseAction pa)
        {
            mPauseActions.Add(pa);
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        public void Pause()
        {
            if (mAsymp)
                mTargetTimeScale = 0.0f;
            else
                Time.timeScale = 0.0f;

            PauseTheWholeGame(true);
        }

        /// <summary>
        /// Unpause the game.
        /// </summary>
        public void Unpause()
        {
            if (mAsymp)
                mTargetTimeScale = mDefaultTimeScale;
            else
                Time.timeScale = mDefaultTimeScale;

            PauseTheWholeGame(false);
        }

        /// <summary>
        /// Pause/Unpause the whole game.
        /// </summary>
        public void PauseTheWholeGame(bool act = true)
        {
            foreach (JCS_PauseAction pauseAction in mPauseActions)
            {
                if (pauseAction == null)
                    continue;


                // NOTE(jenchieh): the act should be opposite with the 
                // enable /disable.

                // e.g. 
                // 1) Pause the game = disable all behaviour
                pauseAction.EnableBehaviourInTheList(!act);
            }

            // resize the list once.
            RemoveNullRefInPauseActionList();
        }

        /// <summary>
        /// Remove all the null reference object from the list.
        /// </summary>
        private void RemoveNullRefInPauseActionList()
        {
            mPauseActions = JCS_Util.RemoveEmptySlotIncludeMissing(mPauseActions);
        }

        /// <summary>
        /// Resize pause action list in certain time.
        /// </summary>
        private void ResizePauseActionListPeriodically()
        {
            mResizePauseActionListTimer += Time.deltaTime;

            if (mResizePauseActionListTimer < mResizePauseActionListTime)
                return;

            // resize the list
            RemoveNullRefInPauseActionList();

            // reset timer.
            mResizePauseActionListTimer = 0;
        }

        /// <summary>
        /// Do asymptotic pause/unpase transition.
        /// </summary>
        private void DoAsymp()
        {
            if (!mAsymp)
                return;

            Time.timeScale += (mTargetTimeScale - Time.timeScale) / mFriction * Time.unscaledDeltaTime;
        }
    }
}
