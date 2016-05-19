/**
 * $File: JCS_DamageText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    public class JCS_DamageText
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private enum TextEffect
        {
            MOVE_UP,    // move up a bit and fade out
            FADE,       // stay where they are and do fade out
            SLIDE_OUT   // slide away and fade outs
        };

        private bool mActive = false;

        [Header("** Runtime Variables **")]
        [SerializeField]
        private TextEffect mDamageTextEffectType = TextEffect.FADE;
        [SerializeField] private Vector3 mScale = Vector3.one;
        // the damage text moving speed
        [SerializeField] private float mMoveSpeed = 0.2f;
        [SerializeField] private float mSpacing = 0.25f;
        [SerializeField] private float mFadeSpeed = 1;
        // the lower order layer in the queue
        [SerializeField] private int mBaseOrderLayer = 5;
        [Header("** Caplitaize Effect **")]
        [SerializeField] private bool mCapitalizeLetter = true;
        [SerializeField] private Vector3 mCapitalLetterScale = new Vector3(2, 2, 2);
        [Header("** Wave Zigge Effect **")]
        [SerializeField] private bool mWaveZiggeEffect = true;
        [SerializeField] private float mWaveZigge = 0.1f;
        [Header("** Asymptotic Scale Effect **")]
        [SerializeField] private bool mAsymptoticScaleEffect = true;
        [SerializeField] private float mAsymptoticScale = 0.1f;

        // handle all the sprite in order to change the color
        private List<SpriteRenderer> mSpriteRenderers = null;

        [Header("** Critical Strike Sprite Setting **")]
        [SerializeField] private Vector3 mCritialSpriteScale = Vector3.one;
        [SerializeField] private float mSpacingX = 0.5f;
        [SerializeField] private float mSpacingY = 0.5f;
        [SerializeField] private bool mRandomSize = true;
        [SerializeField] private float mMinSize = -1;
        [SerializeField] private float mMaxSize = 1;

        // Damage Text
        [Header("** Damage Text Setting (if the game have this kind of feature fill this out!) **")]
        [SerializeField] private Sprite mDamageTextMiss = null;
        [SerializeField] private Sprite mCritialStrike = null;
        [SerializeField] private Sprite mDamageText0 = null;
        [SerializeField] private Sprite mDamageText1 = null;
        [SerializeField] private Sprite mDamageText2 = null;
        [SerializeField] private Sprite mDamageText3 = null;
        [SerializeField] private Sprite mDamageText4 = null;
        [SerializeField] private Sprite mDamageText5 = null;
        [SerializeField] private Sprite mDamageText6 = null;
        [SerializeField] private Sprite mDamageText7 = null;
        [SerializeField] private Sprite mDamageText8 = null;
        [SerializeField] private Sprite mDamageText9 = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool isActive() { return this.mActive; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mSpriteRenderers = new List<SpriteRenderer>();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            // Only for debug!
            Test();
#endif

            if (!mActive)
                return;

            // Do active thing
            DoEffect();
        }

        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.K))
            {
                SpawnDamageText(3363, new Vector2(1, 1));
                //print(GetSingleDigit(5, 1234));
            }

            if (JCS_Input.GetKeyDown(KeyCode.J))
            {
                SpawnDamageText(4805, new Vector2(1, 1));
                //print(GetSingleDigit(2, 4805));
            }

            if (JCS_Input.GetKeyDown(KeyCode.D))
            {
                RemoveAllTheChild();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void SpawnDamageText(int damage, Vector2 pos)
        {
            this.transform.position = pos;


            int totalDigit = damage.ToString().Length;

            bool isEvenNumber = false;
            if ((totalDigit % 2) == 0)
                isEvenNumber = true;

            for (int digit = 1;
                digit <= totalDigit;
                ++digit)
            {
                GameObject gm = new GameObject();
                SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();

                // add to manage
                mSpriteRenderers.Add(sr);

                // get single digit
                int digitNum = GetSingleDigit(digit, damage);

                // set the sprite base on digit number
                sr.sprite = GetSingleDigitSprite(digitNum);

                // set the sorting layer
                sr.sortingOrder = mBaseOrderLayer - digit;

                // set the parent
                gm.transform.SetParent(this.transform);

                // set the position base on the spacing 
                // and target's center!
                Vector3 newPos = gameObject.transform.position;
                newPos.x -= mSpacing * digit;

                // do wave effect
                if (mWaveZiggeEffect)
                {
                    float diffZig = mWaveZigge;

                    if ((digit % 2) == 0)
                        diffZig = -diffZig;

                    if (isEvenNumber)
                        diffZig = -diffZig;

                    newPos.y += diffZig;
                }
                gm.transform.position = newPos;

                Vector3 newScale = mScale;
                // Asymptotic scale
                if (mAsymptoticScaleEffect)
                {
                    float diffScale = mAsymptoticScale * digit;
                    newScale.x += diffScale;
                    newScale.y += diffScale;
                    newScale.z += diffScale;
                }

                // Capitalize Effect
                if (mCapitalizeLetter)
                {
                    if (digit == totalDigit)
                    {
                        newScale += mCapitalLetterScale;
                    }
                }

                gm.transform.localScale = newScale;
            }

            // Check if critical texture exist and spawn it
            if (mCritialStrike != null)
            {
                GameObject gm = new GameObject();
                SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();

                gm.transform.SetParent(this.transform);

                // add to manage
                mSpriteRenderers.Add(sr);

                // 最左邊的空位
                int theLeftDigitSpace = (totalDigit + 1);

                // set the sprite base on digit number
                sr.sprite = GetSingleDigitSprite(10);

                sr.sortingOrder = mBaseOrderLayer - theLeftDigitSpace;

                Vector3 newPos = gameObject.transform.position;
                newPos.x -= mSpacing * theLeftDigitSpace;

                // Adjust a bit so we have a little control
                // the position of this sprite! (Critical Image)
                newPos.x += mSpacingX;
                newPos.y += mSpacingY;

                gm.transform.position = newPos;

                //---------------
                // Apply Scale
                Vector3 newScale = mCritialSpriteScale;

                if (mRandomSize)
                {
                    // Make sure the setting is correct!
                    if (mMinSize < mMaxSize)
                    {
                        float applyRandom = Random.Range(mMinSize, mMinSize);
                        newScale.x += applyRandom;
                        newScale.y += applyRandom;
                    }
                    else
                        JCS_GameErrors.JcsErrors("JCS_DamageText", -1, "Max size cannot be smaller than Min size...");
                }

                gm.transform.localScale = newScale;
            }

            // start the effect
            mActive = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void RemoveAllTheChild()
        {
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                Destroy(this.transform.GetChild(index).gameObject);
            }

            // clear all the sprite from the child
            mSpriteRenderers.Clear();
        }
        private Sprite GetSingleDigitSprite(int num)
        {

            switch (num)
            {
                case 0: return mDamageText0;
                case 1: return mDamageText1;
                case 2: return mDamageText2;
                case 3: return mDamageText3;
                case 4: return mDamageText4;
                case 5: return mDamageText5;
                case 6: return mDamageText6;
                case 7: return mDamageText7;
                case 8: return mDamageText8;
                case 9: return mDamageText9;
                case 10: return mCritialStrike;
            }

            // careful!
            return null;
        }

        private int GetSingleDigit(int digit, int number)
        {
            int totalDigit = number.ToString().Length;
            if (digit > totalDigit)
                return -1;

            int digitCount = (int)Mathf.Pow(10, digit);

            int remainder = number % digitCount;
            int po = digit - 1;
            int divider = (int)Mathf.Pow(10, po);

            return remainder / divider;
        }

        private void DoEffect()
        {
            switch (mDamageTextEffectType)
            {
                case TextEffect.MOVE_UP:
                    MoveUp();
                    break;
                case TextEffect.FADE:
                    Fade();
                    break;
                case TextEffect.SLIDE_OUT:
                    SlideOut();
                    break;
            }
        }
        private void MoveUp()
        {
            Vector3 newPos = this.transform.position;
            newPos.y += mMoveSpeed * Time.deltaTime;
            this.transform.position = newPos;

            Fade();
        }
        private void Fade()
        {
            Color col = new Color();
            foreach (SpriteRenderer sr in mSpriteRenderers)
            {
                col = sr.color;
                col.a -= mFadeSpeed * Time.deltaTime;
                sr.color = col;
            }

            if (col.a <= 0.0f)
            {
                mActive = false;
                // end the effect!
                RemoveAllTheChild();
            }

        }
        private void SlideOut()
        {
            Fade();
        }

    }
}
