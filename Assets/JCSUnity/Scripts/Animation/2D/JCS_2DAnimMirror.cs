/**
 * $File: JCS_2DAnimMirror.cs $
 * $Date: 2017-05-09 11:53:08 $
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
    /// Completely mimic a JCS_2DAnimation's frame.
    /// </summary>
    public class JCS_2DAnimMirror
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Runtime Variables (JCS_2DAnimMirrorAction) **")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Animation that we are going to use to mimic.")]
        [SerializeField]
        private JCS_2DAnimation mMirrorAnimation = null;

        [Tooltip("Animations going to follow the main animation.")]
        [SerializeField]
        private List<JCS_2DAnimation> mMimicAnimations = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public JCS_2DAnimation MirrorAnimation { get { return this.mMirrorAnimation; } set { this.mMirrorAnimation = value; } }
        public List<JCS_2DAnimation> MimicAnimations { get { return this.mMimicAnimations; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            InitMimicAnimations();
        }

        private void LateUpdate()
        {
            if (!Active)
                return;

            DoMimicAnimations();
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
        /// Initialize all the minicing animations.
        /// </summary>
        private void InitMimicAnimations()
        {
            if (mMimicAnimations.Count >= 0)
                return;

            foreach (JCS_2DAnimation anim in mMimicAnimations)
            {
                if (anim == null)
                    continue;

                anim.Active = false;
                anim.PlayOnAwake = false;
                anim.Loop = false;
            }
        }

        /// <summary>
        /// Do all the mimicing for all the mimic animations.
        /// 
        /// Simply just have them exact the same frame.
        /// </summary>
        private void DoMimicAnimations()
        {
            /* Cannot have mirror animation as a null reference... */
            if (mMirrorAnimation == null)
                return;

            foreach (JCS_2DAnimation anim in mMimicAnimations)
            {
                if (anim == null)
                    continue;

                if (mMirrorAnimation.LocalSprite == null)
                    anim.LocalSprite = null;
                else
                    anim.PlayFrame(mMirrorAnimation.CurrentPlayingFrame);
            }
        }

    }
}
