/**
 * $File: JCS_UIManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    public class JCS_UIManager : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_UIManager instance = null;

        //----------------------
        // Private Variables
        [SerializeField] private JCS_Canvas mJCSCanvas = null;
        // Game Play UI (Game Layer - Only One)
        [SerializeField] private JCS_DialogueObject mGameUI = null;          // the most common one!
        // System dialogue (Application Layer - Only One)
        [SerializeField] private JCS_DialogueObject mForceDialogue = null;

        // Game Dialogue (Game Layer - could have multiple one)
        // Dialogue player are focusing
        [SerializeField] private JCS_DialogueObject mFocusGameDialogue = null;
        private JCS_Vector<JCS_DialogueObject> mGameDialogues = null;

        private LinkedList<JCS_DialogueObject> mOpenWindow = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetJCSCanvas(JCS_Canvas can) { this.mJCSCanvas = can; }
        public JCS_Canvas GetJCSCanvas() { return this.mJCSCanvas; }
        public void SetJCSDialogue(JCS_DialogueType type, JCS_DialogueObject jdo)
        {
            switch (type)
            {
                case JCS_DialogueType.GAME_UI:
                    {
                        if (mGameUI != null)
                        {
                            JCS_GameErrors.JcsErrors("JCS_UIManager", -1, "Failed to set \"In Game Dialogue\"...");
                            return;
                        }

                        this.mGameUI = jdo;
                    }
                    break;
                case JCS_DialogueType.GAME_DIALOGUE:
                    {
                        //if (mFocusGameDialogue != null)
                        //{
                        //    JCS_GameErrors.JcsErrors("JCS_UIManager", -1, "Failed to set \"In Game Dialogue\"...");
                        //    return;
                        //}

                        this.mFocusGameDialogue = jdo;
                    }
                    break;
                case JCS_DialogueType.FORCE_DIALOGUE:
                    {
                        if (mForceDialogue != null)
                        {
                            JCS_GameErrors.JcsErrors("JCS_UIManager", -1, "Failed to set \"Force Dialogue\"...");
                            return;
                        }

                        mForceDialogue = jdo;
                    }
                    break;
            }
        }
        public JCS_DialogueObject GetJCSDialogue(JCS_DialogueType type)
        {
            switch (type)
            {
                // Game UI
                case JCS_DialogueType.GAME_UI:
                    return this.mGameUI;

                // Dialogue Box
                case JCS_DialogueType.GAME_DIALOGUE:
                    return this.mFocusGameDialogue;
                case JCS_DialogueType.FORCE_DIALOGUE:
                    return this.mForceDialogue;
            }

            JCS_GameErrors.JcsErrors("JCS_GameManager", -1, "Failed to get Dialogue -> " + type);
            return null;
        }
        public JCS_Vector<JCS_DialogueObject> GetGameDialogues() { return this.mGameDialogues; }
        public LinkedList<JCS_DialogueObject> GetOpenWindow() { return this.mOpenWindow; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;

            mGameDialogues = new JCS_Vector<JCS_DialogueObject>();
            mOpenWindow = new LinkedList<JCS_DialogueObject>();
        }
        private void Start()
        {
            if (JCS_GameSettings.instance == null)
                return;

            if (JCS_GameSettings.instance.THIS_IS_GAME_SCENE)
            {
                if (GetJCSDialogue(JCS_DialogueType.GAME_UI) == null)
                    JCS_ButtonFunctions.PopInGameUI();
            }
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            // Test.
            //DialogueTest();
#endif

            if (JCS_GameSettings.instance.THIS_IS_GAME_SCENE)
            {
                if (GetJCSDialogue(JCS_DialogueType.GAME_UI) == null)
                    JCS_ButtonFunctions.PopInGameUI();
            }

#if (UNITY_EDITOR || UNITY_STANDALONE)

            // Exit in game diagloue not in game UI!!
            // (Admin input)
            if (Input.GetKeyDown(KeyCode.Escape))
                JCS_ButtonFunctions.DestoryCurrentDialogue(JCS_DialogueType.GAME_DIALOGUE);
#endif
        }

        private void DialogueTest()
        {
            if (JCS_Input.GetKeyDown(KeyCode.A))
                JCS_ButtonFunctions.PopIsConnectDialogue();
            if (JCS_Input.GetKeyDown(KeyCode.S))
                JCS_ButtonFunctions.PopSettingDialogue();
            if (JCS_Input.GetKeyDown(KeyCode.D))
                JCS_ButtonFunctions.PopInGameUI();
            if (JCS_Input.GetKeyDown(KeyCode.F))
                JCS_ButtonFunctions.PopTalkDialogue();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public int AddGameDialogueAndGetIndex(JCS_DialogueObject stuff)
        {
            mGameDialogues.push(stuff);

            return mGameDialogues.length - 1;
        }
        public void HideTheLastOpenDialogue()
        {
            // return if nothing in the list
            if (GetOpenWindow().Count == 0)
                return;

            JCS_DialogueObject jdo = GetOpenWindow().Last.Value;

            // once it hide it will remove from the list it self!
            jdo.HideDialogue();
        }
        public void HideAllOpenDialogue()
        {
            // return if nothing in the list
            if (GetOpenWindow().Count == 0)
                return;

            while (GetOpenWindow().Count != 0)
            {
                GetOpenWindow().First.Value.HideDialogueWithoutSound();
            }

        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
