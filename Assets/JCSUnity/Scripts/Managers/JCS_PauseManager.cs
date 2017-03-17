/**
 * $File: JCS_PauseManager.cs $
 * $Date: 2017-02-24 06:10:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// If you are working some kind of game that need the pause 
    /// screen, you definitly need the pause manager to add it onto 
    /// "JCS_Managers" transform in the Hierarchy.
    /// </summary>
    public class JCS_PauseManager
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PauseManager instance = null;

        //----------------------
        // Private Variables

        [Header("** Check Variables (JCS_PauseManager) **")]

#if (UNITY_EDITOR)
        [Tooltip("List of pause action in the scene.")]
        [SerializeField]
        private List<JCS_PauseAction> mPauseActions = null;
#else
        // use link list is faster than a just a list.
        private LinkedList<JCS_PauseAction> mPauseActions = null;
#endif

        [Tooltip(@"Time to resize the pause action list, in seconds.
ATTENTION: this will take certain of performance depends on the pause 
object you have in the list.")]
        [SerializeField]
        [Range(1, 60)]
        private float mResizePauseActionListTime = 20;

        // resize timer.
        private float mResizePauseActionListTimer = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
#if (UNITY_EDITOR)
        public List<JCS_PauseAction> PausesActions { get { return this.mPauseActions; } }
#else
        public LinkedList<JCS_PauseAction> PausesActions { get { return this.mPauseActions; } }
#endif

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            ResizePauseActionListPeriodically();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Add the pause action to the list of pause action list, 
        /// in order to get manage by this "pause manager".
        /// </summary>
        public void AddActionToList(JCS_PauseAction pa)
        {
#if (UNITY_EDITOR)
            mPauseActions.Add(pa);
#else
            mPauseActions.AddLast(pa);
#endif
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

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Remove all the null reference object from the lis.t
        /// </summary>
        private void RemoveNullRefInPauseActionList()
        {
#if (UNITY_EDITOR)
            for (int index = 0;
                index < mPauseActions.Count;
                ++index)
            {
                // remove itself.
                if (mPauseActions[index] == null)
                    mPauseActions.RemoveAt(index);
            }
#else
            foreach (JCS_PauseAction pauseAction in mPauseActions)
            {
                // remove itself.
                if (pauseAction == null)
                    mPauseActions.Remove(pauseAction);
            }
#endif
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

    }
}
