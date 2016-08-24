/**
 * $File: JCS_3DLiquidBarText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    /// <summary>
    /// Object GUI which will do like the counter show.
    /// </summary>
    [RequireComponent(typeof(JCS_3DLiquidBar))]
    public class JCS_3DLiquidBarText
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // Counter with the liquid bar?
        private JCS_3DLiquidBar mLiquidBar = null;

        [Header("** Runtime Variables (JCS_GUICounter) **")]

        [Tooltip("Text to do the effect.")]
        [SerializeField] private Text mCounterText = null;

        [Tooltip("")]
        [SerializeField]
        private Transform mCounterTextWorldTransform = null;

        [Tooltip("Text Render maxinum of the liquid bar value.")]
        [SerializeField] private Text mFullText = null;

        [Tooltip("")]
        [SerializeField]
        private Transform mFullTextWorldTransform = null;

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
            this.mLiquidBar = this.GetComponent<JCS_3DLiquidBar>();
        }

        private void LateUpdate()
        {
            FitCanvas();

            DoTextRender();
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

        /// <summary>
        /// Find the value and set the text to value.
        /// </summary>
        private void DoTextRender()
        {
            if (mCounterText != null)
                mCounterText.text = ((int)mLiquidBar.GetCurrentValue()).ToString();

            if (mFullText != null)
                mFullText.text = mLiquidBar.MaxValue.ToString();
        }

        /// <summary>
        /// Make the Canvas space text fit in world space.
        /// </summary>
        private void FitCanvas()
        {
            if (mCounterText != null &&
                mCounterTextWorldTransform != null
                )
            {
                mCounterText.rectTransform.anchoredPosition = JCS_Camera.main.WorldToCanvasSpace(this.mCounterTextWorldTransform.position);
            }

            if (mFullText != null &&
                mFullTextWorldTransform != null)
            {
                mFullText.rectTransform.anchoredPosition = JCS_Camera.main.WorldToCanvasSpace(this.mFullTextWorldTransform.position);
            }

        }

    }
}
