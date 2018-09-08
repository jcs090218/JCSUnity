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
    /// <summary>
    /// This responsoble handle all the gui into correct proportion and 
    /// right scaling.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_ResizeUI 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_ResizeUI instance = null;

        public static float W_PREV_SCALE = 0.0f;

        public static float H_PREV_SCALE = 0.0f;

        public Vector2 PREV_SIZE_DELTA = Vector2.zero;

        public Vector3 PREV_SCALE = Vector3.zero;

        //----------------------
        // Private Variables

        private RectTransform mRect = null;

        [Header("** Check Variables (JCS_ResizeUI) **")]

        [Tooltip("Width scale.")]
        [SerializeField]
        private float mWScale = 0;

        [Tooltip("Height scale.")]
        [SerializeField]
        private float mHScale = 0;

        [Tooltip("Target scale.")]
        [SerializeField]
        private float mTargetScale = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public RectTransform GetResizeRect() { return this.mRect; }
        public float TargetScale { get { return this.mTargetScale; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mRect = this.GetComponent<RectTransform>();

            RevertResizeUI();

            // if this is the root object set this as un destroyable
            this.gameObject.AddComponent<JCS_UniqueObject>();

            if (instance != null)
            {
                string black_screen_name = JCS_UISettings.BLACK_SCREEN_NAME;
                string white_screen_name = JCS_UISettings.WHITE_SCREEN_NAME;

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

        private void Start()
        {
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

#if (UNITY_5_4_OR_NEWER)
            // TODO(JenChieh): unknown reason that 
            //                something changes this to 
            //                somewhere else. (since 5.4.0f3)
            Vector3 tempPos = appRect.localPosition;
            tempPos.z = 0;
            this.transform.localPosition = Vector3.zero;
#endif

            this.transform.localEulerAngles = Vector3.zero;
        }

        private void Update()
        {
            ResizeUI();
        }

        private void OnDestroy()
        {
            W_PREV_SCALE = mWScale - 1.0f;
            H_PREV_SCALE = mHScale - 1.0f;

            PREV_SIZE_DELTA = mRect.sizeDelta;
            PREV_SCALE = mRect.localScale;
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
        /// Resize the UI if screen size changes.
        /// </summary>
        private void ResizeUI()
        {
            if (mRect == null)
                return;

            mWScale = (float)Screen.width / mRect.sizeDelta.x + W_PREV_SCALE;
            mHScale = (float)Screen.height / mRect.sizeDelta.y + H_PREV_SCALE;

            mTargetScale = (mWScale > mHScale) ? mHScale : mWScale;

            mRect.localScale = Vector3.one * mTargetScale;
        }

        /// <summary>
        /// Revert the resize ui info.
        /// </summary>
        private void RevertResizeUI()
        {
            mWScale = (float)Screen.width / mRect.sizeDelta.x + W_PREV_SCALE;
            mHScale = (float)Screen.height / mRect.sizeDelta.y + H_PREV_SCALE;

            // Get back the resize UI.
            if (PREV_SIZE_DELTA != Vector2.zero)
                mRect.sizeDelta = PREV_SIZE_DELTA;

            if (PREV_SCALE != Vector3.zero)
                mRect.localScale = PREV_SCALE;
        }
    }
}
