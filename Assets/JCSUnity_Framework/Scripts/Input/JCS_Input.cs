/**
 * $File: JCS_Input.cs $
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
    public class JCS_Input : MonoBehaviour
    {
        private static bool mClick = false;
        private static float mClickTime = 0.25f;
        private static float mClickTimer = 0;
        public static bool OnMouseDoubleClick(JCS_InputType type)
        {
            return OnMouseDoubleClick((int)type);
        }
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



        private static Vector3 mMouseDeltaPosition = Vector3.zero;
        private static Vector3 mLasDeltaPosition = Vector3.zero;

        public static Vector3 MouseDeltaPosition()
        {
            Vector3 currentPosition = Input.mousePosition;
            mMouseDeltaPosition = currentPosition - mLasDeltaPosition;
            mLasDeltaPosition = currentPosition;

            return mMouseDeltaPosition;
        }


        public static bool GetMouseButtonDown(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetMouseButtonDown(button);
        }
        public static bool GetMouseButton(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetMouseButton(button);
        }
        public static bool GetMouseButtonUp(int button)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetMouseButtonUp(button);
        }

        public static bool GetKeyDown(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyDown(key);
        }
        public static bool GetKey(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKey(key);
        }
        public static bool GetKeyUp(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyUp(key);
        }
        public static bool GetButton(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButton(buttonName);
        }
        public static bool GetButtonDown(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButtonDown(buttonName);
        }
        public static bool GetButtonUp(string buttonName)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetButtonUp(buttonName);
        }

    }
}
