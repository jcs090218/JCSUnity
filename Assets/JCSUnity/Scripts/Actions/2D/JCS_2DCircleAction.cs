/**
 * $File: JCS_2DCircleAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Do the circle action.
    /// </summary>
    public class JCS_2DCircleAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables **")]
        [SerializeField] private bool mDoMovement = false;
        [SerializeField] private bool mDoRotate = false;

        [Header("** Initialize Variables **")]
        [SerializeField] private float mStartingDegree = 0.0f;
        [SerializeField] private int mRotateDegree = 360;
        private float mRotateDegreeTimer = 0;

        [Header("** Runtime Variables **")]
        // How fast the force apply
        [SerializeField] private float mMoveSpeed = 10.0f;

        // How fast it rotate
        [SerializeField] private float mTurnSpeed = 150.0f;

        // Rotate Direction
        [SerializeField] private JCS_2DFaceType mRotateDirection = JCS_2DFaceType.FACE_LEFT;


        //[Header("** After Rotate Movement **")]
        //[SerializeField] private bool mAfterRotateWalkStraight = true;
        //[SerializeField] private bool mAfterRotateWalkOpposite = false;
        //[SerializeField] private bool mAfterRotateWalkOppositeCycle = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float StartingPosition { get { return this.mStartingDegree; } set { this.mStartingDegree = value; } }
        public bool DoMovement { get { return this.mDoMovement; } set { this.mDoMovement = value; } }
        public bool DoRotate { get { return this.mDoRotate; } set { this.mDoRotate = value; } }

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
            Movement();
            Rotate();
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
        private void Movement()
        {
            if (!mDoMovement)
                return;

            // Do movement
            this.transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        }
        private void Rotate()
        {
            if (!mDoRotate)
                return;

            mRotateDegreeTimer += mTurnSpeed * Time.deltaTime;

            if (mRotateDegreeTimer > mRotateDegree)
            {
                mRotateDegreeTimer = 0;
                mDoRotate = false;
                return;
            }

            // Do rotation!
            this.transform.Rotate(Vector3.forward * mTurnSpeed * -((int)mRotateDirection) * Time.deltaTime);
        }

    }
}
