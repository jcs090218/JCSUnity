/**
 * $File: JCS_DestroySpawnEffect.cs $
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
    /// Effect that spawn a gameobject after this gameobject is destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformPool))]
    public class JCS_DestroySpawnEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_TransformPool mTransformPool = null;
        private JCS_HitListEvent mHitList = null;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;


        [Header("** Runtime Variables (JCS_TransformPool) **")]

        [Tooltip("How many transform spawn?")]
        [SerializeField]
        private int mSpawnCount = 0;

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


        [Header("** Transform Settings (JCS_DestroySpawnEffect) **")]

        [Tooltip("Play the animation as the same position as the destroyed gameobject.")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("Play the animation as the same rotation as the destroyed gameobject.")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("Play the animation as the same scale as the destroyed gameobject.")]
        [SerializeField]
        private bool mSameScale = true;


        [Header("** Random Effect (JCS_DestroySpawnEffect) **")]

        [Tooltip("Randomize the position when spawn the gameobject.")]
        [SerializeField]
        private bool mRandPos = false;

        [Tooltip("Random position value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRange = 0.0f;

        [Tooltip("Randomize the rotation when spawn the gameobject.")]
        [SerializeField]
        private bool mRandRot = false;

        [Tooltip("Random rotation value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandRotRange = 0.0f;

        [Tooltip("Randomize the scale when spawn the gameobject.")]
        [SerializeField]
        private bool mRandScale = false;

        [Tooltip("Random scale value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRange = 0.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool ActiveWhatever { get { return this.mActiveWhatever; } set { this.mActiveWhatever = value; } }
        public bool ActiveWithHitList { get { return this.mActiveWithHitList; } set { this.mActiveWithHitList = value; } }
        public bool ActiveWithDestroyTime { get { return this.mActiveWithDestroyTime; } set { this.mActiveWithDestroyTime = value; } }

        public bool SamePosition { get { return this.mSamePosition; } set { this.mSamePosition = value; } }
        public bool SameRotation { get { return this.mSameRotation; } set { this.mSameRotation = value; } }
        public bool SameScale { get { return this.mSameScale; } set { this.mSameScale = value; } }

        public bool RandPos { get { return this.mRandPos; } set { this.mRandPos = value; } }
        public bool RandRot { get { return this.mRandRot; } set { this.mRandRot = value; } }
        public bool RandScale { get { return this.mRandScale; } set { this.mRandScale = value; } }
        public float RandPosRange { get { return this.mRandPosRange; } set { this.mRandPosRange = value; } }
        public float RandRotRange { get { return this.mRandRotRange; } set { this.mRandRotRange = value; } }
        public float RandScaleRange { get { return this.mRandScaleRange; } set { this.mRandScaleRange = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mTransformPool = this.GetComponent<JCS_TransformPool>();

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


            // spawn the game object.
            DoSpawn();
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
        /// Do the spawning.
        /// </summary>
        private void DoSpawn()
        {
            for (int index = 0;
                index < mSpawnCount;
                ++index)
            {
                Transform newTrans = (Transform)JCS_Utility.SpawnGameObject(mTransformPool.GetRandomObject());

                if (mSamePosition)
                    newTrans.position = this.transform.position;
                if (mSameRotation)
                    newTrans.rotation = this.transform.rotation;
                if (mSameScale)
                    newTrans.localScale = this.transform.localScale;

                // Random Effect
                if (mRandPos)
                    AddRandomPosition(newTrans);
                if (mRandRot)
                    AddRandomRotation(newTrans);
                if (mRandScale)
                    AddRandomScale(newTrans);
            }
        }

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(Transform trans)
        {
            float addPos;
            Vector3 newPos = trans.position;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            // Not sure we have to include z direction?
            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.z += addPos;

            trans.position = newPos;
        }

        /// <summary>
        /// Add random value to the effect's transform's rotation.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomRotation(Transform trans)
        {
            float addRot;
            Vector3 newRot = trans.localEulerAngles;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.x += addRot;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.y += addRot;

            // Not sure we have to include z direction?
            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.z += addRot;

            trans.localEulerAngles = newRot;
        }

        /// <summary>
        /// Add random value to the effect's transform's scale.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomScale(Transform trans)
        {
            float addScale;
            Vector3 newScale = trans.localScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.x += addScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.y += addScale;

            // Not sure we have to include z direction?
            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.z += addScale;

            trans.localScale = newScale;
        }

    }
}
