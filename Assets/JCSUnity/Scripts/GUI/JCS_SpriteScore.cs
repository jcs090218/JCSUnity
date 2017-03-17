/**
 * $File: JCS_SpriteScore.cs $
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
    public class JCS_SpriteScore
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_SpriteScore) **")]

        [Tooltip(@"Current score rendering..., do not use this to 
check value. Because this will always be animate.")]
        [SerializeField]
        private int mCurrentScore = 0;

        [Tooltip("Score text 0")] [SerializeField]
        private Sprite mScoreText0 = null;
        [Tooltip("Score text 1")] [SerializeField]
        private Sprite mScoreText1 = null;
        [Tooltip("Score text 2")] [SerializeField]
        private Sprite mScoreText2 = null;
        [Tooltip("Score text 3")] [SerializeField]
        private Sprite mScoreText3 = null;
        [Tooltip("Score text 4")] [SerializeField]
        private Sprite mScoreText4 = null;
        [Tooltip("Score text 5")] [SerializeField]
        private Sprite mScoreText5 = null;
        [Tooltip("Score text 6")] [SerializeField]
        private Sprite mScoreText6 = null;
        [Tooltip("Score text 7")] [SerializeField]
        private Sprite mScoreText7 = null;
        [Tooltip("Score text 8")] [SerializeField]
        private Sprite mScoreText8 = null;
        [Tooltip("Score text 9")] [SerializeField]
        private Sprite mScoreText9 = null;

        [Tooltip("Each digit, the more length the more digit.")]
        [SerializeField]
        private SpriteRenderer[] mDigitsRendererSlot = null;

        [Tooltip("Interval between each digit.")]
        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float mDigitInterval = 0.5f;


        [Header("** Runtime Variables (JCS_SpriteScore) **")]

        [Tooltip(@"This will make the score have the transition 
between, setting to a new score. If you want the score set directly, you 
should disable this effect for best purpose.")]
        [SerializeField]
        private bool mDeltaToCurrentScore = false;

        [Tooltip("Toward to this score, if mDeltaToCurrentScore active.")]
        [SerializeField]
        private int mTargetScore = 0;

        [Tooltip("How fast the score animate.")]
        [SerializeField] [Range(0.01f, 1.0f)]
        private float mAnimScoreTime = 0;

        // timer for animation the score
        private float mAnimScoreTimer = 0;


        [Header("- Min/Max Settings (JCS_SpriteScore)")]

        [Tooltip("Maxinum score")]
        [SerializeField]
        private int mMaxScore = 999;

        [Tooltip("Mininum score")]
        [SerializeField]
        private int mMinScore = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
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
                    JCS_Debug.JcsErrors(
                        this, "Digit slot cannot be null references...");
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
            for (int digit = 0;
                digit < mDigitsRendererSlot.Length;
                ++digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.JcsErrors(
                        this, "Digit slot cannot be null references...");
                    continue;
                }

                int valDigit = JCS_Mathf.GetSingleDigit(digit + 1, score);

                mDigitsRendererSlot[digit].sprite = GetSingleDigitSprite(valDigit);
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
                ++mCurrentScore;
            else
                --mCurrentScore;

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
