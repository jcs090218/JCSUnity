/**
 * $File: JCS_2DPortal.cs $
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
    /// 
    /// </summary>
    public class JCS_2DPortal
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_2DPortal) **")]

        [Tooltip("Is this portal enable to use?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Use a key to trigger.")]
        [SerializeField]
        private KeyCode mKeyToTrigger = KeyCode.UpArrow;

        [Tooltip("If on trigger enter will trigger immediately.")]
        [SerializeField]
        private bool mAutoTrigger = false;


        [Header("Choose portal type. (JCS_2DPortal) ")]

        [Tooltip("")]
        [SerializeField]
        private JCS_2DPortalType mType = JCS_2DPortalType.SCENE_PORTAL;


        //** SCENE_PORTAL
        [Header("** Scene Portal Settings (JCS_2DPortal) **")]

        [Tooltip("")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulateScene";

        [Tooltip("Lable of the portal.")]
        [SerializeField]
        private JCS_PortalLabel mPortalLabel = JCS_PortalLabel.NONE;


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
        public bool Active { get { return this.mActive; } }
        public bool AutoTrigger { get { return this.mAutoTrigger; } set { this.mAutoTrigger = value; } }
        public JCS_2DPortalType Type { get { return this.mType; } set { this.mType = value; } }
        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public Transform TargetPortal { get { return this.mTargetPortal; } set { this.mTargetPortal = value; } }
        public JCS_PortalLabel PortalLabel { get { return this.mPortalLabel; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            if (mTargetPortal == null &&
                mType == JCS_2DPortalType.TRANSFER_PORTAL)
            {
                JCS_Debug.LogError( "Transform portal does not exists.");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player"></param>
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

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        private void LoadScene()
        {
            // set the portal label, in order to let the next scene load.
            if (JCS_PortalSettings.instance != null)
            {
                JCS_PortalSettings.instance.SCENE_PORTAL_LABEL = mPortalLabel;
            }

            // auto do the action
            JCS_SceneManager.instance.LoadScene(mSceneName);
        }

    }
}
