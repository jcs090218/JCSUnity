/**
 * $File: JCS_LogText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    /// <summary>
    /// Log text for JCS_IGLogSystem to handle.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    [RequireComponent(typeof(JCS_FadeObject))]
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    [RequireComponent(typeof(JCS_SlideEffect))]
    public class JCS_LogText
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private Text mText = null;

        private JCS_FadeObject mFadeObject = null;
        private JCS_SimpleTrackAction mSimpleTrackAction = null;
        private JCS_SlideEffect mSlideEffect = null;

        [Header("** Check Variables (JCS_LogText) **")]

        [SerializeField]
        private JCS_IGLogSystem mIGLogSystem = null;

        [SerializeField]
        private bool mActive = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Text unityText { get { return this.mText; } }
        public bool isActive() { return this.mActive; }

        public JCS_SimpleTrackAction SimpleTrackAction { get { return this.mSimpleTrackAction; } }
        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }

        public void SetIGLogSystem(JCS_IGLogSystem sys)
        {
            this.mIGLogSystem = sys;
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mText = this.GetComponent<Text>();

            mFadeObject = this.GetComponent<JCS_FadeObject>();
            mSimpleTrackAction = this.GetComponent<JCS_SimpleTrackAction>();
            mSlideEffect = this.GetComponent<JCS_SlideEffect>();

            // set the fade out call back, 
            // so we active from pool, 
            // and check to see if the object is fade out complete.
            // if is complete set the active to false (return 
            // to pool).
            mFadeObject.fadeOutCallback = FadeOutCompleteCallback;
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// when activate from pool.
        /// </summary>
        public void Execute(string message)
        {
            if (mActive)
            {
                JCS_Debug.LogError( 
                    "Call this while the object is still active.");

                return;
            }

            // set the message text
            SetText(message);

            // fade out
            mFadeObject.FadeOut();

            // slide out
            mSlideEffect.Active();

            mActive = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SetText(string message)
        {
            mText.text = message;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Function assgin to FadeObject in order to let
        /// the object know when the object to deactive, 
        /// and prepare for next use.
        /// </summary>
        private void FadeOutCompleteCallback()
        {
            mActive = false;

            // remove from render queue
            mIGLogSystem.RemoveFromRenderVec(this);

            mSlideEffect.Deactive();
        }

    }
}
