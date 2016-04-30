/**
 * $File: JCS_GameWindowHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_GameWindowHandler : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private static JCS_GameWindowHandler instance = null;

        // System UI
        [Header("** System Dialogue **")]
        [SerializeField] private Transform[] mSystemUI = null;

        // Game UI
        [Header("** Game User Interface **")]
        [SerializeField] private Transform mGameUI = null;

        // List of all the Game Window we are going to use in the game
        [Header("** Game Window List **")]
        [SerializeField] private Transform[] mGameWindowList = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            if (instance != null)
            {
                // [IMPORTANT] Only accept one this object!!!
                DestroyImmediate(this.gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            PopSystemUI();
            PopGameUI();
            PopGameWindow();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void PopSystemUI()
        {
            if (mSystemUI.Length == 0)
            {
                JCS_GameErrors.JcsErrors("JCS_GameWindowHandler", -1, "No Game Window in the scene!");
                return;
            }

            JCS_DialogueObject jcs_do = null;

            foreach (Transform trans in mSystemUI)
            {
                if (trans == null)
                    continue;

                jcs_do = trans.GetComponent<JCS_DialogueObject>();

                if (jcs_do != null)
                {
                    JCS_UsefualFunctions.SpawnGameObject(trans);
                    jcs_do.HideDialogue();
                }
            }

        }
        private void PopGameUI()
        {
            if (mGameUI == null)
                return;

            JCS_DialogueObject jcs_do = mGameUI.GetComponent<JCS_DialogueObject>();

            if (jcs_do != null)
            {
                JCS_UsefualFunctions.SpawnGameObject(mGameUI);
                jcs_do.ShowDialogue();
                jcs_do.SetKeyCode(KeyCode.None);
            }
        }
        private void PopGameWindow()
        {
            if (mGameWindowList.Length == 0)
            {
                JCS_GameErrors.JcsErrors("JCS_GameWindowHandler", -1, "No Game Window in the scene!");
                return;
            }

            JCS_DialogueObject jcs_do = null;

            foreach (Transform trans in mGameWindowList)
            {
                if (trans == null)
                    continue;

                jcs_do = trans.GetComponent<JCS_DialogueObject>();

                if (jcs_do != null)
                {
                    JCS_UsefualFunctions.SpawnGameObject(trans);
                    jcs_do.HideDialogue();
                }

            }
        }
    }
}
