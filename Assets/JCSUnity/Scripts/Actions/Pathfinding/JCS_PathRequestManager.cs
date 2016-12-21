/**
 * $File: JCS_PathRequestManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(JCS_Pathfinding))]
    public class JCS_PathRequestManager
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PathRequestManager instance = null;

        //----------------------
        // Private Variables

        private Queue<PathRequest> mPathRequestQueue = new Queue<PathRequest>();
        private PathRequest mCurrentPathRequest;

        private JCS_Pathfinding mPathfinding = null;

        private bool mIsProcessingPath = false;

        /// <summary>
        /// 
        /// </summary>
        private struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }
        }

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
            instance = this;

            mPathfinding = this.GetComponent<JCS_Pathfinding>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathStart"></param>
        /// <param name="pathEnd"></param>
        /// <param name="callback"></param>
        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.mPathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="success"></param>
        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            mCurrentPathRequest.callback(path, success);
            mIsProcessingPath = false;
            TryProcessNext();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        private void TryProcessNext()
        {
            if (!mIsProcessingPath && mPathRequestQueue.Count > 0)
            {
                mCurrentPathRequest = mPathRequestQueue.Dequeue();
                mIsProcessingPath = true;
                mPathfinding.StartFindPath(mCurrentPathRequest.pathStart, mCurrentPathRequest.pathEnd);
            }
        }

    }
}
