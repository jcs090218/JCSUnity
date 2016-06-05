/**
 * $File: JCS_2DPortal.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{

    public class JCS_2DPortal 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables **")]
        [SerializeField]
        private KeyCode mKeyToTrigger = KeyCode.UpArrow;
        [SerializeField] private bool mAutoTrigger = false;


        [Header("Choose portal type.")]
        [SerializeField]
        private JCS_2DPortalType mType = JCS_2DPortalType.SCENE_PORTAL;

        //** SCENE_PORTAL
        [Header("** Scene Portal Settings **")]
        [SerializeField]
        private string mSceneName = "JCS_Demo";

        //** TRANSFER_PORTAL
        [Header("** Transfer Portal Settings **")]
        [SerializeField]
        private Transform mTargetPortal = null;


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
            if (mTargetPortal == null &&
                mType == JCS_2DPortalType.TRANSFER_PORTAL)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPortal", -1, "");
                return;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            JCS_Player p = other.GetComponent<JCS_Player>();
            if (p == null)
                return;

            DoPortal(mType, p);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoPortal(JCS_2DPortalType type, JCS_Player player)
        {

            switch (type)
            {
                case JCS_2DPortalType.SCENE_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKey(mKeyToTrigger))
                                LoadScene();
                        }
                        else
                        {
                            // auto do the action
                            LoadScene();
                        }
                    }
                    break;
                case JCS_2DPortalType.TRANSFER_PORTAL:
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
        public void LoadScene()
        {
            JCS_SceneManager.instance.LoadScene(mSceneName);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
