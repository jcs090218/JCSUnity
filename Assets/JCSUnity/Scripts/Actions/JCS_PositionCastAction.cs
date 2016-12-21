/**
 * $File: JCS_PositionCastAction.cs $
 * $Date: 2016-12-01 02:55:56 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Cast the current transform to certain space.
    /// 
    /// NOTE(jenchieh): 
    /// Screen: from [0,0] to [Screen.width, Screen.height]
    /// Viewport: from[-1, -1] to[1, 1]
    /// </summary>
    public class JCS_PositionCastAction
        : JCS_UnityObject
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_PositionCastAction) **")]

        [Tooltip("Check to see the current casting position.")]
        [SerializeField]
        private Vector3 mCastPosition = Vector3.zero;

        [Tooltip("Key to test if cast to screen space.")]
        [SerializeField]
        private KeyCode mCastToScreenKey = KeyCode.S;

        [Tooltip("Key to test if cast to world soace.")]
        [SerializeField]
        private KeyCode mCastToWorldKey = KeyCode.W;
#endif

        [Header("** Runtime Variables (JCS_PositionCastAction) **")]

        [Tooltip("")]
        [SerializeField]
        private Vector3 mPositionOffset = Vector3.zero;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
#if (UNITY_EDITOR)
        public Vector3 CastPosition { get { return this.mCastPosition; } }
#endif
        public Vector3 PositionOffset { get { return this.mPositionOffset; } set { this.mPositionOffset = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // update unity object data once.
            UpdateUnityData();
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (JCS_Input.GetKeyDown(mCastToScreenKey))
                mCastPosition = CastToScreen(mCastPosition);
            if (JCS_Input.GetKeyDown(mCastToWorldKey))
                mCastPosition = CastToWorld(mCastPosition);

            if (JCS_Input.GetKeyDown(KeyCode.A))
            {
                print(JCS_Camera.main.WorldToCanvasSpace(new Vector3(99, 55, 11)));
            }
            if (JCS_Input.GetKeyDown(KeyCode.B))
            {
                print(JCS_Camera.main.CanvasToWorldSpace(new Vector2(2545.7f, 1414.3f)));
            }
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 CastToScreen(Vector3 pos)
        {
            JCS_Camera jcsCam = JCS_Camera.main;

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.TEXT:
                case JCS_UnityObjectType.UI:
                    {
                        this.LocalPosition = jcsCam.WorldToCanvasSpace(pos) + (Vector2)mPositionOffset;
                    }
                    break;
            }

            return this.LocalPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 CastToWorld(Vector2 pos)
        {
            JCS_Camera jcsCam = JCS_Camera.main;

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                case JCS_UnityObjectType.SPRITE:
                    {
                        this.LocalPosition = jcsCam.CanvasToWorldSpace(pos) + mPositionOffset;
                    }
                    break;
            }

            return this.LocalPosition;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
