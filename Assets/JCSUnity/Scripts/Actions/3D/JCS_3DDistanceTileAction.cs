/**
 * $File: JCS_3DDistanceTileAction.cs $
 * $Date: 2017-04-07 02:04:46 $
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
    /// Move a game object in certain distance then set the game object
    /// back to original position.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    public class JCS_3DDistanceTileAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private Vector3 mOriginPos = Vector3.zero;


        [Header("** Runtime Variables (JCS_3DDistanceTileAction) **")]

        [Tooltip("Is the component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("How long this game object could tracvel.")]
        [SerializeField]
        [Range(0, 30000)]
        private float mDistance = 0;

        [Tooltip("Use the local position instead of global position.")]
        [SerializeField]
        private bool mUseLocalPosition = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool UseLocalPosition { get { return this.mUseLocalPosition; } set { this.mUseLocalPosition = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            // NOTE(jenchieh): prevent if other component want to change
            // this position, we have the record position at function 
            // 'Start' rather than 'Awake'
            if (mUseLocalPosition)
                this.mOriginPos = this.transform.localPosition;
            else
                this.mOriginPos = this.transform.position;
        }

        private void Update()
        {
            if (!mActive)
                return;

            // Find out the distance between the current moved 
            // position and the starting position.
            float distance = 0.0f;
            if (mUseLocalPosition)
                distance = Vector3.Distance(this.transform.localPosition, mOriginPos);
            else 
                distance = Vector3.Distance(this.transform.position, mOriginPos);

            // check if the distance reach?
            if (distance < mDistance)
                return;

            // set back to original position.
            if (mUseLocalPosition)
                this.transform.localPosition = this.mOriginPos;
            else
                this.transform.position = this.mOriginPos;
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

    }
}
