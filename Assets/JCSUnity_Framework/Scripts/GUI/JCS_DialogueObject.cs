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

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(JCS_GameWindow))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DialogueObject : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        // unique index to open the dialogue
        [SerializeField] private int mDialogueIndex = -1;
        // Key to open this dialogue
        [SerializeField] private KeyCode mKeyCode = KeyCode.None;

        // this type for priority
        [SerializeField] private JCS_DialogueType mDialogueType = JCS_DialogueType.GAME_DIALOGUE;
        [SerializeField] private bool mAsOriginalSize = false;
        [SerializeField] private bool mAsOriginalPosition = false;
        [SerializeField] private bool mAsOriginalRotation = false;

        // type for positioning the panel every time it opens
        [SerializeField] private JCS_PanelType mPanelType = JCS_PanelType.RESET_PANEL;

        private Vector3 mOriginalScale = Vector3.zero;
        private Vector3 mOriginalPosition = Vector3.zero;
        private Quaternion mOriginalRotation = new Quaternion(0, 0, 0, 0);

        private bool mIsVisible = false;


        [SerializeField] private AudioClip mOpenWindowClip = null;
        [SerializeField] private AudioClip mCloseWindowClip = null;
        private JCS_SoundPlayer mJCSSoundPlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
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
            JCS_UIManager uim = JCS_UIManager.instance;

            // Once we load the scene we need to let new object 
            // in the scene know about us!
            uim.SetJCSDialogue(mDialogueType, this);

            // add to open window list if the window is open!
            AddToOpenWindowList();

            // only game diagloue can do this
            if (mDialogueType == JCS_DialogueType.GAME_DIALOGUE)
                mDialogueIndex = uim.AddGameDialogueAndGetIndex(this);

            ResetDialogue();
        }
        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();
            this.mJCSSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            JCS_SoundSettings ss = JCS_SoundSettings.instance;


            // Audio
            {
                if (mOpenWindowClip == null)
                    this.mOpenWindowClip = ss.DEFAULT_OPEN_WINDOW_CLIP;

                if (mCloseWindowClip == null)
                    this.mCloseWindowClip = ss.DEFAULT_CLOSE_WINDOW_CLIP;
            }

        }

        private void Start()
        {
            JCS_UIManager uim = JCS_UIManager.instance;

            uim.SetJCSDialogue(mDialogueType, this);

            // only game diagloue can do this
            if (mDialogueType == JCS_DialogueType.GAME_DIALOGUE)
                mDialogueIndex = uim.AddGameDialogueAndGetIndex(this);


            // Find the correct parent depend on the mode
            // developer choose and do the command
            {
                Transform parentObject = null;

                // if is Resize UI is enable than add Dialogue under
                // resize ui transform
                if (JCS_GameSettings.instance.RESIZE_UI)
                    parentObject = uim.GetJCSCanvas().GetResizeUI().transform;
                // Else we add it directly under the Canvas
                else
                    parentObject = uim.GetJCSCanvas().GetCanvas().transform;

                // set it to parent
                this.gameObject.transform.SetParent(parentObject);
            }

            ResetDialogue();

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
        public void ResetDialogue()
        {
            // IMPORTANT(JenChieh): override the resize UI part
            if (mAsOriginalSize)
                mRectTransform.localScale = mOriginalScale;
            else
                mRectTransform.localScale = Vector3.one;


            if (mAsOriginalPosition)
                mRectTransform.localPosition = mOriginalPosition;
            else
                mRectTransform.localPosition = Vector3.zero;


            if (mAsOriginalRotation)
                mRectTransform.localRotation = mOriginalRotation;
            else
                mRectTransform.localRotation = new Quaternion(0, 0, 0, 0);

        }
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
        public void DestroyDialogue()
        {
            // start the app
            JCS_ApplicationManager.APP_PAUSE = false;

            JCS_UIManager.instance.GetGameDialogues().slice(mDialogueIndex);

            // destroy this dialogue
            Destroy(this.gameObject);
        }
        public void ShowDialogue()
        {
            mIsVisible = true;

            //this.gameObject.SetActive(true);

            // active all the child object
            for (int index = 0; 
                index < this.transform.childCount;
                ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(true);
            }

            MoveToTheLastChild();

            if (GetDialogueType() == JCS_DialogueType.GAME_DIALOGUE)
                JCS_UIManager.instance.SetJCSDialogue(JCS_DialogueType.GAME_DIALOGUE, this);

            // let UIManager know the window is opened
            SwapToTheLastOpenWindowList();

            if (mOpenWindowClip != null)
                mJCSSoundPlayer.PlayOneShot(mOpenWindowClip, JCS_SoundType.SOUND_2D);
        }
        public void HideDialogueWithoutSound()
        {
            mIsVisible = false;

            // Instead of disable the object it self,
            // we deactive all the child object
            //this.gameObject.SetActive(false);


            // deactive all the child object
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(false);
            }

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
        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            this.transform.SetParent(null);     // not sure we need this?
            this.transform.SetParent(parent);

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
