/**
 * $File: JCS_DialogueSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_DialogueSystem 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        
        [Header("** Check Variables (JCS_DialogueSystem) **")]

        [Tooltip("Script to run the current text box.")]
        [SerializeField]
        private JCS_DialogueScript mDialogueScript = null;

        [Tooltip("Message in the text box.")]
        [SerializeField]
        private string mMessage = "";

        [Tooltip("Skip to the end of the message.")]
        [SerializeField]
        private bool mSkip = false;

        [Tooltip("Trigger of checking the scrolling effect is running?")]
        [SerializeField]
        private bool mScrolling = false;

        [Tooltip("Check if the dialogue is active or not...")]
        [SerializeField]
        private bool mActive = false;

        public int Mode { get; set; }
        public int Type { get; set; }
        public int Selection { get; set; }



        [Header("** Runtime Variables (JCS_DialogueSystem) **")]

        [Tooltip("Main text GUI component to scroll.")]
        [SerializeField]
        private Text mTextBox = null;

        [Tooltip("Speed of scrolling the text.")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mScrollTime = 0.1f;

        // timer to calculate the scroll time
        private float mScrollTimer = 0;

        [Tooltip("Panel Rect Transfrom.")]
        [SerializeField]
        private RectTransform mPanelTrans = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mOkBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mExitBtn = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mYesBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mNoBtn = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mNextBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mPreviousBtn = null;

        [Tooltip("Quest use.")]
        [SerializeField]
        private JCS_Button mAcceptBtn = null;
        [Tooltip("Quest use.")]
        [SerializeField]
        private JCS_Button mDeclineBtn = null;

        // Text index to make sure 
        // each character in the textbox.
        private int mTextIndex = 0;

        // 解密用. 中間的數字改一下就是selection index!
        private string mSelectStringFront = "#L" + 0 + "##b";
        private string mSelectStringBack = "#k#l";


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_DialogueScript DialogueScript { get { return this.mDialogueScript; } set { this.mDialogueScript = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // try to get transfrom by 
            // it own current transfrom.
            if (mPanelTrans == null)
                mPanelTrans = this.GetComponent<RectTransform>();

            // set to manager to get manage.
            JCS_UtilitiesManager.instance.SetDiaglogueSystem(this);

            // check if text box null references...
            if (mTextBox == null)
            {
                JCS_GameErrors.JcsWarnings(this,
                    "You have the dialogue system in the scene, but u did not assign a text box... Try to delete it?");
            }

            InitBtnsSet();
        }
        
        private void LateUpdate()
        {
            // scrolling text effect.
            ScrollText();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        public void ActiveDialogue(JCS_DialogueScript script)
        {
            mDialogueScript = script;

            if (mActive)
            {
                JCS_GameErrors.JcsErrors(
                    this, 
                    "Dialogue System is already active... Failed to active another one.");

                return;
            }

            // check if the script attached is available?
            if (DialogueScript == null)
                return;


            // reset the action, so it will always start
            // from the beginning.
            mDialogueScript.ResetAction();

            // active panel
            PanelActive(true);

            // otherwise active the dialogue
            mActive = true;

            // run the first action.
            RunAction();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendNext(string msg)
        {
            NextBtnActive(true);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendNextPrev(string msg)
        {
            NextBtnActive(true);
            PrevBtnActive(true);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendOk(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(true);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendYesNo(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(true);
            NoBtnActive(true);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendSimple(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendAcceptDecline(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            AcceptBtnActive(true);
            DeclineBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Call this to end the dialogue status.
        /// </summary>
        public void Dispose()
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            OkBtnActive(false);

            // disable the exit button!
            ExitBtnActive(false);


            // dis-attach the script.
            mDialogueScript = null;

            ResetStats();

            // de-active the panel transform.
            PanelActive(false);

            // de-active dialogue system.
            mActive = false;
        }

        /// <summary>
        /// Reset all dialogue system.
        /// </summary>
        public void ResetStats()
        {
            mScrolling = false;
            mSkip = false;
            mTextIndex = 0;

            // reset message.
            mMessage = "";
            mTextBox.text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void WorldMessage(string msg)
        {
            // do broadcast.
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do scroll text action.
        /// </summary>
        private void ScrollText()
        {
            // check if the effect is on
            if (!mScrolling)
                return;

            // do timer.
            mScrollTimer += Time.deltaTime;

            // check if each index of character in message 
            // is good to display in the text box render queue.
            if (mScrollTimer < mScrollTime)
                return;

            // reset timer
            mScrollTimer = 0;

            if (mMessage == mTextBox.text)
            {
                // make sure no errors
                mScrolling = false;
                mSkip = false;

                // set directly to the text box.
                mTextBox.text = mMessage;

                // reset text index counter
                mTextIndex = 0;
                return;
            }

            if (mSkip)
            {
                // set directly to the text box.
                mTextBox.text = mMessage;

                mSkip = false;

                // end effect.
                mScrolling = false;
            }
            else
            {
                // do the scrolling
                mTextBox.text = mMessage.Substring(0, mTextIndex);
            }

            // increament the index
            ++mTextIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void PanelActive(bool act)
        {
            if (mPanelTrans == null)
                return;

            mPanelTrans.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void NextBtnActive(bool act)
        {
            if (mNextBtn == null)
                return;

            mNextBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void PrevBtnActive(bool act)
        {
            if (mPreviousBtn == null)
                return;

            mPreviousBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void NoBtnActive(bool act)
        {
            if (mNoBtn == null)
                return;

            mNoBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void YesBtnActive(bool act)
        {
            if (mYesBtn == null)
                return;

            mYesBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void OkBtnActive(bool act)
        {
            if (mOkBtn == null)
                return;

            mOkBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void ExitBtnActive(bool act)
        {
            if (mExitBtn == null)
                return;

            mExitBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void AcceptBtnActive(bool act)
        {
            if (mAcceptBtn == null)
                return;

            mAcceptBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void DeclineBtnActive(bool act)
        {
            if (mDeclineBtn == null)
                return;

            mDeclineBtn.gameObject.SetActive(act);
        }

        /// <summary>
        /// Initalize all the button.
        /// </summary>
        private void InitBtnsSet()
        {
            if (mOkBtn != null)
            {
                mOkBtn.SetSystemCallback(OkBtnCallback);
            }

            if (mNoBtn != null)
            {
                mNoBtn.SetSystemCallback(NoBtnCallback);
            }

            if (mYesBtn != null)
            {
                mYesBtn.SetSystemCallback(YesBtnCallback);
            }

            if (mNextBtn != null)
            {
                mNextBtn.SetSystemCallback(NextBtnCallback);
            }

            if (mPreviousBtn != null)
            {
                mPreviousBtn.SetSystemCallback(PreviousBtnCallback);
            }

            if (mExitBtn != null)
            {
                mExitBtn.SetSystemCallback(ExitBtnCallback);
            }

            if (mAcceptBtn != null)
            {
                mAcceptBtn.SetSystemCallback(AcceptBtnCallback);
            }

            if (mDeclineBtn != null)
            {
                mDeclineBtn.SetSystemCallback(DeclineBtnCallback);
            }
        }

        /// <summary>
        /// Run the script once.
        /// </summary>
        private void RunAction()
        {
            if (!mActive)
                return;

            mDialogueScript.Action(Mode, Type, Selection);
        }

        /// <summary>
        /// What if "Next Button" clicked?
        /// </summary>
        private void NextBtnCallback()
        {
            ResetStats();

            ++mDialogueScript.Status;

            RunAction();
        }
        /// <summary>
        /// What if "Previous Button" clicked?
        /// </summary>
        private void PreviousBtnCallback()
        {
            ResetStats();

            --mDialogueScript.Status;

            RunAction();
        }
        /// <summary>
        /// What if "Yes Button" clicked?
        /// </summary>
        private void YesBtnCallback()
        {

            RunAction();
        }
        /// <summary>
        /// What if "No Button" clicked?
        /// </summary>
        private void NoBtnCallback()
        {

            RunAction();
        }
        /// <summary>
        /// What if "Ok Button" clicked?
        /// </summary>
        private void OkBtnCallback()
        {
            // when exit button happens, 
            // exit the dialogue box.
            Dispose();
        }
        /// <summary>
        /// What if "Exit Button" clicked?
        /// </summary>
        private void ExitBtnCallback()
        {
            // when exit button happens, 
            // exit the dialogue box.
            Dispose();
        }
        /// <summary>
        /// What if "Accept Button" clicked?
        /// </summary>
        private void AcceptBtnCallback()
        {

            RunAction();
        }
        /// <summary>
        /// What if "Decline Button" clicked?
        /// </summary>
        private void DeclineBtnCallback()
        {

            RunAction();
        }

    }
}
