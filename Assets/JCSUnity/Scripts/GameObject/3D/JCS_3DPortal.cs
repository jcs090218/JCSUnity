/**
 * $File: JCS_3DPortal.cs $
 * $Date: 2016-11-06 17:25:43 $
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
    /// 
    /// </summary>
    public class JCS_3DPortal
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_3DPortal) **")]

        [Tooltip("")]
        [SerializeField]
        private KeyCode mKeyToTrigger = KeyCode.UpArrow;

        [Tooltip("")]
        [SerializeField]
        private bool mAutoTrigger = false;


        [Header("Choose portal type. (JCS_2DPortal) ")]

        [Tooltip("")]
        [SerializeField]
        private JCS_3DPortalType mType = JCS_3DPortalType.SCENE_PORTAL;

        //** SCENE_PORTAL
        [Header("** Scene Portal Settings (JCS_2DPortal) **")]

        [Tooltip("")]
        [SerializeField]
        private string mSceneName = "JCS_Demo";

        //** TRANSFER_PORTAL
        [Header("** Transfer Portal Settings (JCS_2DPortal) **")]

        [Tooltip("")]
        [SerializeField]
        private Transform mTargetPortal = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool AutoTrigger { get { return this.mAutoTrigger; } set { this.mAutoTrigger = value; } }
        public JCS_3DPortalType Type { get { return this.mType; } set { this.mType = value; } }
        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public Transform TargetPortal { get { return this.mTargetPortal; } set { this.mTargetPortal = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            if (mTargetPortal == null &&
                mType == JCS_3DPortalType.TRANSFER_PORTAL)
            {
                JCS_Debug.LogError(this, "Transform portal does not exists.");
                return;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            JCS_Player p = other.GetComponent<JCS_Player>();
            if (p == null)
                return;

            // switch the scene.
            DoPortal(mType, p);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        public void DoPortal(JCS_3DPortalType type, JCS_Player player)
        {
            switch (type)
            {
                case JCS_3DPortalType.SCENE_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKey(mKeyToTrigger))
                                JCS_SceneManager.instance.LoadScene(mSceneName);
                        }
                        else
                        {
                            // auto do the action
                            JCS_SceneManager.instance.LoadScene(mSceneName);
                        }
                    }
                    break;
                case JCS_3DPortalType.TRANSFER_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKeyDown(mKeyToTrigger))
                            {
                                if (mTargetPortal != null)
                                    player.transform.position = mTargetPortal.position;
                            }
                        }
                        else
                        {
                            // auto do the action
                            if (mTargetPortal != null)
                                player.transform.position = mTargetPortal.position;
                        }
                    }
                    break;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
