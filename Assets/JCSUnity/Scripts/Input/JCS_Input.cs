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
using System.Collections.Generic;


namespace JCSUnity
{
    /// <summary>
    /// Joystick enum design here...
    /// </summary>
    public enum JCS_JoystickButton
    {
        NONE,

        /* General Gamepad */
        HOME_BUTTON,    // KeyOrMouseButton  (PS)

        START_BUTTON,   // KeyOrMouseButton  (Options)
        BACK_BUTTON,    // KeyOrMouseButton  (Share)

        BUTTON_A,       // KeyOrMouseButton  (Circle)
        BUTTON_B,       // KeyOrMouseButton  (Square)
        BUTTON_X,       // KeyOrMouseButton
        BUTTON_Y,       // KeyOrMouseButton (Triangle)

        LEFT_BUMPER,    // KeyOrMouseButton  (L1)
        RIGHT_BUMPER,   // KeyOrMouseButton  (R1)

        LEFT_TRIGGER,   // JoystickAxis  (L2)
        RIGHT_TRIGGER,  // JoystickAxis  (R2)

        BUTTON_UP,      // JoystickAxis
        BUTTON_DOWN,    // JoystickAxis
        BUTTON_LEFT,    // JoystickAxis
        BUTTON_RIGHT,   // JoystickAxis

        STICK_RIGHT_X,  // JoystickAxis
        STICK_RIGHT_Y,  // JoystickAxis

        STICK_LEFT_X,   // JoystickAxis
        STICK_LEFT_Y,   // JoystickAxis
    };

    /// <summary>
    /// Support joystick up to 30 people.
    /// </summary>
    public enum JCS_JoystickIndex
    {
        // Joystick 0 - 10 (1 ~ 11)
        FROM_ALL_JOYSTICK = 0x00,
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

    public delegate void JoystickPlugged();  // Callback when joystick is plugged.
    public delegate void JoystickUnPlugged();  // Callback when joystick is unplugged.

    /// <summary>
    /// Unity's Input class re-wrapper.
    /// </summary>
    public class JCS_Input 
    {
        private static bool mClick = false;
        private static float mClickTime = 0.25f;
        private static float mClickTimer = 0;

        private static Dictionary<string, bool>
            mJoystickKeyPressed = new Dictionary<string, bool>();
        private static Dictionary<string, bool>
            mJoystickKeyReleased = new Dictionary<string, bool>();

        private static List<string> mJoystickKeyWasPreseed = new List<string>();
        private static List<string> mJoystickKeyWasReleased = new List<string>();

        public static JoystickPlugged joystickPluggedCallback = JoystickPluggedDefaultCallback;
        public static JoystickUnPlugged joystickUnPluggedCallback = JoystickUnPluggedDefaultCallback;

        // record down the if the joystick was connected.
        private static bool mIsJoystickConnected = IsJoystickConnected();


        /* Default callback function pointer. */
        private static void JoystickPluggedDefaultCallback() { JCS_Debug.Log("At least one joystick connected!!!"); }
        private static void JoystickUnPluggedDefaultCallback() { JCS_Debug.Log("No joystick connected..."); }


        /// <summary>
        /// Main loop for input.
        /// </summary>
        public static void LateUpdate()
        {
            DoJoystickCallback();

            ClearJoystickKeymapBuffer();
        }

        /// <summary>
        /// Do one callback, usually for resetting.
        /// </summary>
        public static void InputCallbackOnce()
        {
            if (mIsJoystickConnected)
                joystickPluggedCallback.Invoke();
            else
                joystickUnPluggedCallback.Invoke();
        }

        /// <summary>
        /// Do the joystick callback.
        /// </summary>
        private static void DoJoystickCallback()
        {
            if (mIsJoystickConnected == IsJoystickConnected())
                return;

            mIsJoystickConnected = IsJoystickConnected();

            InputCallbackOnce();
        }

