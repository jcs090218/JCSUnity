/**
 * $File: JCS_2DOrderAnimPlayer.cs $
 * $Date: 2017-05-10 18:23:15 $
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
    /// Play animator's animations in order base on if the animation are 
    /// done playing it.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DOrderAnimPlayer
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private JCS_2DAnimator mAnimator = null;

        [Header("** Check Variables (JCS_2DOrderAnimPlayer) **")]

        [Tooltip("Current playing animation id.")]
        [SerializeField]
        private int mCurrentAnimationId = 0;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Start()
        {
            this.mAnimator = this.GetComponent<JCS_2DAnimator>();

            // start the animation from the first id.
            mAnimator.DoAnimation(0, true);
        }

        private void Update()
        {
            // check if animation done playing.
            if (!mAnimator.CurrentAnimation.IsDonePlaying)
                return;

            // add up for next animation.
            ++mCurrentAnimationId;

            // reset anim id.
            if (mCurrentAnimationId == mAnimator.Animations.Length)
                mCurrentAnimationId = 0;

            // play animaiton.
            mAnimator.DoAnimation(mCurrentAnimationId, false, true);
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

    }
}
