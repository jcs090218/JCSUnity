/**
 * $File: JCS_2DRandAnimController.cs $
 * $Date: 2017-05-10 17:49:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Randomly play animation in animator object depends on time
    /// object which is the 'JCS_AdjustTimeTrigger' component.
    /// </summary>
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DRandAnimController
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private JCS_AdjustTimeTrigger mAdjustTimeTrigger = null;

        private JCS_2DAnimator m2DAnimator = null;


        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();
            this.mAdjustTimeTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            this.mAdjustTimeTrigger.actions = RandomPlayAnimationInAnimator;
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Play a random animation in the animator.
        /// </summary>
        private void RandomPlayAnimationInAnimator()
        {
            int animLength = m2DAnimator.Animations.Length;
            if (animLength == 0)
                return;

            int randIndex = JCS_Random.Range(0, animLength);

            // play this animation
            m2DAnimator.DoAnimation(randIndex);
        }

    }
}
