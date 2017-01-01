/**
 * $File: JCS_DialogueObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Base class of Game Window.
    /// </summary>
    [RequireComponent(typeof(JCS_GameWindow))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DialogueObject 
        : JCS_PanelRoot
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_SoundPlayer mJCSSoundPlayer = null;

        [Header("** Runtime Variables (JCS_DialogueObject) **")]

        [Tooltip("unique index to open the dialogue")]
        [SerializeField]
        private int mDialogueIndex = -1;

        [Tooltip("Key to open this dialogue")]
        [SerializeField]
        private KeyCode mKeyCode = KeyCode.None;

        [Tooltip("this type for priority")]
        [SerializeField]
        private JCS_DialogueType mDialogueType = JCS_DialogueType.PLAYER_DIALOGUE;

        [Tooltip("type for positioning the panel every time it opens")]
        [SerializeField]
        private JCS_PanelType mPanelType = JCS_PanelType.RESET_PANEL;


        [Header("- Sound Setting (JCS_DialogueObject) ")]

        [Tooltip("Sound when open this dialouge window.")]
        [SerializeField]
        private AudioClip mOpenWindowClip = null;

        [Tooltip("Sound when close this dialouge window.")]
        [SerializeField]
        private AudioClip mCloseWindowClip = null;
        

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int GetDialogueIndex() { return this.mDialogueIndex; }
        public KeyCode GetKeyCode() { return this.mKeyCode; }
        public void SetKeyCode(KeyCode key) { this.mKeyCode = key; }
        public JCS_DialogueType GetDialogueType() { return this.mDialogueType; }
        public JCS_PanelType GetPanelType() { return this.mPanelType; }
        public bool IsOpenWindow() { return this.mIsVisible; }

        //========================================
        //      Unity's function
        //------------------------------
#if !(UNITY_5_4_OR_NEWER)
        // Called by Unity after a new level was loaded.
        private void OnLevelWasLoaded()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            JCS_UIManager uim = JCS_UIManager.instance;

            // Once we load the scene we need to let new object 
            // in the scene know about us!
            uim.SetJCSDialogue(mDialogueType, this);

            // add to open window list if the window is open!
            AddToOpenWindowList();

            ResetDialogue();
        }
#endif

        protected override void Awake()
        {
#if (UNITY_5_4_OR_NEWER)
            this.mRectTransform = this.GetComponent<RectTransform>();
            if (mRectTransform == null)
                return;

            // this is the new way of doing the "OnLevelWasLoaded" 
            // function call after version 5.4
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                JCS_UIManager uim = JCS_UIManager.instance;

                // Once we load the scene we need to let new object 
                // in the scene know about us!
                uim.SetJCSDialogue(mDialogueType, this);

                // add to open window list if the window is open!
                AddToOpenWindowList();

                ResetDialogue();
            };
#endif

            this.mJCSSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            base.Awake();

            JCS_SoundSettings ss = JCS_SoundSettings.instance;


            // Assign Default Audio
            {
                if (mOpenWindowClip == null)
                    this.mOpenWindowClip = ss.DEFAULT_OPEN_WINDOW_CLIP;

                if (mCloseWindowClip == null)
                    this.mCloseWindowClip = ss.DEFAULT_CLOSE_WINDOW_CLIP;
            }

        }

        protected override void Start()
        {
            JCS_UIManager uim = JCS_UIManager.instance;

            uim.SetJCSDialogue(mDialogueType, this);

            base.Start();

            // pause the app
            JCS_ApplicationManager.APP_PAUSE = true;
        }

        protected void Update()
        {
            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            ProcessInput();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        
        /// <summary>
        /// 
        /// </summary>
        public void DoPanelType()
        {
            switch (mPanelType)
            {
                case JCS_PanelType.RESET_PANEL:
                    {
                        DestroyDialogue();
                    } break;
                case JCS_PanelType.RECORD_PANEL:
                    {
                        HideDialogue();
                    } break;
                case JCS_PanelType.RECORD_PANEL_TO_DATABASE:
                    {
                        // send packet
                    } break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowDialogue()
        {
            ShowDialogueWithoutSound();

            // set focus dialogue
            if (GetDialogueType() == JCS_DialogueType.PLAYER_DIALOGUE)
                JCS_UIManager.instance.SetJCSDialogue(JCS_DialogueType.PLAYER_DIALOGUE, this);

            // let UIManager know the window is opened
            SwapToTheLastOpenWindowList();

            if (mOpenWindowClip != null && mJCSSoundPlayer != null)
                mJCSSoundPlayer.PlayOneShot(mOpenWindowClip, JCS_SoundType.SOUND_2D);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void HideDialogueWithoutSound()
        {
            base.HideDialogueWithoutSound();

            RemoveFromOpenWindowList();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideDialogue()
        {
            HideDialogueWithoutSound();

            // Apply sound
            if (mCloseWindowClip != null && mJCSSoundPlayer != null)
                mJCSSoundPlayer.PlayOneShot(mCloseWindowClip, JCS_SoundType.SOUND_2D);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ToggleVisibility()
        {
            if (mIsVisible)
                HideDialogue();
            else
                ShowDialogue();

            ResetDialogue();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void MoveToTheLastChild()
        {
            base.MoveToTheLastChild();

            // once it move to the last child, (meaning the window have been focus)
            // 
            SwapToTheLastOpenWindowList();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        private void AddToOpenWindowList()
        {
            switch (mDialogueType)
            {
                case JCS_DialogueType.GAME_UI:
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    return;
            }

            // first check the window is open or not open
            if (!IsOpenWindow())
                return;

            // add to the list so the manager know what window is opened
            JCS_UIManager.instance.GetOpenWindow().AddLast(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveFromOpenWindowList()
        {
            JCS_UIManager.instance.GetOpenWindow().Remove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SwapToTheLastOpenWindowList()
        {
            // TODO(JenChieh): optimize

            // remove the list
            RemoveFromOpenWindowList();

            // add back to the list (so it will be the last)
            AddToOpenWindowList();
        }

        /// <summary>
        /// Process the input base on platform
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetKeyDown(GetKeyCode()))
            {
                ToggleVisibility();
            }
        }

    }
}
