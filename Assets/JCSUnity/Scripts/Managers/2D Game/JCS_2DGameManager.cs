/**
 * $File: JCS_2DGameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_2DGameManager
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_2DGameManager instance = null;

        //----------------------
        // Private Variables
        
        // a list of item object will ignore their collision detection.
        private List<JCS_2DPositionPlatform> mPlatformList = null;

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

            mPlatformList = new List<JCS_2DPositionPlatform>();
        }

        private void Update()
        {

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void AddPlatformList(JCS_2DPositionPlatform pp)
        {
            mPlatformList.Add(pp);
        }
        public void IgnoreAllPlatformTrigger(Collider c)
        {
            for (int index = 0;
                index < mPlatformList.Count;
                ++index)
            {
                Physics.IgnoreCollision(mPlatformList[index].GetPlatformTrigger(), 
                    c, true);
            }
        }
        public void IgnoreAllPlatformCollider(Collider c)
        {
            for (int index = 0;
                index < mPlatformList.Count;
                ++index)
            {
                Physics.IgnoreCollision(mPlatformList[index].GetPlatformCollider(),
                    c, true);
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
