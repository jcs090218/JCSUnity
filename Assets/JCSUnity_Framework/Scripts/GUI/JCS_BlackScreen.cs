/**
 * $File: JCS_BlackScreen.cs $
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

    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_BlackScreen : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_AlphaObject mAO = null;

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
            this.mAO = this.GetComponent<JCS_AlphaObject>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void FadeIn(float time)
        {
            mAO.FadeIn(time);
        }
        public void FadeOut(float time)
        {
            mAO.FadeOut(time);
        }
        public bool IsFadeIn()
        {
            return mAO.IsFadeIn();
        }
        public bool IsFadeOut()
        {
            return mAO.IsFadeOut();
        }

        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            Vector3 recordPos = this.transform.localPosition;
            Vector3 recordScale = this.transform.localScale;
            Quaternion recordRot = this.transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                this.transform.SetParent(null);
                this.transform.SetParent(parent);
            }

            // here we set it back!
            this.transform.localPosition = recordPos;
            this.transform.localScale = recordScale;
            this.transform.localRotation = recordRot;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