        /// <summary>
        /// Clear the joystick keymap buffer in order to implement
        /// joystick button down and joystick button up.
        /// </summary>
        private static void ClearJoystickKeymapBuffer()
        {
            for (int index = 0;
                index < mJoystickKeyWasPreseed.Count;
                ++index)
            {
                string idString = mJoystickKeyWasPreseed[index];
                mJoystickKeyPressed[idString] = true;
            }

            for (int index = 0;
                index < mJoystickKeyWasReleased.Count;
                ++index)
            {
                string idString = mJoystickKeyWasReleased[index];
                mJoystickKeyReleased[idString] = true;
            }

            // Clear all key action history.
            mJoystickKeyWasPreseed.Clear();
            mJoystickKeyWasReleased.Clear();
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// This sould be in the Update() function.
        /// </summary>
        /// <param name="type"> type by JCS_InputType (self-define) </param>
        /// <returns> true: if double click, false nothing happens </returns>
        public static bool OnMouseDoubleClick(JCS_InputType type, bool ignorePause = false)
        {
            return OnMouseDoubleClick((int)type, ignorePause);
        }
        /// <summary>
        /// This sould be in the Update() function.
        /// </summary>
        /// <param name="button"> type by keycode (Unity built-in) </param>
        /// <returns> true: if double click, false nothing happens </returns>
        public static bool OnMouseDoubleClick(int button, bool ignorePause = false)
        {
            // Check first click
            if (!mClick)
            {
                if (GetMouseButtonDown(button, ignorePause))
                    mClick = true;
            }
            // Check double click
            else
            {
                mClickTimer += Time.deltaTime;

                if (GetMouseButtonDown(button, ignorePause))
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
        /// Get the mouse position on canvas space.
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
        public static bool GetMouseByAction(JCS_KeyActionType act, JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseByAction(act, (int)button, ignorePause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseByAction(JCS_KeyActionType act, int button, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetMouseButton(button, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetMouseButtonDown(button, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetMouseButtonUp(button, ignorePause);
            }

            JCS_Debug.LogError("This cannot happed.");
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonDown(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButtonDown((int)button, ignorePause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonDown(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButtonDown(button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButton(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButton((int)button, ignorePause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButton(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButton(button);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonUp(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButtonUp((int)button, ignorePause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetMouseButtonUp(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

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
        public static bool GetKeyByAction(JCS_KeyActionType act, KeyCode key, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetKey(key, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetKeyDown(key, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetKeyUp(key, ignorePause);
            }

            JCS_Debug.LogError("This cannot happed, because all states applied.");
            return false;
        }
        /// <summary>
        /// Is the key down?
        /// </summary>
        /// <param name="key"> key to check if the key is down. </param>
        /// <returns>
        /// true: key is down, false: vice versa.
        /// </returns>
        public static bool GetKeyDown(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetKeyDown(key);
        }
        /// <summary>
        /// Is the key held down?
        /// </summary>
        /// <param name="key"> key to check if the key is held down. </param>
        /// <returns>
        /// true: key is held down, false: vice versa.
        /// </returns>
        public static bool GetKey(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetKey(key);
        }
        /// <summary>
        /// Is the key up?
        /// </summary>
        /// <param name="key"> key to check if is key up. </param>
        /// <returns> true: is key up, false: vice versa. </returns>
        public static bool GetKeyUp(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

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
        public static bool GetButtonByAction(JCS_KeyActionType act, string buttonName, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetButton(buttonName, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetButtonDown(buttonName, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetButtonUp(buttonName, ignorePause);
            }


            JCS_Debug.LogError("This cannot happed.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButton(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButton(buttonName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonDown(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButtonDown(buttonName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonUp(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButtonUp(buttonName);
        }

        /// <summary>
        /// Loop through the key and key what key is pressed.
        /// </summary>
        /// <param name="type"> type of the key pressed option. </param>
        /// <returns> key u pressed. </returns>
        public static KeyCode GetAnyKeyByAction(JCS_KeyActionType type, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return KeyCode.None;
            }

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
        public static bool IsAnyKeyBuffer(JCS_KeyActionType type, bool ignorePause = false)
        {
            KeyCode code = GetAnyKeyByAction(type, ignorePause);

            return (code != KeyCode.None);
        }

        /// <summary>
        /// Check if any key is down?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is down.
        /// false: no key is down.
        /// </returns>
        public static bool GetAnyKeyDown(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_DOWN);
        }

        /// <summary>
        /// Check if any key is pressed.
        /// </summary>
        /// <returns>
        /// true: somewhere the key is pressed.
        /// false: no key is pressed.
        /// </returns>
        public static bool GetAnyKey(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY);
        }

        /// <summary>
        /// Check if any key is up?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is up.
        /// false: no key is up.
        /// </returns>
        public static bool GetAnyKeyUp(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_UP);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetAxis((int)joystickIndex, btn, ignorePause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (btn == JCS_JoystickButton.NONE)
                return 0;

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            return GetAxis(idString, ignorePause);
        }
        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="name"> name of the joystick name. </param>
        /// <returns> value the joystick tilt. </returns>
        public static float GetAxis(string name, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return 0;
            }

            if (name == "")
            {
                JCS_Debug.LogError("InputManager's name variable cannot be empty string...");
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
        public static bool GetJoystickButton(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickButton((int)joystickIndex, btn, ignorePause);
        }
        /// <summary>
        /// Get the joystick buffer.
        /// </summary>
        /// <returns></returns>
        public static bool GetJoystickButton(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            // check if any joystick connected.
            if (!IsJoystickConnected())
                return false;

            return GetAxis(joystickIndex, btn, ignorePause) > 0;
        }

        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <param name="idString"> string id </param>
        /// <returns></returns>
        public static bool GetJoystickButton(string idString, bool ignorePause = false)
        {
            return GetAxis(idString, ignorePause) > 0;
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKey((int)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return GetJoystickButton(joystickIndex, btn, ignorePause);
        }


        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKeyUp((JCS_JoystickIndex)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            if (GetJoystickKey(joystickIndex, btn, ignorePause))
            {
                if (mJoystickKeyReleased.ContainsKey(idString))
                    mJoystickKeyReleased[idString] = false;
            }
            else
            {
                if (!mJoystickKeyReleased.ContainsKey(idString))
                {
                    mJoystickKeyReleased.Add(idString, true);
                }
                else
                {
                    if (mJoystickKeyReleased[idString])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasReleased.Contains(idString))
                            mJoystickKeyWasReleased.Add(idString);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKeyDown((JCS_JoystickIndex)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            if (GetJoystickKey(joystickIndex, btn, ignorePause))
            {
                if (!mJoystickKeyPressed.ContainsKey(idString))
                {
                    mJoystickKeyPressed.Add(idString, false);
                }
                // Key contains!
                else
                {
                    if (mJoystickKeyPressed[idString])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasPreseed.Contains(idString))
                            mJoystickKeyWasPreseed.Add(idString);
                        return true;
                    }
                }

            }
            else
            {
                if (mJoystickKeyPressed.ContainsKey(idString))
                    mJoystickKeyPressed[idString] = false;
            }

            return false;
        }

        public static bool GetJoystickKeyByAction(
            JCS_KeyActionType act, 
            JCS_JoystickIndex id,
            JCS_JoystickButton key, 
            bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetJoystickKey(id, key, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetJoystickKeyDown(id, key, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetJoystickKeyUp(id, key, ignorePause);
            }

            JCS_Debug.LogError("This cannot happed.");
            return false;
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
            return (!JCS_Utility.IsArrayEmpty(Input.GetJoystickNames()));
        }

    }
}
