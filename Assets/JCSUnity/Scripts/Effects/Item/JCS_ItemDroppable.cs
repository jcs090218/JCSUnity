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
        [SerializeField]
        [Range(0, 100)]
        private float mPossiblityDropAction = 100;

        [Tooltip("Number of Item Drap possibilty will increase exponentially.")]
        [SerializeField]
        [Range(1, 1000)]
        private int mDropRate = 1;

        [Tooltip("How many item u want to drop?")]
        [SerializeField]
        [Range(0, 100)]
        private int mMinNumItemDrop = 0;

        [Tooltip("How many item u want to drop?")]
        [SerializeField]
        [Range(0, 100)]
        private int mMaxNumItemDrop = 0;

        [Tooltip("Item Set for droping thing")]
        [SerializeField]
        private ItemSet[] mItemSet = null;


        [Header("Gravity Effect (JCS_ItemDroppable)")]

        [Tooltip("")]
        [SerializeField]
        private bool mIsGravity = true;

        [Tooltip("")]
        [SerializeField]
        [Range(0, 120)]
        private float mJumpForce = 10;

        [Tooltip("Randomize the jump force to each item?")]
        [SerializeField]
        private bool mRandomizeJumpForce = false;

        [Tooltip("Add this force to item")]
        [SerializeField]
        [Range(0.01f, 5)]
        private float mRandomizeJumpForceForce = 0;

        [Header("- Rotate Effect ")]

        [Tooltip("")]
        [SerializeField]
        private bool mRotateWhileDropping = true;

        [Tooltip("")]
        [SerializeField]
        private float mRotateSpeed = 1000;


        [Header("Spread Effect (JCS_ItemDroppable)")]

        [Tooltip("")]
        [SerializeField]
        private bool mSpreadEffect = true;

        [Tooltip("")]
        [SerializeField]
        private float mSpreadGap = 0.4f;

        [Tooltip("")]
        [SerializeField]
        private bool mIncludeDepth = false;


        [Header("Destroy Effect (JCS_ItemDroppable)")]

        [Tooltip("")]
        [SerializeField]
        private bool mDestroyFadeOutEffect = true;

        [Tooltip("")]
        [SerializeField]
        private float mDestroyTime = 30;

        [Tooltip("")]
        [SerializeField]
        private float mFadeTime = 1;


        [Header("Others (JCS_ItemDroppable)")]

        [Tooltip("")]
        [SerializeField]
        private bool mConstWaveEffect = true;


        [Header("** Audio (JCS_ItemDroppable) **")]

        [Tooltip("Drop Sound")]
        [SerializeField]
        private AudioClip mDropSound = null;

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

        /// <summary>
        /// Calculate possibility and drop the item.
        /// </summary>
        public void DropItems()
        {
            if (mMinNumItemDrop > mMaxNumItemDrop)
            {
                JCS_Debug.JcsErrors("JCS_ItemDroppable", "No item drop. min max.");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mustDropItem"></param>
        /// <param name="specIndex"></param>
        /// <param name="only"> only drop this item. </param>
        /// <param name="count"></param>
        public void DropItems(JCS_Item mustDropItem, int specIndex = -1, bool only = false, int count = 1)
        {
            if (mustDropItem == null)
            {
                JCS_Debug.JcsErrors(
                    this, "Must drop item cannot be null references...");

                return;
            }

            if (count <= 0)
            {
                JCS_Debug.JcsErrors(
                    this, "Cannot drop item with count less or equal to zero...");

                return;
            }


            if (only)
            {
                // just
                int itemDrop = count;

                bool isEven = ((itemDrop % 2) == 0) ? true : false;

                for (int index = 0;
                    index < count;
                    ++index)
                {
                    // simple assign the item.
                    JCS_Item item = mustDropItem;

                    // do drop the item.
                    DropAnItem(item, index, isEven);
                }

            }
            else
            {
                if (mMinNumItemDrop > mMaxNumItemDrop)
                {
                    JCS_Debug.JcsErrors("JCS_ItemDroppable", "No item drop. min max.");
                    return;
                }

                // calculate and see if we do the drop action.
                float doDrop = JCS_Utility.JCS_FloatRange(0, 100);
                if (doDrop > mPossiblityDropAction)
                    return;

                // start doing the drop action.

                int itemDrop = JCS_Utility.JCS_IntRange(mMinNumItemDrop, mMaxNumItemDrop + 1) * mDropRate + count;

                bool isEven = ((itemDrop % 2) == 0) ? true : false;

                int randDropIndex = specIndex;


                // check index out of range.
                if (specIndex < 0 || specIndex >= itemDrop)
                    randDropIndex = JCS_Utility.JCS_IntRange(0, itemDrop);


                for (int index = 0;
                    index < itemDrop;
                    ++index)
                {
                    JCS_Item item = null;

                    if (index == randDropIndex)
                    {
                        // assign must drop item.
                        item = mustDropItem;
                    }
                    else
                        item = ItemDropped();

                    if (item == null)
                        continue;

                    DropAnItem(item, index, isEven);
                }
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
            DropAnItem(
                item,
                index,
                isEven,
                mIsGravity,
                mSpreadEffect,
                mRotateWhileDropping,
                mConstWaveEffect,
                mDestroyFadeOutEffect);
        }

        /// <summary>
        /// Drop an item.
        /// </summary>
        /// <param name="item"> item u want to spawn </param>
        /// <param name="index"> index to know the force this is pushing to. </param>
        /// <param name="isEven"> is the index even number? </param>
        /// <param name="isGravity"> do gravity effect. </param>
        /// <param name="spreadEffect"> do spread effect. </param>
        /// <param name="rotateDrop"> rotate while dropping. </param>
        /// <param name="waveEffect"> do wave effect while on the ground. </param>
        /// <param name="destroyFade"> while picking it up will fade and destroy. </param>
        private void DropAnItem(
            JCS_Item item,
            int index,
            bool isEven,
            bool isGravity,
            bool spreadEffect,
            bool rotateDrop,
            bool waveEffect,
            bool destroyFade)
        {
            JCS_Item jcsi = (JCS_Item)JCS_Utility.SpawnGameObject(
               item,
               this.transform.position,
               this.transform.rotation);

            bool isEvenIndex = ((index % 2) == 0) ? true : false;

            if (isGravity)
            {

                JCS_OneJump jcsoj = jcsi.gameObject.AddComponent<JCS_OneJump>();

                float gapDirection = mSpreadGap;
                if (isEvenIndex)
                    gapDirection = -mSpreadGap;

                float gapForce = 0;

                if (spreadEffect)
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

                float jumpForce = mJumpForce;
                if (mRandomizeJumpForce)
                {
                    jumpForce += JCS_Utility.JCS_FloatRange(-mRandomizeJumpForceForce, mRandomizeJumpForceForce);
                }

                jcsoj.DoForce(gapForce, jumpForce, mIncludeDepth);

                if (rotateDrop)
                {
                    JCS_ItemRotation jcsir = jcsi.gameObject.AddComponent<JCS_ItemRotation>();
                    jcsir.RotateSpeed = mRotateSpeed;
                    jcsir.Effect = true;

                    // if z axis interact in game
                    if (mIncludeDepth)
                    {
                        // add one more axis.
                        JCS_ItemRotation jcsir2 = jcsi.gameObject.AddComponent<JCS_ItemRotation>();
                        jcsir2.RotateSpeed = JCS_Utility.JCS_FloatRange(-mRotateSpeed, mRotateSpeed);
                        jcsir2.Effect = true;
                        jcsir2.RotateDirection = JCS_Vector3Direction.UP;
                    }
                }
            }

            if (waveEffect)
            {
                JCS_2DConstWaveEffect jcscw = jcsi.gameObject.AddComponent<JCS_2DConstWaveEffect>();
                jcscw.Effect = true;
            }

            if (destroyFade)
            {
                JCS_DestroyObjectWithTime jcsao = jcsi.gameObject.AddComponent<JCS_DestroyObjectWithTime>();
                jcsao.GetFadeObject().FadeTime = mFadeTime;
                jcsao.DestroyTime = mDestroyTime;

                // set the object type the same.
                jcsao.GetFadeObject().SetObjectType(item.GetObjectType());

                jcsao.GetFadeObject().UpdateUnityData();
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

            float accumMaxDropRate = 0;
            float accumMinDropRate = 0;

            for (int index = 0;
                index < mItemSet.Length;
                ++index)
            {
                accumMaxDropRate += mItemSet[index].dropRate;

                if (index == 0)
                {
                    if (JCS_Utility.WithInRange(
                        0,
                        mItemSet[0].dropRate,
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
                        accumMinDropRate,
                        accumMaxDropRate,
                        dropIndex))
                {
                    item = mItemSet[index].item;
                    break;
                }

                accumMinDropRate += mItemSet[index].dropRate;
            }

            // meaning the last one.
            if (item == null &&
                mItemSet.Length != 0 &&
                mItemSet[mItemSet.Length - 1].dropRate != 0)
            {
                item = mItemSet[mItemSet.Length - 1].item;
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
