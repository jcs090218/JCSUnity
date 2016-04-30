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
    public class JCS_2DPortal : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField]
        private JCS_2DPortalType mType = JCS_2DPortalType.SCENE_PORTAL;

        //** SCENE_PORTAL
        [SerializeField]
        private string mSceneName = "JCS_Demo";

        //** TRANSFER_PORTAL
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
            JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();
            if (other.gameObject.name == player.name)
            {
                DoPortal(mType);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoPortal(JCS_2DPortalType type)
        {
            JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();

            switch (type)
            {
                case JCS_2DPortalType.SCENE_PORTAL:
                    {
                        if (JCS_Input.GetKey(KeyCode.UpArrow))
                            LoadScene();
                    }
                    break;
                case JCS_2DPortalType.TRANSFER_PORTAL:
                    {
                        if (JCS_Input.GetKeyDown(KeyCode.UpArrow))
                        {
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
