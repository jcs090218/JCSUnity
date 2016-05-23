/**
 * $File: JCS_ItemDroppable.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
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
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_ItemDroppable
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [System.Serializable]
        public struct ItemSet
        {
            [Tooltip("Possibilty droping this item, the higher the more possiblility")]
            [Range(0, 100)] public uint dropRate;
            [Tooltip("Item to drop")]
            public JCS_Item item;
        };

        [Header("** Runtime Variables **")]
        [Tooltip("Number of Item Drap possibilty will increase exponentially.")]
        [SerializeField] private uint mDropRate = 1;
        [Tooltip("How many item u want to drop")]
        [SerializeField] private uint mMinNumItemDrop = 0;
        [SerializeField] private uint mMaxNumItemDrop = 0;

        [Tooltip("Item Set for droping thing")]
        [SerializeField] private ItemSet[] mItemSet = null;

        // [Header("** Item's Effect Settings **")]
        [Header("Gravity Effect (Item)")]
        [SerializeField] private bool mIsGravity = true;
        [SerializeField] private float mJumpForce = 10;
        [Header("- Rotate Effect")]
        [SerializeField] private bool mRotateWhileDropping = true;
        [SerializeField] private float mRotateSpeed = 1000;

        [Header("Spread Effect (Item)")]
        [SerializeField] private bool mSpreadEffect = true;
        [SerializeField] private float mSpreadGap = 1;

        [Header("Destroy Effect (Item)")]
        [SerializeField] private bool mDestroyFadeOutEffect = true;
        [SerializeField] private float mDestroyTime = 3;
        [SerializeField] private float mFadeTime = 1;

        [Header("Others (Item)")]
        [SerializeField] private bool mConstWaveEffect = true;

        [Header("** Audio **")]
        private JCS_SoundPlayer mSoundPlayer = null;
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
        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

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
                JCS_GameErrors.JcsErrors("JCS_ItemDroppable", -1, "No item drop. min max.");
                return;
            }

            uint itemDrop = JCS_UsefualFunctions.JCS_IntRange(mMinNumItemDrop, mMaxNumItemDrop) * mDropRate;

            bool isEven = ((itemDrop % 2) == 0) ? true : false;


            for (uint index = 0;
                index < itemDrop;
                ++index)
            {
                JCS_Item item = ItemDropped();

                DropAItem(item, index, isEven);
            }

            // play drop sound.
            PlayDropSound();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void DropAItem(JCS_Item item, uint index, bool isEven)
        {
            JCS_Item jcsi = (JCS_Item)JCS_UsefualFunctions.SpawnGameObject(
                item,
                this.transform.position, 
                this.transform.rotation);
            jcsi.Drop();

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
                jcsao.GetAlphaObject().FadeObjecType = JCS_UnityObjectType.SPRITE;
                jcsao.GetAlphaObject().UpdateUnityData();
            }

        }

        private JCS_Item ItemDropped()
        {
            JCS_Item item = null;

            uint totalChance = 0;

            for (uint index = 0;
                index < mItemSet.Length;
                ++index)
            {
                totalChance += mItemSet[index].dropRate;
            }

            uint dropIndex = JCS_UsefualFunctions.JCS_IntRange(0, totalChance);

            

            for (int index = 0;
                index < mItemSet.Length - 1;
                ++index)
            {
                if (index == 0)
                {
                    if (dropIndex >= 0 && 
                        dropIndex <= mItemSet[0].dropRate)
                    {
                        item = mItemSet[0].item;
                        break;
                    }
                    continue;
                }

                if (mItemSet[index].dropRate >= dropIndex &&
                    mItemSet[index + 1].dropRate <= dropIndex)
                {
                    item = mItemSet[index].item;
                    break;
                }
            }

            if (item == null)
            {
                // if item still null than mush be the last one!
                item = mItemSet[mItemSet.Length - 1].item;
            }


            return item;
        }

        private void PlayDropSound()
        {
            if (mDropSound == null)
                return;

            mSoundPlayer.PlayOneShot(mDropSound);
        }

    }
}
