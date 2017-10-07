/**
 * $File: JCS_InputSettings.cs $
 * $Date: 2016-10-15 19:37:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{

    /// <summary>
    /// Handle cross platform input settings.
    /// 
    /// URL(jenchieh): http://wiki.unity3d.com/index.php?title=Xbox360Controller
    /// </summary>
    public class JCS_InputSettings
        : JCS_Settings<JCS_InputSettings>
    {

        //----------------------
        // Public Variables
        public const string HOME_BUTTON = "jcsunity button home";
        public const string JOYSTICK_BUTTON_START = "jcsunity button start";
        public const string JOYSTICK_BUTTON_BACK = "jcsunity button back";
        public const string JOYSTICK_BUTTON_A = "jcsunity button a";
        public const string JOYSTICK_BUTTON_B = "jcsunity button b";
        public const string JOYSTICK_BUTTON_X = "jcsunity button x";
        public const string JOYSTICK_BUTTON_Y = "jcsunity button y";
        public const string JOYSTICK_BUTTON_UP = "jcsunity button up";
        public const string JOYSTICK_BUTTON_DOWN = "jcsunity button down";
        public const string JOYSTICK_BUTTON_RIGHT = "jcsunity button right";
        public const string JOYSTICK_BUTTON_LEFT = "jcsunity button left";

        public const string STICK_RIGHT_X = "jcsunity stick right x";
        public const string STICK_RIGHT_Y = "jcsunity stick right y";

        public const string STICK_LEFT_X = "jcsunity stick left x";
        public const string STICK_LEFT_Y = "jcsunity stick left y";

        public const string JOYSTICK_BUTTON_RT = "jcsunity button right trigger";
        public const string JOYSTICK_BUTTON_LT = "jcsunity button left trigger";

        public const string JOYSTICK_BUTTON_LB = "jcsunity button left bumper";
        public const string JOYSTICK_BUTTON_RB = "jcsunity button right bumper";

        public const float DEFAULT_SENSITIVITY = 1.0f;
        public const float DEFAULT_DEAD = 0.2f;

        //----------------------
        // Private Variables

        /// <summary>
        /// Map of the joystick.
        /// </summary>
        [Serializable]
        public struct JoystickMap
        {
            [Header("** Check Varaibles (JoystickMap) **")]

            [Tooltip("")]
            public float StickRightXVal;

            [Tooltip("")]
            public float StickRightYVal;

            [Tooltip("")]
            public float StickLeftXVal;

            [Tooltip("")]
            public float StickLeftYVal;


            [Header("** Initialize Varaibles (JoystickMap) **")]

            #region Button

            [Tooltip("Home button.")]
            public string HomeButton;

            [Tooltip("")]
            public string JoystickButtonStart;

            [Tooltip("")]
            public string JoystickButtonBack;

            [Tooltip("Joystick button A")]
            public string JoystickButtonA;

            [Tooltip("Joystick button B")]
            public string JoystickButtonB;

            [Tooltip("Joystick button X")]
            public string JoystickButtonX;

            [Tooltip("Joystick button Y")]
            public string JoystickButtonY;

            [Tooltip("")]
            public string JoystickButtonUp;

            [Tooltip("")]
            public string JoystickButtonDown;

            [Tooltip("")]
            public string JoystickButtonRight;

            [Tooltip("")]
            public string JoystickButtonLeft;

            #endregion

            #region Stick
            [Header("- Stick")]

            [Tooltip("Stick on the right")]
            public string StickRightX;

            [Tooltip("Stick on the right")]
            public string StickRightY;

            [Tooltip("Stick on the left")]
            public string StickLeftX;

            [Tooltip("Stick on the left")]
            public string StickLeftY;

            #endregion

            #region Trigger

            [Header("- Trigger")]

            [Tooltip("")]
            public string JoystickButtonRT;

            [Tooltip("")]
            public string JoystickButtonLT;

            #endregion

            #region Bumper

            [Header("- Bumper")]

            [Tooltip("")]
            public string JoystickButtonLB;

            [Tooltip("")]
            public string JoystickButtonRB;

            #endregion

        };


        [Header("** Initialize Varaibles (JCS_InputSettings) **")]

        [Tooltip("Targeting game pad going to use in the game.")]
        [SerializeField]
        private JCS_GamePadType mTargetGamePad = JCS_GamePadType.XBOX_360;

        [Tooltip("How many joystick in the game? Do the mapping for these joysticks.")]
        [SerializeField]
        private JoystickMap[] mJoysticks = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_GamePadType TargetGamePad { get { return this.mTargetGamePad; } }
        public JoystickMap[] Joysticks { get { return this.mJoysticks; } set { this.mJoysticks = value; } }
        public JoystickMap GetJoysitckMapByIndex(int index)
        {
            return this.mJoysticks[index];
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Update()
        {
            GetJoystickInfo();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Get the joystick button name by joystick button label.
        /// </summary>
        /// <param name="label"> joystick button label </param>
        /// <returns> name of the joystick button. </returns>
        public static string GetJoystickButtonName(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return JOYSTICK_BUTTON_A;
                case JCS_JoystickButton.BUTTON_B: return JOYSTICK_BUTTON_B;
                case JCS_JoystickButton.BUTTON_X: return JOYSTICK_BUTTON_X;
                case JCS_JoystickButton.BUTTON_Y: return JOYSTICK_BUTTON_Y;

                case JCS_JoystickButton.HOME_BUTTON: return HOME_BUTTON;

                case JCS_JoystickButton.START_BUTTON: return JOYSTICK_BUTTON_START;
                case JCS_JoystickButton.BACK_BUTTON: return JOYSTICK_BUTTON_BACK;

                case JCS_JoystickButton.LEFT_TRIGGER: return JOYSTICK_BUTTON_LT;
                case JCS_JoystickButton.RIGHT_TRIGGER: return JOYSTICK_BUTTON_RT;

                case JCS_JoystickButton.LEFT_BUMPER: return JOYSTICK_BUTTON_LB;
                case JCS_JoystickButton.RIGHT_BUMPER: return JOYSTICK_BUTTON_RB;

                case JCS_JoystickButton.BUTTON_UP: return JOYSTICK_BUTTON_UP;
                case JCS_JoystickButton.BUTTON_DOWN: return JOYSTICK_BUTTON_DOWN;
                case JCS_JoystickButton.BUTTON_LEFT: return JOYSTICK_BUTTON_LEFT;
                case JCS_JoystickButton.BUTTON_RIGHT: return JOYSTICK_BUTTON_RIGHT;

                case JCS_JoystickButton.STICK_RIGHT_X: return STICK_RIGHT_X;
                case JCS_JoystickButton.STICK_RIGHT_Y: return STICK_RIGHT_Y;

                case JCS_JoystickButton.STICK_LEFT_X: return STICK_LEFT_X;
                case JCS_JoystickButton.STICK_LEFT_Y: return STICK_LEFT_Y;
            }

            JCS_Debug.LogWarning(@"Try to get the name with unknown joystick 
button is not allow...");
            return "";
        }

        /// <summary>
        /// Is the button label a axis joystick value?
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsAxisJoystickButton(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_UP:
                case JCS_JoystickButton.BUTTON_DOWN:
                case JCS_JoystickButton.BUTTON_LEFT:
                case JCS_JoystickButton.BUTTON_RIGHT:

                case JCS_JoystickButton.STICK_LEFT_X:
                case JCS_JoystickButton.STICK_LEFT_Y:

                case JCS_JoystickButton.STICK_RIGHT_X:
                case JCS_JoystickButton.STICK_RIGHT_Y:

                case JCS_JoystickButton.LEFT_TRIGGER:
                case JCS_JoystickButton.RIGHT_TRIGGER:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get Unity's controller naming convention.
        /// </summary>
        /// <param name="label"> label we target to get the naming. </param>
        /// <returns> Naming convention by Unity's InputManager, for getting 
        /// the right device driver id. </returns>
        public static string GetPositiveNameByLabel(JCS_JoystickButton label)
        {
#if (UNITY_STANDALONE_WIN)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 0";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 1";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 2";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 3";

                case JCS_JoystickButton.HOME_BUTTON: return "";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 7";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 6";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 5";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 4";
            }
#elif (UNITY_STANDALONE_LINUX)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 0";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 1";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 2";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 3";

                case JCS_JoystickButton.HOME_BUTTON: return "joystick button 15";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 7";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 6";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 5";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 4";
            }
#elif (UNITY_STANDALONE_OSX)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 16";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 17";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 18";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 19";

                case JCS_JoystickButton.HOME_BUTTON: return "joystick button 15";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 9";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 10";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 14";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 13";
            }
#endif


            return "";
        }

        /// <summary>
        /// Check if any specific button's buffer need to be invert.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsInvert(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.LEFT_TRIGGER:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static JCS_AxisChannel GetAxisChannel(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_UP:
                case JCS_JoystickButton.BUTTON_DOWN:
                    return JCS_AxisChannel.CHANNEL_06;

                case JCS_JoystickButton.BUTTON_RIGHT:
                case JCS_JoystickButton.BUTTON_LEFT:
                    return JCS_AxisChannel.CHANNEL_07;

                case JCS_JoystickButton.STICK_LEFT_X:
                    return JCS_AxisChannel.X_Axis;
                case JCS_JoystickButton.STICK_LEFT_Y:
                    return JCS_AxisChannel.Y_Axis;
                case JCS_JoystickButton.STICK_RIGHT_X:
                    return JCS_AxisChannel.CHANNEL_04;
                case JCS_JoystickButton.STICK_RIGHT_Y:
                    return JCS_AxisChannel.CHANNEL_05;

                case JCS_JoystickButton.LEFT_TRIGGER:  // this need to be invert.
                case JCS_JoystickButton.RIGHT_TRIGGER:
                    return JCS_AxisChannel.CHANNEL_03;
            }

            // default.
            return JCS_AxisChannel.X_Axis;
        }

        /// <summary>
        /// Get the axis type depends on the joystick button label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static JCS_AxisType GetAxisType(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A:
                case JCS_JoystickButton.BUTTON_B:
                case JCS_JoystickButton.BUTTON_X:
                case JCS_JoystickButton.BUTTON_Y:

                case JCS_JoystickButton.HOME_BUTTON:

                case JCS_JoystickButton.START_BUTTON:
                case JCS_JoystickButton.BACK_BUTTON:

                case JCS_JoystickButton.LEFT_BUMPER:
                case JCS_JoystickButton.RIGHT_BUMPER:
                    {
                        return JCS_AxisType.KeyOrMouseButton;
                    }

                case JCS_JoystickButton.BUTTON_UP:
                case JCS_JoystickButton.BUTTON_DOWN:
                case JCS_JoystickButton.BUTTON_LEFT:
                case JCS_JoystickButton.BUTTON_RIGHT:

                case JCS_JoystickButton.STICK_LEFT_X:
                case JCS_JoystickButton.STICK_LEFT_Y:

                case JCS_JoystickButton.STICK_RIGHT_X:
                case JCS_JoystickButton.STICK_RIGHT_Y:

                case JCS_JoystickButton.LEFT_TRIGGER:
                case JCS_JoystickButton.RIGHT_TRIGGER:
                    {
                        return JCS_AxisType.JoystickAxis;
                    }
            }

            return JCS_AxisType.KeyOrMouseButton;
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_InputSettings _old, JCS_InputSettings _new)
        {
            _new.Joysticks = _old.Joysticks;
        }

        //----------------------
        // Private Functions

        /// <summary>
        /// Update the joystick info.
        /// </summary>
        private void GetJoystickInfo()
        {
            // check if any joystick connected.
            if (!JCS_Input.IsJoystickConnected())
                return;

            // 
            for (int index = 0;
                index < mJoysticks.Length;
                ++index)
            {
                JoystickMap joystickMap = GetJoysitckMapByIndex(index);

                // get stick value.
                joystickMap.StickLeftXVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_LEFT_X);
                joystickMap.StickLeftYVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_LEFT_Y);

                joystickMap.StickRightXVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_RIGHT_X);
                joystickMap.StickRightYVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_RIGHT_Y);
            }
        }

    }
}
