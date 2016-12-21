/**
 * $File: JCS_GameWindowHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{

    /// <summary>
    /// Object to create the instance of Game Window.
    /// </summary>
    public class JCS_GameWindowHandler 
        : JCS_Settings<JCS_GameWindowHandler>
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // System UI
        [Header("** System Dialogue **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject[] mSystemUI = null;


        // Game UI
        [Header("** Game User Interface **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject mGameUI = null;


        // NPC Dialogue
        [Header("** NPC Dialogue **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject mNPCDialogue = null;


        // List of all the Game Window we are going to use in the game
        [Header("** Player Dialogue **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject[] mPlayerDialogue = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_DialogueObject GetPlayerDialogueAt(int index) { return this.mPlayerDialogue[index]; }
        public JCS_DialogueObject NPCDialogue { get { return this.mNPCDialogue; } set { this.mNPCDialogue = value; } }
        public JCS_DialogueObject[] PlayerDialogue { get { return this.mPlayerDialogue; } set { this.mPlayerDialogue = value; } }
        public JCS_DialogueObject GameUI { get { return this.mGameUI; } set { this.mGameUI = value; } }
        public JCS_DialogueObject[] SystemUI { get { return this.mSystemUI; } set { this.mSystemUI = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            PopSystemUI();
            PopGameUI();
            PopNPCDialogue();
            PopPlayerDialogue();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        public void ShowGameUI()
        {
            if (mGameUI == null)
            {
                JCS_Debug.JcsErrors(
                    this, "Game UI is not avialiable references...");
                return;
            }

            mGameUI.ShowDialogueWithoutSound();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideGameUI()
        {
            if (mGameUI == null)
            {
                JCS_Debug.JcsErrors(
                    this, "Game UI is not avialiable references...");
                return;
            }

            mGameUI.HideDialogueWithoutSound();
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_GameWindowHandler _old, JCS_GameWindowHandler _new)
        {
            _new.SystemUI = _old.SystemUI;
            _new.NPCDialogue = _old.NPCDialogue;
            _new.GameUI = _old.GameUI;
            _new.PlayerDialogue = _old.PlayerDialogue;
        }

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        private void PopNPCDialogue()
        {
            PopDialogue(mNPCDialogue);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGameUI()
        {
            PopDialogue(mGameUI);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopPlayerDialogue()
        {
            JCS_DialogueObject[] temp = (JCS_DialogueObject[])mPlayerDialogue.Clone();
            PopDialogue(temp);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopSystemUI()
        {
            JCS_DialogueObject[] temp = (JCS_DialogueObject[])mSystemUI.Clone();
            PopDialogue(temp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private JCS_DialogueObject PopDialogue(JCS_DialogueObject obj)
        {
            if (obj == null)
                return null;

            obj = (JCS_DialogueObject)JCS_Utility.SpawnGameObject(obj);
            obj.ShowDialogue();
            obj.SetKeyCode(KeyCode.None);

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private JCS_DialogueObject[] PopDialogue(JCS_DialogueObject[] list)
        {
            if (list.Length == 0)
                return null;

            for (int index = 0;
                index < list.Length;
                ++index)
            {
                if (list[index] == null)
                    continue;

                list[index] = (JCS_DialogueObject)JCS_Utility.SpawnGameObject(list[index]);
                list[index].HideDialogue();
            }

            return list;
        }

    }
}
