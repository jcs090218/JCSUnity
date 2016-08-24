/**
 * $File: JCS_CollisionManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_CollisionManager
    : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_CollisionManager instance = null;

        //----------------------
        // Private Variables

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
        }

        private void Start()
        {
            SetCollisionMode();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Call this everytime u spawn a object 
        /// with character controller components on it.
        /// </summary>
        public void SetCollisionMode()
        {
            if (!JCS_GameSettings.instance.IGNORE_EACH_OTHER_CHARACTER_CONTROLLER)
                return;

            CharacterController[] controllers = Resources.FindObjectsOfTypeAll<CharacterController>();

            // Make all the character controller ignore each other
            for (int index = 0;
                index < controllers.Length;
                ++index)
            {
                for (int pairIndex = index + 1;
                    pairIndex < controllers.Length;
                    ++pairIndex)
                {

                    Physics.IgnoreCollision(
                            controllers[index],
                            controllers[pairIndex], true);
                }
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
