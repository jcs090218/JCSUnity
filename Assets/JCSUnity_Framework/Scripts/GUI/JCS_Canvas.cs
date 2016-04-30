/**
 * $File: JCS_Canvas.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class JCS_Canvas : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_Canvas instance = null;

        //----------------------
        // Private Variables
        [SerializeField] private Canvas mCanvas = null;
        [SerializeField] private JCS_ResizeUI mResizeUI = null;
        [SerializeField] private string mResizeUI_path = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/ResizeUI";

        private RectTransform mAppRect = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public RectTransform GetAppRect() { return this.mAppRect; }
        public Canvas GetCanvas() { return this.mCanvas; }
        public void SetResizeUI(JCS_ResizeUI ui) { this.mResizeUI = ui; }
        public JCS_ResizeUI GetResizeUI() { return this.mResizeUI; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {

            // if this is the root object set this as un destroyable
            this.gameObject.AddComponent<JCS_UniqueObject>();

            if (instance != null)
            {
                JCS_GameErrors.JcsErrors("JCS_Canvas", -1, "There are too many Canvas object in the scene. (Delete)");

                string black_screen_name = JCS_GameSettings.BLACK_SCREEN_NAME;
                string game_ui_name = JCS_GameSettings.GAME_UI_NAME;

                // cuz the transform list will change while we set the transform to 
                // the transform, 
                List<Transform> readyToSetList = new List<Transform>();


                Transform tempTrans = instance.transform;
                // so record all the transform
                for (int index = 0;
                    index < tempTrans.childCount;
                    ++index)
                {
                    Transform child = tempTrans.GetChild(index);
                    if (child.name == black_screen_name ||
                        child.name == (black_screen_name + "(Clone)"))
                        continue;

                    if (child.name == game_ui_name ||
                        child.name == (game_ui_name + "(Clone)"))
                        continue;

                    if (child.name == "JCS_IgnorePanel")
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
            

            // attach the new one
            instance = this;


            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();

            JCS_UIManager.instance.SetJCSCanvas(this);

            if (JCS_GameSettings.instance.RESIZE_UI)
            {
                // resizable UI in order to resize the UI correctly
                GameObject gm = JCS_UsefualFunctions.SpawnGameObject(mResizeUI_path);
                gm.transform.SetParent(this.transform);
            }
        }

        private void Start()
        {
            if (JCS_GameSettings.instance.RESIZE_UI)
            {
                if (mResizeUI == null)
                {
                    JCS_GameErrors.JcsErrors("JCS_Canvas");
                    return;
                }

                // get the screen width and height
                Vector2 actualRect = this.GetAppRect().sizeDelta;

                // set it to the right resolution
                mResizeUI.GetResizeRect().sizeDelta = actualRect;
            }
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
