/**
 * $File: JCS_Input.cs $
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
    public enum JCS_MouseButton
    {
        NONE = -1,
        LEFT = 0,
        RIGHT = 1,
        MIDDLE = 2
    };

    /// <summary>
    /// 
    /// </summary>
    public class JCS_Input 
    {
        private static bool mClick = false;
        private static float mClickTime = 0.25f;
        private static float mClickTimer = 0;

        /// <summary>
        /// This sould be in the Update() function.
        /// </summary>
        /// <param name="type"> type by JCS_InputType (self-define) </param>
        /// <returns> true: if double click, false nothing happens </returns>
        public static bool OnMouseDoubleClick(JCS_InputType type)
        {
            return OnMouseDoubleClick((int)type);
        }
        /// <summary>
        /// This sould be in the Update() function.
        /// </summary>
        /// <param name="button"> type by keycode (Unity built-in) </param>
        /// <returns> true: if double click, false nothing happens </returns>
        public static bool OnMouseDoubleClick(int button)
        {
            // Check first click
            if (!mClick)
            {
                if (JCS_Input.GetMouseButtonDown(button))
                    mClick = true;
            }
            // Check double click
            else
            {
                mClickTimer += Time.deltaTime;

                if (JCS_Input.GetMouseButtonDown(button))
                {
                    mClickTimer = 0;
                    mClick = false;
                    return true;
                }

                if (mClickTime < mClickTimer)
                {
                    mClick = false;
                    mClickTimer = 0;
                }
            }

            return false;
        }


        // mouse delta position.
        private static Vector3 mMouseDeltaPosition = Vector3.zero;
        // record down last mouse position, in order to
        // get the updated delta mouse position. (variable above.)
        private static Vector3 mLasDeltaPosition = Vector3.zero;

        /// <summary>
        /// Return mouse delta position between frame
        /// to frame.
        /// </summary>
        /// <returns> delta value </returns>
        public static Vector3 MouseDeltaPosition()
        {
            Vector3 currentPosition = Input.mousePosition;
            mMouseDeltaPosition = currentPosition - mLasDeltaPosition;
            mLasDeltaPosition = currentPosition;

            return mMouseDeltaPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> mouse position on canvas </returns>
        public static Vector3 CanvasMousePosition()
        {
            Vector2 pos;
            Canvas myCanvas = JCS_Canvas.instance.GetCanvas();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                myCanvas.transform as RectTransform, 
                Input.mousePosition, 
                myCanvas.worldCamera, 
                out pos);

            return myCanvas.transform.TransformPoint(pos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vector3 MousePositionOnGUILayer()
        {
            Vector3 guiMousePosition = Input.mousePosition;

            // original point is at the center of the screen,  
            // so set (0, 0) back to bottom left
            guiMousePosition.x -= Screen.width / 2;
            guiMousePosition.y -= Screen.height / 2;

            return guiMousePosition;
        }

        /// <summary>
        /// Return mouse position between 0 to 1.
        /// </summary>
        /// <returns> values </returns>
        public static Vector2 MousePosition0To1()
        {
            float mouseRatioX = Input.mousePosition.x / Screen.width;
            float mouseRatioY = Input.mousePosition.y / Screen.height;

            return new Vector2(mouseRatioX, mouseRatioY);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseByAction(JCS_KeyActionType act, JCS_MouseButton button)
        {
            return GetMouseByAction(act, (int)button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseByAction(JCS_KeyActionType act, int button)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetMouseButton(button);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetMouseButtonDown(button);
                case JCS_KeyActionType.KEY_UP:
                    return GetMouseButtonUp(button);
            }

            JCS_GameErrors.JcsErrors(
                "JCS_Input",
                 
                "This cannot happed.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonDown(JCS_MouseButton button)
        {
            return Input.GetMouseButtonDown((int)button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonDown(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButtonDown(button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButton(JCS_MouseButton button)
        {
            return Input.GetMouseButton((int)button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButton(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButton(button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonUp(JCS_MouseButton button)
        {
            return Input.GetMouseButtonUp((int)button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonUp(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButtonUp(button);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyByAction(JCS_KeyActionType act, KeyCode key)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetKey(key);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetKeyDown(key);
                case JCS_KeyActionType.KEY_UP:
                    return GetKeyUp(key);
            }

            JCS_GameErrors.JcsErrors(
                "JCS_Input",
                 
                "This cannot happed.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyDown(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyDown(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKey(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyUp(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonByAction(JCS_KeyActionType act, string buttonName)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetButton(buttonName);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetButtonDown(buttonName);
                case JCS_KeyActionType.KEY_UP:
                    return GetButtonUp(buttonName);
            }


            JCS_GameErrors.JcsErrors(
                "JCS_Input",
                 
                "This cannot happed.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButton(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButton(buttonName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonDown(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButtonDown(buttonName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonUp(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButtonUp(buttonName);
        }

    }
}
