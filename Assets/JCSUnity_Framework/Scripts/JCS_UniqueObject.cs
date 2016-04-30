/**
 * $File: JCS_UniqueObject.cs $
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
    public class JCS_UniqueObject : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private JCS_SingletonObject mType = JCS_SingletonObject.NONE;

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
            CheckAndDontDestroy();
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
        private void CheckAndDontDestroy()
        {
            switch (mType)
            {
                case JCS_SingletonObject.JCS_MANAGERS:
                    {
                        JCS_Managers[] jc = GameObject.FindObjectsOfType<JCS_Managers>();

                        if (jc.Length != 1)
                        {
                            JCS_GameErrors.JcsErrors("JCS_UniqueObject", -1, "Too many " + mType.ToString() + " in the scene...(Delete)");
                            DestroyImmediate(this.gameObject);
                            return;
                        }
                    }
                    break;

                case JCS_SingletonObject.JCS_CANVAS:
                    {
                        JCS_Canvas[] jc = GameObject.FindObjectsOfType<JCS_Canvas>();

                        if (jc.Length != 1)
                        {
                            JCS_GameErrors.JcsErrors("JCS_UniqueObject", -1, "Too many " + mType.ToString() + " in the scene...(Delete)");
                            DestroyImmediate(this.gameObject);
                            return;
                        }

                    }
                    break;

                case JCS_SingletonObject.JCS_GAMESETTINGS:
                    {
                        JCS_GameSettings[] jc = GameObject.FindObjectsOfType<JCS_GameSettings>();

                        if (jc.Length != 1)
                        {
                            JCS_GameErrors.JcsErrors("JCS_UniqueObject", -1, "Too many " + mType.ToString() + " in the scene...(Delete)");
                            DestroyImmediate(this.gameObject);
                            return;
                        }

                    } break;
            }

            DontDestroyOnLoad(this.gameObject);
        }

    }
}
