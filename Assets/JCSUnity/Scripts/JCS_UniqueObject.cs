/**
 * $File: JCS_UniqueObject.cs $
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
    /// Singleton patter in Unity scripting layer.
    /// </summary>
    public class JCS_UniqueObject 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Tooltip("")]
        [SerializeField]
        private JCS_SingletonObject mType = JCS_SingletonObject.NONE;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_SingletonObject GetSingletonObjectType() { return this.mType; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            CheckByType();

            // only the root object can do this.
            if (this.transform.parent == null)
                DontDestroyOnLoad(this.gameObject);
            else
            {
                JCS_Debug.JcsWarnings(
                    this, "Only the root object can be use DontDestoryOnLoad...");
            }
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

        /// <summary>
        /// Check the signleton by type.
        /// 
        /// NOTE(jenchieh): Non-use.
        /// </summary>
        private void CheckByType()
        {
            //switch (mType)
            //{
            //    case JCS_SingletonObject.JCS_MANAGERS:
            //        {
            //            JCS_Managers[] jc = GameObject.FindObjectsOfType<JCS_Managers>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.JcsErrors("JCS_UniqueObject",   "Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }
            //        }
            //        break;

            //    case JCS_SingletonObject.JCS_CANVAS:
            //        {
            //            JCS_Canvas[] jc = GameObject.FindObjectsOfType<JCS_Canvas>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.JcsErrors("JCS_UniqueObject",   "Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }

            //        }
            //        break;

            //    case JCS_SingletonObject.JCS_GAMESETTINGS:
            //        {
            //            JCS_GameSettings[] jc = GameObject.FindObjectsOfType<JCS_GameSettings>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.JcsErrors("JCS_UniqueObject",   "Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }

            //        } break;
            //}
            
        }

    }
}
