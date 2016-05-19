/**
 * $File: JCS_2DRotateAction.cs $
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
    public class JCS_2DRotateAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private float mStartingDegree = 0.0f;
        // How fast it rotate
        [SerializeField]
        private float mTurnSpeed = 1000.0f;

        [SerializeField]
        private JCS_2DFaceType mRotateDirection = JCS_2DFaceType.FACE_LEFT;

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

            // apply the starting position
            this.transform.Rotate(Vector3.forward, -mStartingDegree);
        }

        private void Update()
        {
            this.transform.Rotate(Vector3.forward * mTurnSpeed * -((int)mRotateDirection) * Time.deltaTime);
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
