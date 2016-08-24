/**
 * $File: JCS_Button.cs $
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
    /// Buttton Interface (NGUI)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public abstract class JCS_Button 
        : MonoBehaviour
    {
        protected RectTransform mRectTransform = null;
        protected Button mButton = null;
        protected Image mImage = null;

        [Header("** Initialize Variables (JCS_Button) **")]
        [Tooltip("Auto add listner to button click event?")]
        [SerializeField] protected bool mAutoListener = true;
        [Tooltip("Index pairing with Dialogue, in order to call the correct index.")]
        [SerializeField] protected int mDialogueIndex = -1;

        [Header("** Runtime Variables (JCS_Button) **")]
        [Tooltip("Is the button interactable or not. (Default: true)")]
        [SerializeField]
        protected bool mInteractable = true;

        [Tooltip("Color tint when button is interactable.")]
        [SerializeField]
        protected Color mInteractColor = new Color(1, 1, 1, 1);

        [Tooltip("Color tint when button is not interactable.")]
        [SerializeField]
        protected Color mNotInteractColor = new Color(1, 1, 1, 0.5f);

        public delegate void CallBackFunc();
        // JCSUnity framework only callback, do not override this callback.
        private CallBackFunc mBtnClickSystemCallBack =null;
        // for user's callback.
        protected CallBackFunc mBtnClickCallBack = null;

        //========================================
        //      setter / getter
        //------------------------------
        public Image Image { get { return this.mImage; } }
        public RectTransform GetRectTransfom() { return this.mRectTransform; }
        public int DialogueIndex { get { return this.mDialogueIndex; } set { this.mDialogueIndex = value; } }
        public void SetCallback(CallBackFunc func) { this.mBtnClickCallBack = func; }
        public void SetSystemCallback(CallBackFunc func) { this.mBtnClickSystemCallBack = func; }
        public bool AutoListener { get { return this.mAutoListener; } set { this.mAutoListener = value; } }
        public bool Interactable {
            get { return this.mInteractable; }
            set
            {
                mInteractable = value;

                // set this, in order to get the effect immdediatly.
                SetInteractable();
            }
        }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mRectTransform = this.GetComponent<RectTransform>();
            mButton = this.GetComponent<Button>();
            mImage = this.GetComponent<Image>();
            
            if (mAutoListener)
            {
                // add listener itself, but it won't show in the inspector.
                mButton.onClick.AddListener(JCS_ButtonClick);
            }

            // set the stating interactable.
            SetInteractable();
        }

        //========================================
        //      Self-Define
        //------------------------------

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public virtual void JCS_ButtonClick()
        {
            if (mBtnClickSystemCallBack != null)
                mBtnClickSystemCallBack.Invoke();

            // do callback function
            if (mBtnClickCallBack != null)
                mBtnClickCallBack.Invoke();
        }
        
        /// <summary>
        /// Use this to enable and disable the button.
        /// </summary>
        /// <param name="act"></param>
        public virtual void SetInteractable(bool act)
        {
            mInteractable = act;
            mButton.enabled = mInteractable;

            if (mInteractable)
            {
                mImage.color = mInteractColor;
            }
            else
            {
                mImage.color = mNotInteractColor;
            }
        }
        public virtual void SetInteractable()
        {
            SetInteractable(mInteractable);
        }

    }
}
