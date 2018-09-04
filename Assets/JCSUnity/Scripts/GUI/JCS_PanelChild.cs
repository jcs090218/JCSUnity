/**
 * $File: JCS_PanelChild.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// This do the same thing as JCS_PanelRoot, but instead of all
    /// the child have to check JCS_PanelRoot is vague.The solution
    /// from this, we decide to have another component by name it
    /// differently and loop through the component and check if the
    /// panel has the correct proportion and scaling. Notice this
    /// class already been set by other same component, this will not
    /// be active.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_PanelChild
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        [Header("** Check Variables (JCS_PanelChild) **")]

        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        // IMPORTANT(jenchieh): the Text component is so special in 
        // Unity Enigne that we need to treat this specifically in order 
        // to get the correct apsect screen looking.
        [SerializeField]
        private Text mText = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_PanelRoot PanelRoot { get { return this.mPanelRoot; } set { this.mPanelRoot = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            if (mPanelRoot == null)
                mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();

            // Rely on "Script Execution Order"
            {
                // get all the same class object on this game object.
                JCS_PanelChild[] tempPanelChild = null;
                tempPanelChild = this.GetComponents<JCS_PanelChild>();

                // only do it once.
                if (/* Check 'jpr' null for spawn GUI objects. */
                    mPanelRoot != null && 
                    /* Regular checks. */
                    tempPanelChild.Length == 1 &&
                    tempPanelChild[0] == this)
                {
                    FitPerfectSize(
                        mPanelRoot.PanelDeltaWidthRatio,
                        mPanelRoot.PanelDeltaHeightRatio);

                    // Try to fix the text's font size issue.
                    FixTextFontSize(
                        mPanelRoot.PanelDeltaWidthRatio,
                        mPanelRoot.PanelDeltaHeightRatio);
                }

                // since we add this script assuming we are 
                // int the fit perfect size mode
                // see "JCS_PanelRoot" -> mFitScreenSize variables
                AddPanelChild();
            }
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Fit screen size base on Unity Engine architecture.
        /// </summary>
        /// <param name="xRatio"></param>
        /// <param name="yRatio"></param>
        public void FitPerfectSize(float xRatio, float yRatio)
        {
            Vector3 newPosition = mRectTransform.localPosition;
            newPosition.x = newPosition.x / xRatio;
            newPosition.y = newPosition.y / yRatio;

            float guiWidth = mRectTransform.sizeDelta.x / xRatio;
            float guiHeight = mRectTransform.sizeDelta.y / yRatio;

            /*
             * NOTE(jenchieh): 
             * Cool, `sizeDelta' will actually change the `localPosition'
             * now since version 2017.4.
             * 
             * So we set the `sizeDelta' (width and height) first, then
             * set the `localPosition'.
             */
            {
                mRectTransform.sizeDelta = new Vector2(guiWidth, guiHeight);

                // set to the new position
                mRectTransform.localPosition = newPosition;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Add all child with same effect.
        /// </summary>
        private void AddPanelChild()
        {
            Transform tempTrans = this.transform;

            for (int index = 0;
                index < transform.childCount;
                ++index)
            {
                Transform child = tempTrans.GetChild(index);

                // Only added once.
                if (child.GetComponent<JCS_PanelChild>() != null)
                    continue;

                JCS_PanelChild panelChild = child.gameObject.AddComponent<JCS_PanelChild>();
                panelChild.PanelRoot = mPanelRoot;
            }
        }

        /// <summary>
        /// Try to fix the text's font size.
        /// </summary>
        private void FixTextFontSize(float xRatio, float yRatio)
        {
            this.mText = this.GetComponent<Text>();

            if (mText == null)
                return;

            /* Fix the font size. */
            if (mPanelRoot.FixTextByFontSize)
            {
                float smallerRatio = Mathf.Min(xRatio, yRatio);

                mText.fontSize = (int)(mText.fontSize / smallerRatio);
            }

            /* Fix the scale. */
            if (mPanelRoot.FixTextByScale)
            {
                Vector3 newScale = mText.transform.localScale;
                newScale.x = newScale.x / xRatio;
                newScale.y = newScale.y / yRatio;
                mText.transform.localScale = newScale;
            }
        }
    }
}
