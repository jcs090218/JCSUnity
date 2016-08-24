/**
 * $File: JCS_2DDropItemAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Simulate the drop item action.
    /// Control the compoenent in order to do this action.
    /// </summary>
    [RequireComponent(typeof(JCS_ItemDroppable))]
    public class JCS_2DDropItemAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_ItemDroppable mItemDroppable = null;

        [Header("** Drop when Dies Settings **")]

        [Tooltip("Drop once when the object is dead.")]
        [SerializeField] private bool mDropWhenDies = true;


        [Header("** Drop by time Settings **")]

        [Tooltip("Drop by time.")]
        [SerializeField] private bool mDropByTime = false;

        [Tooltip("Time to drop one time.")]
        [SerializeField] [Range(0, 10)]
        private float mTimePerDrop = 0;

        [Tooltip("Effecting the time every time it drop.")]
        [SerializeField] [Range(0, 5)]
        private float mRandomTimeRange = 0;

        // a time combine random + timer per drop value.
        private float mDropRealTime = 0;

        // timer to calculate weather to do drop action or not
        private float mDropTimer = 0;

        // trigger to re-calculate the drop time
        private bool mDroped = false;

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
            mItemDroppable = this.GetComponent<JCS_ItemDroppable>();
        }

        private void Update()
        {
            DoDropByTime();
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


            // check the event is true
            if (!mDropWhenDies)
                return;

            // do effects
            mItemDroppable.DropItems();
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
        /// Main function runs drop by time.
        /// </summary>
        private void DoDropByTime()
        {
            if (!mDropByTime)
                return;

            ResetTime();

            mDropTimer += Time.deltaTime;

            if (mDropRealTime < mDropTimer)
            {
                // Do drop action.
                mItemDroppable.DropItems();

                // re-calculate next drop
                mDroped = true;
            }

        }

        /// <summary>
        /// Once we do the drop item action, 
        /// re-calculate the next drop time.
        /// so every time it drop is random.
        /// </summary>
        private void ResetTime()
        {
            if (!mDroped)
                return;

            // get the offset random time.
            float randTime = JCS_Utility.JCS_FloatRange(-mRandomTimeRange, mRandomTimeRange);

            // formula, on timing design here.
            mDropRealTime = randTime + mTimePerDrop;

            // reset timer
            mDropTimer = 0;

            mDroped = false;
        }

    }
}
