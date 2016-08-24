/**
 * $File: JCS_ItemDroppable.cs $
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
    /// Make the object drop,
    /// plz attach this script.
    /// </summary>
    [RequireComponent(typeof(JCS_ItemIgnore))]
    public class JCS_ItemDroppable
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        [System.Serializable]
        public struct ItemSet
        {
            [Tooltip("Possibilty droping this item, the higher the more possiblility")]
            [Range(0, 100)]
            public float dropRate;
            [Tooltip("Item to drop")]
            public JCS_Item item;
        };

        //----------------------
        // Private Variables


        [Header("** Runtime Variables (JCS_ItemDroppable) **")]

        [Tooltip(@"Weather or drop will depends on 
this variables first, before to do the actually 
compare algorithm")]
        [SerializeField] [Range(0, 100)]
        private float mPossiblityDropAction = 100;

        [Tooltip("Number of Item Drap possibilty will increase exponentially.")]
        [SerializeField] [Range(1, 1000)]
        private int mDropRate = 1;

        [Tooltip("How many item u want to drop")]
        [SerializeField] [Range(0, 100)]
        private int mMinNumItemDrop = 0;
        [SerializeField] [Range(0,100)]
        private int mMaxNumItemDrop = 0;

        [Tooltip("Item Set for droping thing")]
        [SerializeField] private ItemSet[] mItemSet = null;

        // [Header("** Item's Effect Settings **")]
        [Header("Gravity Effect (JCS_ItemDroppable)")]
        [SerializeField] private bool mIsGravity = true;
        [SerializeField] private float mJumpForce = 10;
        [Header("- Rotate Effect ")]
        [SerializeField] private bool mRotateWhileDropping = true;
        [SerializeField] private float mRotateSpeed = 1000;

        [Header("Spread Effect (JCS_ItemDroppable)")]
        [SerializeField] private bool mSpreadEffect = true;
        [SerializeField] private float mSpreadGap = 0.4f;

        [Header("Destroy Effect (JCS_ItemDroppable)")]
        [SerializeField] private bool mDestroyFadeOutEffect = true;
        [SerializeField] private float mDestroyTime = 30;
        [SerializeField] private float mFadeTime = 1;

        [Header("Others (JCS_ItemDroppable)")]
        [SerializeField] private bool mConstWaveEffect = true;


        [Header("** Audio (JCS_ItemDroppable) **")]
        [Tooltip("Drop Sound")]
        [SerializeField] private AudioClip mDropSound = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.B))
                DropItems();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DropItems()
        {
            if (mMinNumItemDrop > mMaxNumItemDrop)
            {
                JCS_GameErrors.JcsErrors("JCS_ItemDroppable", "No item drop. min max.");
                return;
            }

            // calculate and see if we do the drop action.
            float doDrop = JCS_Utility.JCS_FloatRange(0, 100);
            if (doDrop > mPossiblityDropAction)
                return;

            // start doing the drop action.

            int itemDrop = JCS_Utility.JCS_IntRange(mMinNumItemDrop, mMaxNumItemDrop + 1) * mDropRate;

            bool isEven = ((itemDrop % 2) == 0) ? true : false;


            for (int index = 0;
                index < itemDrop;
                ++index)
            {
                JCS_Item item = ItemDropped();

                if (item == null)
                    continue;

                DropAnItem(item, index, isEven);
            }

            // play drop sound.
            PlayDropSound();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Drop an item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="isEven"></param>
        private void DropAnItem(JCS_Item item, int index, bool isEven)
        {
            JCS_Item jcsi = (JCS_Item)JCS_Utility.SpawnGameObject(
                item,
                this.transform.position, 
                this.transform.rotation);

            bool isEvenIndex = ((index % 2) == 0) ? true : false;

            if (mIsGravity)
            {
                
                JCS_OneJump jcsoj = jcsi.gameObject.AddComponent<JCS_OneJump>();

                float gapDirection = mSpreadGap;
                if (isEvenIndex)
                    gapDirection = -mSpreadGap;

                float gapForce = 0;

                if (mSpreadEffect)
                {

                    if (isEven)
                    {
                        if (!isEvenIndex)
                        {
                            gapForce = (gapDirection * (index - 1)) + gapDirection;
                        }
                        else
                        {
                            gapForce = (gapDirection * (index)) + gapDirection;
                        }
                    }
                    // if total is odd
                    else
                    {
                        if (isEvenIndex)
                        {
                            gapForce = (gapDirection * (index));
                        }
                        else
                        {
                            gapForce = (gapDirection * (index)) + gapDirection;
                        }
                    }

                    
                }

                jcsoj.DoForce(gapForce, mJumpForce);

                if (mRotateWhileDropping)
                {
                    JCS_ItemRotation jcsir = jcsi.gameObject.AddComponent<JCS_ItemRotation>();
                    jcsir.RotateSpeed = mRotateSpeed;
                    jcsir.Effect = true;
                }
            }

            if (mConstWaveEffect)
            {
                JCS_2DConstWaveEffect jcscw = jcsi.gameObject.AddComponent<JCS_2DConstWaveEffect>();
                jcscw.Effect = true;
            }

            if (mDestroyFadeOutEffect)
            {
                JCS_DestroyObjectWithTime jcsao = jcsi.gameObject.AddComponent<JCS_DestroyObjectWithTime>();
                jcsao.GetAlphaObject().FadeTime = mFadeTime;
                jcsao.DestroyTime = mDestroyTime;
                jcsao.GetAlphaObject().SetObjectType(JCS_UnityObjectType.SPRITE);
                jcsao.GetAlphaObject().UpdateUnityData();
            }

        }

        /// <summary>
        /// Decide what item to drop base on
        /// the array list we have!
        /// </summary>
        /// <returns> item to drop. </returns>
        private JCS_Item ItemDropped()
        {
            JCS_Item item = null;

            float totalChance = 0;

            // add all possiblity chance together.
            for (int index = 0;
                index < mItemSet.Length;
                ++index)
            {
                totalChance += mItemSet[index].dropRate;
            }

            float dropIndex = JCS_Utility.JCS_FloatRange(0, totalChance + 1);

            float accumDropRate = 0;

            for (int index = 0;
                index < mItemSet.Length;
                ++index)
            {
                accumDropRate += mItemSet[index].dropRate;

                if (index == 0)
                {
                    if (JCS_Utility.WithInRange(
                        0, mItemSet[0].dropRate, 
                        dropIndex))
                    {
                        item = mItemSet[0].item;
                        break;
                    }

                    continue;
                }

                // 比如: 10, 20, 30, 40

                // Loop 1: 0 ~ 10
                // Loop 2: 20(30-10) ~ 30
                // Loop 3: 30(60-30) ~ 60
                // Loop 4: 40(100-60) ~ 100     每個都減掉上一個的Drop Rate!
                if (JCS_Utility.WithInRange(
                        accumDropRate - mItemSet[index - 1].dropRate,
                        accumDropRate,
                        dropIndex))
                {
                    item = mItemSet[index].item;
                    break;
                }
            }


            return item;
        }

        /// <summary>
        /// Play one drop sound if drop 
        /// sound is assigned.
        /// </summary>
        private void PlayDropSound()
        {
            if (mDropSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mDropSound);
        }

    }
}
