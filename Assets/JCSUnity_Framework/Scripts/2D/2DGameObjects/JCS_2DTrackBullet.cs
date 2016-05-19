/**
 * $File: JCS_2DTrackBullet.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_2DTrackAction))]
    [RequireComponent(typeof(JCS_2DGoStraightAction))]
    [RequireComponent(typeof(JCS_DestroyObjectWithTime))]
    [RequireComponent(typeof(JCS_HitCountEvent))]
    public class JCS_2DTrackBullet
        : JCS_Bullet
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_2DTrackAction mTrackAction = null;
        private JCS_2DGoStraightAction mGoStraightAction = null;

        [Header("** Initialize Varialbes **")]
        [SerializeField] private float mDelayTimeToFollow = 0;
        private float mDelayTimeToFollowTimer = 0;

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
            mTrackAction = this.GetComponent<JCS_2DTrackAction>();
            mTrackAction.Following = false;


            mGoStraightAction = this.GetComponent<JCS_2DGoStraightAction>();
            mGoStraightAction.enabled = true;
        }

        private void Update()
        {
            DoCounter();
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
        private void DoCounter()
        {
            // is already following!
            if (mTrackAction.Following)
                return;

            mDelayTimeToFollowTimer += Time.deltaTime;

            if (mDelayTimeToFollow < mDelayTimeToFollowTimer)
            {
                // start follow!
                mTrackAction.Following = true;

                mGoStraightAction.enabled = false;
                mDelayTimeToFollowTimer = 0;
            }
        }

    }
}
