/**
 * $File: JCS_DeltaNumber.cs $
 * $Date: 2017-03-10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Score system with the sprite 0 to 9.
    /// </summary>
    public class JCS_DeltaNumber
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_DeltaNumber) **")]

        [Tooltip(@"Current score rendering..., do not use this to 
check value. Because this will always be animate.")]
        [SerializeField]
        private int mCurrentScore = 0;

        [Tooltip("Default null slot. [Default: transparent_256.png]")]
        [SerializeField]
        private Sprite mScoreNull = null;
        [Tooltip("Score text 0")]
        [SerializeField]
        private Sprite mScoreText0 = null;
        [Tooltip("Score text 1")]
        [SerializeField]
        private Sprite mScoreText1 = null;
        [Tooltip("Score text 2")]
        [SerializeField]
        private Sprite mScoreText2 = null;
        [Tooltip("Score text 3")]
        [SerializeField]
        private Sprite mScoreText3 = null;
        [Tooltip("Score text 4")]
        [SerializeField]
        private Sprite mScoreText4 = null;
        [Tooltip("Score text 5")]
        [SerializeField]
        private Sprite mScoreText5 = null;
        [Tooltip("Score text 6")]
        [SerializeField]
        private Sprite mScoreText6 = null;
        [Tooltip("Score text 7")]
        [SerializeField]
        private Sprite mScoreText7 = null;
        [Tooltip("Score text 8")]
        [SerializeField]
        private Sprite mScoreText8 = null;
        [Tooltip("Score text 9")]
        [SerializeField]
        private Sprite mScoreText9 = null;

        [Tooltip("Each digit, the more length the more digit.")]
        [SerializeField]
        private JCS_UnityObject[] mDigitsRendererSlot = null;

        [Tooltip("Interval between each digit.")]
        [SerializeField]
        [Range(0.1f, 4000.0f)]
        private float mDigitInterval = 0.5f;


        [Header("** Runtime Variables (JCS_DeltaNumber) **")]

        [Tooltip("Clear all the empty zero from the left.")]
        [SerializeField]
        private bool mClearEmptyLeftZero = false;


        [Header("- Min/Max Settings (JCS_DeltaNumber)")]

        [Tooltip("Maxinum score")]
        [SerializeField]
        private int mMaxScore = 999;

        [Tooltip("Mininum score")]
        [SerializeField]
        private int mMinScore = 0;


        [Header("- Delta Animation Settings (JCS_DeltaNumber)")]

        [Tooltip(@"This will make the score have the transition 
between, setting to a new score. If you want the score set directly, you 
should disable this effect for best purpose.")]
        [SerializeField]
        private bool mDeltaToCurrentScore = false;

        [Tooltip("Toward to this score, if mDeltaToCurrentScore active.")]
        [SerializeField]
        private int mTargetScore = 0;

        [Tooltip("How fast the score animate.")]
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float mAnimScoreTime = 0;

        // timer for animation the score
        private float mAnimScoreTimer = 0;

        [Tooltip("How much the delta value add up.")]
        [SerializeField] [Range(1, 1000)]
        private int mDeltaProduct = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool ClearEmptyLeftZero { get { return this.mClearEmptyLeftZero; } set { this.mClearEmptyLeftZero = value; } }
        public bool DeltaToCurrentScore { get { return this.mDeltaToCurrentScore; } set { this.mDeltaToCurrentScore = value; } }
        public int TargetScore
        {
            get { return this.mTargetScore; }
            set
            {
                this.mTargetScore = value;

                // by setting the delta score will enable the 
                // delta to current score effect.
                this.mDeltaToCurrentScore = true;
            }
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // update all the unity object.
            foreach (JCS_UnityObject digit in mDigitsRendererSlot)
            {
                if (digit == null)
                    continue;

                digit.UpdateUnityData();
            }

            // update interval
            UpdateIntervalForEachDigit();

            // init score to current.
            UpdateScore();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoDeltaCurrentScore();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKey(KeyCode.N))
                UpdateScore(20);
            if (JCS_Input.GetKey(KeyCode.M))
                UpdateScore(40);

            if (JCS_Input.GetKey(KeyCode.B))
                UpdateScore(0);
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Update the score GUI
        /// </summary>
        public void UpdateScore()
        {
            UpdateScore(mCurrentScore);
        }

        /// <summary>
        /// Update the score GUI
        /// </summary>
        /// <param name="score"> score apply to show </param>
        public void UpdateScore(int score, bool anime = false)
        {
            if (mDeltaToCurrentScore)
            {
                if (!anime)
                    mTargetScore = score;
            }
            else
            {
                // apply to current score
                mCurrentScore = score;
            }

            int targetScore = score;

            if (targetScore < mMinScore)
                targetScore = mMinScore;
            else if (targetScore > mMaxScore)
                targetScore = mMaxScore;

            DoScoreGUI(targetScore);
        }

        /// <summary>
        /// Update the space to each digit.
        /// </summary>
        /// <param name="interval"> target interval </param>
        public void UpdateIntervalForEachDigit(float interval)
        {
            // update interval
            this.mDigitInterval = interval;

            for (int digit = 0;
                digit < mDigitsRendererSlot.Length;
                ++digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
                    continue;
                }

                Vector3 newPos = mDigitsRendererSlot[digit].transform.localPosition;
                newPos.x += -(interval * digit);
                mDigitsRendererSlot[digit].transform.localPosition = newPos;
            }
        }

        /// <summary>
        /// Update the space to each digit.
        /// </summary>
        public void UpdateIntervalForEachDigit()
        {
            UpdateIntervalForEachDigit(mDigitInterval);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do the GUI score logic.
        /// </summary>
        /// <param name="score"> score to apply. </param>
        private void DoScoreGUI(int score)
        {
            // check the first non zero from the left.
            bool meetNonZero = false;

            for (int digit = mDigitsRendererSlot.Length - 1;
                digit >= 0;
                --digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
                    continue;
                }

                int valDigit = JCS_Mathf.GetSingleDigit(digit + 1, score);

                mDigitsRendererSlot[digit].LocalSprite = GetSingleDigitSprite(valDigit);

                if (mClearEmptyLeftZero)
                {

                    /**
                     * Last digit is zero, we set to zero. so skip it.
                     */
                    if (valDigit != -1)
                        meetNonZero = true;

                    if (!meetNonZero)
                        mDigitsRendererSlot[digit].LocalSprite = mScoreNull;
                }
            }
        }

        /// <summary>
        /// Main algorithm to approach to targe score.
        /// </summary>
        private void DoDeltaCurrentScore()
        {
            // check if the effect active.
            if (!mDeltaToCurrentScore)
                return;

            // if score the same, return.
            if (mCurrentScore == mTargetScore)
                return;

            mAnimScoreTimer += Time.deltaTime;

            // check if timer reach the time.
            if (mAnimScoreTimer < mAnimScoreTime)
                return;

            // find out to increase/decrease the score.
            if (mCurrentScore < mTargetScore)
                mCurrentScore += mDeltaProduct;
            else
                mCurrentScore -= mDeltaProduct;

            UpdateScore(mCurrentScore, true);

            // reset timer
            mAnimScoreTimer = 0;
        }

        /// <summary>
        /// Return a digit sprite.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private Sprite GetSingleDigitSprite(int num)
        {
            switch (num)
            {
                case 0: return mScoreText0;
                case 1: return mScoreText1;
                case 2: return mScoreText2;
                case 3: return mScoreText3;
                case 4: return mScoreText4;
                case 5: return mScoreText5;
                case 6: return mScoreText6;
                case 7: return mScoreText7;
                case 8: return mScoreText8;
                case 9: return mScoreText9;
            }

            // default return 'zero' sprite.
            return mScoreText0;
        }
    }
}
