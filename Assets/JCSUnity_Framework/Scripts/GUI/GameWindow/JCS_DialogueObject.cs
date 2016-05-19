/**
 * $File: JCS_DialogueObject.cs $
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

    [RequireComponent(typeof(JCS_GameWindow))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DialogueObject 
        : JCS_PanelRoot
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        // unique index to open the dialogue
        [SerializeField] private int mDialogueIndex = -1;
        // Key to open this dialogue
        [SerializeField] private KeyCode mKeyCode = KeyCode.None;

        // this type for priority
        [SerializeField] private JCS_DialogueType mDialogueType = JCS_DialogueType.PLAYER_DIALOGUE;
        
        // type for positioning the panel every time it opens
        [SerializeField] private JCS_PanelType mPanelType = JCS_PanelType.RESET_PANEL;


        [SerializeField] private AudioClip mOpenWindowClip = null;
        [SerializeField] private AudioClip mCloseWindowClip = null;
        private JCS_SoundPlayer mJCSSoundPlayer = null;

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
        private void OnLevelWasLoaded()
        {
            if (mRectTransform == null)
                this.mRectTransform = this.GetComponent<RectTransform>();

            JCS_UIManager uim = JCS_UIManager.instance;

            // Once we load the scene we need to let new object 
            // in the scene know about us!
            uim.SetJCSDialogue(mDialogueType, this);

            // add to open window list if the window is open!
            AddToOpenWindowList();

            ResetDialogue();
        }
        protected override void Awake()
        {
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

        private void Update()
        {
            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            if (JCS_Input.GetKeyDown(GetKeyCode()))
            {
                ToggleVisibility();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        
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
        public override void HideDialogueWithoutSound()
        {
            base.HideDialogueWithoutSound();

            RemoveFromOpenWindowList();
        }
        public void HideDialogue()
        {
            HideDialogueWithoutSound();

            // Apply sound
            if (mCloseWindowClip != null && mJCSSoundPlayer != null)
                mJCSSoundPlayer.PlayOneShot(mCloseWindowClip, JCS_SoundType.SOUND_2D);
        }

        public void ToggleVisibility()
        {
            if (mIsVisible)
                HideDialogue();
            else
                ShowDialogue();

            ResetDialogue();
        }
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
        private void RemoveFromOpenWindowList()
        {
            JCS_UIManager.instance.GetOpenWindow().Remove(this);
        }
        private void SwapToTheLastOpenWindowList()
        {
            // TODO(JenChieh): optimize

            // remove the list
            RemoveFromOpenWindowList();

            // add back to the list (so it will be the last)
            AddToOpenWindowList();
        }

    }
}
