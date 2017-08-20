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
        [SerializeField] [Range(0, 30000)]
        private float mDistance = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            // NOTE(jenchieh): prevent if other component want to change
            // this position, we have the record position at function 
            // 'Start' rather than 'Awake'
            this.mOriginPos = this.transform.localPosition;
        }

        private void Update()
        {
            if (!mActive)
                return;

            // Find out the distance between the current moved 
            // position and the starting position.
            float distance = Vector3.Distance(this.transform.localPosition, mOriginPos);

            // check if the distance reach?
            if (distance < mDistance)
                return;

            // set back to original position.
            this.transform.localPosition = this.mOriginPos;
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
