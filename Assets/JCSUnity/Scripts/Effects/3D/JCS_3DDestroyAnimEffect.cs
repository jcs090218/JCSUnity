/**
 * $File: JCS_3DDestroyAnimEffect.cs $
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

    /// <summary>
    /// When gameobject destroy, this will be activate and 
    /// play an animation.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_AnimPool))]
    public class JCS_3DDestroyAnimEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_AnimPool mDestroyAnim = null;
        private JCS_HitListEvent mHitList = null;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;


        [Header("** Runtime Variables (JCS_2DDestroyAnimEffect) **")]

        [Tooltip("Sorting layer this effect going to render.")]
        [SerializeField]
        private int mOrderLayer = 1;

        [Tooltip("How many times to animate then destroy.")]
        [SerializeField]
        private int mLoopTimes = 1;

        //-- Hit List
        [Tooltip("Active this effect by what ever this object is destoryed.")]
        [SerializeField]
        private bool mActiveWhatever = false;

        [Tooltip("Active the effect by hitting the certain object.")]
        [SerializeField]
        private bool mActiveWithHitList = true;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;


        [Header("** Position Settings (JCS_2DDestroyAnimEffect) **")]

        [Tooltip("The same position as the destroyed game object?")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("The same rotation as the destroyed game object?")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("The same scale as the destroyed game object?")]
        [SerializeField]
        private bool mSameScale = true;

        [Header("** Random Effect (JCS_2DDestroyAnimEffect) **")]

        [Tooltip("Enable/Disable Random Position Effect")]
        [SerializeField]
        private bool mRandPos = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandPosRange = 0;

        [Tooltip("Enable/Disable Random Rotation Effect")]
        [SerializeField]
        private bool mRandRot = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandRotRange = 0;

        [Tooltip("Enable/Disable Random Scale Effect")]
        [SerializeField]
        private bool mRandScale = false;
        [SerializeField]
        [Tooltip("Range will be within this negative to positive!")]
        [Range(0, 10)]
        private float mRandScaleRange = 0;

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

            this.mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
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


            // trigger this effect?
            bool onTrigger = false;

            if (mActiveWhatever)
            {
                onTrigger = true;
            }
            // no need to check the rest.
            else
            {
                // if checking for hit list
                if (mActiveWithHitList)
                {
                    if (mHitList.IsHit)
                        onTrigger = true;
                }

                // if checking for destroy time.
                if (mActiveWithDestroyTime)
                {
                    if (mDestroyObjectWithTime != null)
                    {
                        if (mDestroyObjectWithTime.TimesUp)
                            onTrigger = true;
                    }
                    else
                    {
                        JCS_Debug.LogError(
                            this, "You active the destroy time but without the JCS_DestroyObjectWithTime component...");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;


            GameObject gm = new GameObject();
#if (UNITY_EDITOR)
            gm.name = "JCS_3DDestroyAnimEffect";
#endif

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = mDestroyAnim.GetRandomAnim();

            // add this event, so the when animation done play it will get destroy.
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

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
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

        /// <summary>
        /// Add random value to the effect's transform's rotation.
        /// </summary>
        /// <param name="dae"> effect transform </param>
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

        /// <summary>
        /// Add random value to the effect's transform's scale.
        /// </summary>
        /// <param name="dae"> effect transform </param>
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
