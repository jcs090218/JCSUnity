/**
 * $File: JCS_WebcamButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    public class JCS_WebcamButton
        : JCS_Button
    {
        [SerializeField] private JCS_Webcam mWebcam = null;

        protected override void Awake()
        {
            base.Awake();

            // try to get the webcam from the scene.
            if (mWebcam == null)
                mWebcam = (JCS_Webcam)FindObjectOfType(typeof(JCS_Webcam));
        }

        public override void JCS_ButtonClick()
        {
            // current only one function applied,
            // so just do take snap shot as defualt action!
            TakeSnapshotWebcam();

            // do call back
            base.JCS_ButtonClick();
        }

        private void TakeSnapshotWebcam()
        {
            if (mWebcam == null)
            {
                JCS_GameErrors.JcsReminders(
                    "JCS_WebcamButton",
                     
                    "Assign the button but no webcam attached or in the scene.");

                return;
            }

            mWebcam.TakeSnapshotWebcam();
        }
    }
}
