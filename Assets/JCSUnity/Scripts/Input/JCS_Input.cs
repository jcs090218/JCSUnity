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
    /// Joystick enum design here...
    /// </summary>
    public enum JCS_JoystickButton
    {
        NONE,

        BUTTON_A,
        BUTTON_B,
        BUTTON_X,
        BUTTON_Y,

        HOME_BUTTON,

        START_BUTTON,
        BACK_BUTTON,

        LEFT_TRIGGER,
        RIGHT_TRIGGER,

        LEFT_BUMPER,
        RIGHT_BUMPER,

        BUTTON_UP,
        BUTTON_DOWN,
        BUTTON_LEFT,
        BUTTON_RIGHT,

        // temp.
        STICK_RIGHT_X,
        STICK_RIGHT_Y,

        STICK_LEFT_X,
        STICK_LEFT_Y,
    };

    /// <summary>
    /// Support joystick up to 30 people.
    /// </summary>
    public enum JCS_JoystickIndex
    {
        // Joystick 0 - 29 (1 ~ 30)
        JOYSTICK_00 = 0x00,
        JOYSTICK_01 = 0x01,
        JOYSTICK_02 = 0x02,
        JOYSTICK_03 = 0x03,
        JOYSTICK_04 = 0x04,
        JOYSTICK_05 = 0x05,
        JOYSTICK_06 = 0x06,
        JOYSTICK_07 = 0x07,
        JOYSTICK_08 = 0x08,
        JOYSTICK_09 = 0x09,

        JOYSTICK_10 = 0x0A,
        JOYSTICK_11 = 0x0B,
        JOYSTICK_12 = 0x0C,
        JOYSTICK_13 = 0x0D,
        JOYSTICK_14 = 0x0E,
        JOYSTICK_15 = 0x0F,
        JOYSTICK_16 = 0x10,
        JOYSTICK_17 = 0x11,
        JOYSTICK_18 = 0x12,
        JOYSTICK_19 = 0x13,

        JOYSTICK_20 = 0x14,
        JOYSTICK_21 = 0x15,
        JOYSTICK_22 = 0x16,
        JOYSTICK_23 = 0x17,
        JOYSTICK_24 = 0x18,
        JOYSTICK_25 = 0x19,
        JOYSTICK_26 = 0x1A,
        JOYSTICK_27 = 0x1B,
        JOYSTICK_28 = 0x1C,
        JOYSTICK_29 = 0x1D,
    };

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

            JCS_Debug.LogError(
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

            JCS_Debug.LogError("This cannot happed, because all states applied.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// Is the key down?
        /// </summary>
        /// <param name="key"> key to check if the key is down. </param>
        /// <returns>
        /// true: key is down, false: vice versa.
        /// </returns>
        public static bool GetKeyDown(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyDown(key);
        }
        /// <summary>
        /// Is the key held down?
        /// </summary>
        /// <param name="key"> key to check if the key is held down. </param>
        /// <returns>
        /// true: key is held down, false: vice versa.
        /// </returns>
        public static bool GetKey(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKey(key);
        }
        /// <summary>
        /// Is the key up?
        /// </summary>
        /// <param name="key"> key to check if is key up. </param>
        /// <returns> true: is key up, false: vice versa. </returns>
        public static bool GetKeyUp(KeyCode key)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// Get the boolean check depends on the button status.
        /// 
        /// Button status are list here:
        /// 1) Button Down
        /// 2) Button Hold
        /// 3) Button Up
        /// </summary>
        /// <param name="act"> button down, button up, or button pressed. </param>
        /// <param name="buttonName"> name of the button. </param>
        /// <returns> 
        /// Is either pressed, down, up. or not pressed, down, up. 
        /// </returns>
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


            JCS_Debug.LogError(
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

        /// <summary>
        /// Loop through the key and key what key is pressed.
        /// </summary>
        /// <param name="type"> type of the key pressed option. </param>
        /// <returns> key u pressed. </returns>
        public static KeyCode GetAnyKeyByAction(JCS_KeyActionType type)
        {
            // loop through the key code list
            foreach (KeyCode val in JCS_Utility.GetValues<KeyCode>())
            {
                // if the key is pressed, return it.
                if (GetKeyByAction(type, val))
                {
                    return val;
                }
            }

            // default return.
            return KeyCode.None;
        }

        /// <summary>
        /// Check if any key is pressed.
        /// </summary>
        /// <param name="type"> action u want to check. </param>
        /// <returns>
        /// true: key buffer in
        /// false: no key buffer in
        /// </returns>
        public static bool IsAnyKeyBuffer(JCS_KeyActionType type)
        {
            KeyCode code = GetAnyKeyByAction(type);

            return (code != KeyCode.None);
        }

        /// <summary>
        /// Check if any key is down?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is down.
        /// false: no key is down.
        /// </returns>
        public static bool GetAnyKeyDown()
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_DOWN);
        }

        /// <summary>
        /// Check if any key is pressed.
        /// </summary>
        /// <returns>
        /// true: somewhere the key is pressed.
        /// false: no key is pressed.
        /// </returns>
        public static bool GetAnyKey()
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY);
        }

        /// <summary>
        /// Check if any key is up?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is up.
        /// false: no key is up.
        /// </returns>
        public static bool GetAnyKeyUp()
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return false;

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_UP);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn)
        {
            return GetAxis((int)joystickIndex, btn);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(int joystickIndex, JCS_JoystickButton btn)
        {
            string num = "";

            // get the joystick
            JCS_InputSettings.JoystickMap joystickMap = JCS_InputSettings.instance.GetJoysitckMapByIndex(joystickIndex);

            switch (btn)
            {
                case JCS_JoystickButton.HOME_BUTTON:
                    num = joystickMap.HomeButton;
                    break;

                case JCS_JoystickButton.START_BUTTON:
                    num = joystickMap.JoystickButtonStart;
                    break;
                case JCS_JoystickButton.BACK_BUTTON:
                    num = joystickMap.JoystickButtonBack;
                    break;

                case JCS_JoystickButton.BUTTON_A:
                    num = joystickMap.JoystickButtonA;
                    break;
                case JCS_JoystickButton.BUTTON_B:
                    num = joystickMap.JoystickButtonB;
                    break;
                case JCS_JoystickButton.BUTTON_X:
                    num = joystickMap.JoystickButtonX;
                    break;
                case JCS_JoystickButton.BUTTON_Y:
                    num = joystickMap.JoystickButtonY;
                    break;

                case JCS_JoystickButton.BUTTON_UP:
                    num = joystickMap.JoystickButtonUp;
                    break;
                case JCS_JoystickButton.BUTTON_DOWN:
                    num = joystickMap.JoystickButtonDown;
                    break;
                case JCS_JoystickButton.BUTTON_LEFT:
                    num = joystickMap.JoystickButtonLeft;
                    break;
                case JCS_JoystickButton.BUTTON_RIGHT:
                    num = joystickMap.JoystickButtonRight;
                    break;

                    // Stick Right
                case JCS_JoystickButton.STICK_RIGHT_X:
                    num = joystickMap.StickRightX;
                    break;
                case JCS_JoystickButton.STICK_RIGHT_Y:
                    num = joystickMap.StickRightY;
                    break;

                // Stick Left
                case JCS_JoystickButton.STICK_LEFT_X:
                    num = joystickMap.StickLeftX;
                    break;
                case JCS_JoystickButton.STICK_LEFT_Y:
                    num = joystickMap.StickLeftY;
                    break;

                    // Bumper
                case JCS_JoystickButton.RIGHT_BUMPER:
                    num = joystickMap.JoystickButtonRB;
                    break;
                case JCS_JoystickButton.LEFT_BUMPER:
                    num = joystickMap.JoystickButtonLB;
                    break;

                    // Trigger
                case JCS_JoystickButton.RIGHT_TRIGGER:
                    num = joystickMap.JoystickButtonRT;
                    break;
                case JCS_JoystickButton.LEFT_TRIGGER:
                    num = joystickMap.JoystickButtonLT;
                    break;
            }

            return GetAxis(num);
        }
        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="name"> name of the joystick name. </param>
        /// <returns> value the joystick tilt. </returns>
        public static float GetAxis(string name)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return 0;

            if (name == "")
            {
                JCS_Debug.LogError(
                    "name cannot be empty string...");

                return 0;
            }

            return Input.GetAxis(name);
        }

        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="btn"></param>
        /// <returns> buffer pressure from hardware. </returns>
        public static bool GetJoystickButton(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn)
        {
            return GetJoystickButton((int)joystickIndex, btn);
        }
        /// <summary>
        /// Get the joystick buffer.
        /// </summary>
        /// <returns></returns>
        public static bool GetJoystickButton(int joystickIndex, JCS_JoystickButton btn)
        {
            // check if any joystick connected.
            if (!IsJoystickConnected())
            {
                JCS_Debug.LogError(
                    @"There are no joystick connected, and u 
still trying to make the joystick specific function call.");

                return false;
            }

            bool reverse = false;

            // these trigger have to be reserve.
            switch (btn)
            {
                case JCS_JoystickButton.BUTTON_LEFT:
                case JCS_JoystickButton.BUTTON_DOWN:
                    reverse = true;
                    break;
            }

            if (reverse)
                return (GetAxis(joystickIndex, btn) < 0);

            return (GetAxis(joystickIndex, btn) > 0);
        }

        /// <summary>
        /// Check if there is joy stick connect to 
        /// the pc.
        /// 
        /// SOURCE(jenchieh): http://answers.unity3d.com/questions/218477/how-can-i-detect-if-a-gamepad-is-present.html
        /// </summary>
        /// <returns>
        /// true: is connected
        /// false: nothing is detected.
        /// </returns>
        public static bool IsJoystickConnected()
        {
            return (Input.GetJoystickNames().Length != 0);
        }

    }
}
