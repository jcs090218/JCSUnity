/**
 * $File: JCS_DestroyAnimEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_AnimPool))]
    public class JCS_DestroyAnimEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        private JCS_AnimPool mDestroyAnim = null;
        [SerializeField] private int mOrderLayer = 1;
        [Tooltip("How many times to animate then destroy.")]
        [SerializeField] private int mLoopTimes = 1;

        //-- Hit List
        [SerializeField] private bool mDestroyWithHitList = true;
        private JCS_HitListEvent mHitList = null;

        //-- Time
        [SerializeField] private bool mDestroyWithTime = false;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        [Header("** Settings **")]
        [Tooltip("The same position as the destroyed game object?")]
        [SerializeField] private bool mSamePosition= true;
        [Tooltip("The same rotation as the destroyed game object?")]
        [SerializeField] private bool mSameRotation = true;
        [Tooltip("The same scale as the destroyed game object?")]
        [SerializeField] private bool mSameScale = true;

        [Header("** Random Effect **")]
        [Tooltip("Enable/Disable Random Position Effect")]
        [SerializeField] private bool mRandPos = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)] private float mRandPosRange = 0;

        [Tooltip("Enable/Disable Random Rotation Effect")]
        [SerializeField] private bool mRandRot = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)] private float mRandRotRange = 0;

        [Tooltip("Enable/Disable Random Scale Effect")]
        [SerializeField] private bool mRandScale = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)] private float mRandScaleRange = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int LoopTimes { get { return this.mLoopTimes; } set { this.mLoopTimes = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mDestroyAnim = this.GetComponent<JCS_AnimPool>();

            mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_ApplicationManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.instance.IsSwitchingScene())
                return;
            
            // TODO(Jen-Chieh): 意義不明...
            if (mDestroyWithHitList)
            {
                if (!mHitList.IsHit)
                    return;
            }

            if (mDestroyWithTime)
            {

                if (mDestroyObjectWithTime != null)
                {
                    if (mDestroyObjectWithTime.TimesUp)
                        return;
                }
            }

            GameObject gm = new GameObject();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroyAnimEffect";
#endif

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = mDestroyAnim.GetRandomAnim();

            JCS_DestroyAnimEndEvent dae = gm.AddComponent<JCS_DestroyAnimEndEvent>();
            dae.LoopTimes = LoopTimes;

            if (mSamePosition)
                dae.transform.position = this.transform.position;
            if (mSameRotation)
                dae.transform.rotation = this.transform.rotation;
            if (mSameScale)
                dae.transform.localScale = this.transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(dae);
            if (mRandRot)
                AddRandomRotation(dae);
            if (mRandScale)
                AddRandomScale(dae);
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
        private void AddRandomPosition(JCS_DestroyAnimEndEvent dae)
        {
            float addPos;
            Vector3 newPos = dae.transform.position;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            // Not sure we have to include z direction?
            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.z += addPos;

            dae.transform.position = newPos;
        }
        private void AddRandomRotation(JCS_DestroyAnimEndEvent dae)
        {
            float addRot;
            Vector3 newRot = dae.transform.localEulerAngles;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.x += addRot;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.y += addRot;

            // Not sure we have to include z direction?
            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.z += addRot;

            dae.transform.localEulerAngles = newRot;
        }
        private void AddRandomScale(JCS_DestroyAnimEndEvent dae)
        {
            float addScale;
            Vector3 newScale = dae.transform.localScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.x += addScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.y += addScale;

            // Not sure we have to include z direction?
            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.z += addScale;

            dae.transform.localScale = newScale;
        }

    }
}
