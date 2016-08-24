/**
 * $File: JCS_LiquidBarText.cs $
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
    [RequireComponent(typeof(JCS_GUILiquidBar))]
    public class JCS_LiquidBarText
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // Counter with the liquid bar?
        private JCS_GUILiquidBar mLiquidBar = null;

        [Header("** Runtime Variables (JCS_GUICounter) **")]

        [Tooltip("Text to do the effect.")]
        [SerializeField] private Text mCounterText = null;

        [Tooltip("Text Render maxinum of the liquid bar value.")]
        [SerializeField] private Text mFullText = null;


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
            this.mLiquidBar = this.GetComponent<JCS_GUILiquidBar>();
        }

        private void LateUpdate()
        {
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
            if (mCounterText == null)
            {
                JCS_GameErrors.JcsReminders(
                    this, "No counter text can be render...");

                return;
            }

            mCounterText.text = ((int)mLiquidBar.GetCurrentValue()).ToString();

            if (mFullText == null)
            {
                JCS_GameErrors.JcsReminders(
                    this, "No full text can be render...");

                return;
            }

            mFullText.text = mLiquidBar.MaxValue.ToString();
        }

    }
}
