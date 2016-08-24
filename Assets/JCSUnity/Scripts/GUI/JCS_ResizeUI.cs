/**
 * $File: JCS_ResizeUI.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{

    [RequireComponent(typeof(RectTransform))]
    public class JCS_ResizeUI 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_ResizeUI instance = null;

        //----------------------
        // Private Variables
        private RectTransform mRect = null;
        private float mWScale = 0;
        private float mHScale = 0;
        private float mTargetScale = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public RectTransform GetResizeRect() { return this.mRect; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mRect = this.GetComponent<RectTransform>();

            // if this is the root object set this as un destroyable
            this.gameObject.AddComponent<JCS_UniqueObject>();

            if (instance != null)
            {

                string black_screen_name = JCS_GameSettings.BLACK_SCREEN_NAME;
                string white_screen_name = JCS_GameSettings.WHITE_SCREEN_NAME;

                // cuz the transform list will change while we set the transform to 
                // the transform, 
                List<Transform> readyToSetList = new List<Transform>();

                Transform tempTrans = instance.transform;
                for (int index = 0;
                    index < tempTrans.childCount;
                    ++index)
                {
                    Transform child = tempTrans.GetChild(index);
                    if (child.name == black_screen_name ||
                        child.name == (black_screen_name + "(Clone)"))
                        continue;

                    if (child.name == white_screen_name ||
                        child.name == (white_screen_name + "(Clone)"))
                        continue;

                    if (child.name == "JCS_IgnorePanel")
                        continue;

                    // TODO(JenChieh): optimize this?
                    if (child.GetComponent<JCS_IgnoreDialogueObject>() != null)
                        continue;

                    // add to set list ready to set to the new transform as parent
                    readyToSetList.Add(child);
                }

                // set to the new transform
                foreach (Transform trans in readyToSetList)
                {
                    // set parent to the new canvas in the new scene
                    trans.SetParent(this.transform);
                }

                // Delete the old one
                DestroyImmediate(instance.gameObject);
            }
            

            instance = this;

            JCS_Canvas.instance.SetResizeUI(this);
        }

        private void Update()
        {
            ResizeUI();
        }

        public void ResizeUI()
        {
            if (mRect == null)
                return;

            mWScale = Screen.width / mRect.sizeDelta.x;
            mHScale = Screen.height / mRect.sizeDelta.y;
            mTargetScale = (mWScale > mHScale) ? mHScale : mWScale;
            transform.localScale = Vector3.one * mTargetScale;
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

    }
}
