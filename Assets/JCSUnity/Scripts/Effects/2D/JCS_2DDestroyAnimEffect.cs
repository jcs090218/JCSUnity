/**
 * $File: JCS_2DDestroyAnimEffect.cs $
 * $Date: 2017-04-17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// When gameobject destroy, this will be activate and 
    /// play an animation. 
    /// 
    /// Differet from normal 'JCS_DestroyAnimEffect' was this
    /// uses the JCS_2DAnimation to display the animation.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_2DAnimPool))]
    public class JCS_2DDestroyAnimEffect
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // pool to grab animation to play.
        private JCS_2DAnimPool m2DAnimPool = null;


        [Header("** Runtime Variables (JCS_2DDestroyAnimEffectFinal) **")]

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
        private JCS_HitListEvent mHitList = null;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;


        [Header("** Position Settings (JCS_2DDestroyAnimEffectFinal) **")]

        [Tooltip("The same position as the destroyed game object?")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("The same rotation as the destroyed game object?")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("The same scale as the destroyed game object?")]
        [SerializeField]
        private bool mSameScale = true;


        [Header("** Random Effect (JCS_2DDestroyAnimEffectFinal) **")]

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

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.m2DAnimPool = this.GetComponent<JCS_2DAnimPool>();

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
                            "You active the destroy time but without the JCS_DestroyObjectWithTime component...");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;


            GameObject gm = new GameObject();
#if (UNITY_EDITOR)
            gm.name = "JCS_2DDestroyAnimEffect";
#endif

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;

            JCS_2DAnimation randAnim = m2DAnimPool.GetRandomAnim();
            JCS_2DAnimation newAnim = gm.AddComponent<JCS_2DAnimation>();

            newAnim.Active = randAnim.Active;
            newAnim.Loop = randAnim.Loop;
            newAnim.PlayOnAwake = randAnim.PlayOnAwake;
            newAnim.FramePerSec = randAnim.FramePerSec;

            // set the animation to just spawn animation.
            newAnim.SetAnimationFrame(randAnim.GetAnimationFrame());
            newAnim.Play();

            // add this event, so the when animation done play it will get destroy.
            JCS_2DDestroyAnimEndEvent das = gm.AddComponent<JCS_2DDestroyAnimEndEvent>();
            das.LoopTimes = mLoopTimes;

            if (mSamePosition)
                sr.transform.position = this.transform.position;
            if (mSameRotation)
                sr.transform.rotation = this.transform.rotation;
            if (mSameScale)
                sr.transform.localScale = this.transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(sr);
            if (mRandRot)
                AddRandomRotation(sr);
            if (mRandScale)
                AddRandomScale(sr);
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
        private void AddRandomPosition(SpriteRenderer dae)
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
        private void AddRandomRotation(SpriteRenderer dae)
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
        private void AddRandomScale(SpriteRenderer dae)
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
