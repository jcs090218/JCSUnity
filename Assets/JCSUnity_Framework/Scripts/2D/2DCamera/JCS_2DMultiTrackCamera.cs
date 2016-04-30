/**
 * $File: JCS_2DMultiTrackCamera.cs $
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

    [RequireComponent(typeof(AudioListener))]
    public class JCS_2DMultiTrackCamera : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("Target List to track")]
        [SerializeField] private JCS_Player[] mTargets = null;

        private AudioListener mAudioListener = null;

        [SerializeField] private JCS_2DCamera mJCS_2DCamera = null;
        private string mJCS_2DCameraPath = "JCSUnity_Framework_Resources/JCS_2DCamera";

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        
        public AudioListener GetAudioListener() { return this.mAudioListener; }

        //========================================
        //      Unity's function
        //------------------------------
        protected void Awake()
        {

            mAudioListener = this.GetComponent<AudioListener>();


            if (mJCS_2DCamera == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DMultiTrackCamera", -1, "There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_UsefualFunctions.SpawnGameObject(
                    mJCS_2DCameraPath,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            // if still null, setting error!!
            if (mJCS_2DCamera == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DMultiTrackCamera", -1, "The object spawn does not have the \"JCS_2DCamera\" components...");
                return;
            }

            mJCS_2DCamera.SetFollowTarget(this.transform);
        }

        private void Start()
        {
            JCS_SoundManager.instance.SetAudioListener(GetAudioListener());
        }

        private void Update()
        {
            this.transform.position = CalculateTheCameraPosition();
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
        private Vector3 CalculateTheCameraPosition()
        {
            // no target trackable
            if (mTargets.Length == 0)
                return transform.position;

            float minHeight = 0, 
                maxHeight = 0, 
                minWidth = 0, 
                maxWidth = 0;

            bool firstAssign = false;

            foreach (JCS_Player p in mTargets)
            {
                if (p == null)
                    continue;

                Transform trans = p.transform;

                // found the first object
                if (!firstAssign)
                {
                    minWidth = trans.position.x;
                    maxWidth = trans.position.x;
                    minHeight = trans.position.y;
                    maxHeight = trans.position.y;
                    firstAssign = true;
                    continue;
                }
                else
                {
                    // if other object is height than the other
                    // override the min/max value
                    if (trans.position.x < minWidth)
                        minWidth = trans.position.x;
                    if (trans.position.x > maxWidth)
                        maxWidth = trans.position.x;

                    if (trans.position.y < minHeight)
                        minHeight = trans.position.y;
                    if (trans.position.y > maxHeight)
                        maxHeight = trans.position.y;
                }
            }

            float finalPosY = ((maxHeight - minHeight) / 2) + minHeight;
            float finalPosX = ((maxWidth - minWidth) / 2) + minWidth;


            return new Vector3(finalPosX, finalPosY, transform.position.z);
        }

    }
}
