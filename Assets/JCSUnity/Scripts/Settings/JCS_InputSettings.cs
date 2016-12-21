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
    /// 
    /// </summary>
    public class JCS_InputSettings
        : JCS_Settings<JCS_InputSettings>
    {

        //----------------------
        // Public Variables
        public const string HomeButton = "jcsunity button home";
        public const string JoystickButtonStart = "jcsunity button start";
        public const string JoystickButtonBack = "jcsunity button back";
        public const string JoystickButtonA = "jcsunity button a";
        public const string JoystickButtonB = "jcsunity button b";
        public const string JoystickButtonX = "jcsunity button x";
        public const string JoystickButtonY = "jcsunity button y";
        public const string JoystickButtonUp = "jcsunity button up";
        public const string JoystickButtonDown = "jcsunity button down";
        public const string JoystickButtonRight = "jcsunity button right";
        public const string JoystickButtonLeft = "jcsunity button left";

        public const string StickRightX = "jcsunity stick right x";
        public const string StickRightY = "jcsunity stick right y";

        public const string StickLeftX = "jcsunity stick left x";
        public const string StickLeftY = "jcsunity stick left y";

        public const string JoystickButtonRT = "jcsunity button right trigger";
        public const string JoystickButtonLT = "jcsunity button left trigger";

        public const string JoystickButtonLB = "jcsunity button left bumper";
        public const string JoystickButtonRB = "jcsunity button right bumper";


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


            //public JoystickMap(int StickRightX)
            //{

            //}

        };


        [Header("** Initialize Varaibles (JCS_InputSettings) **")]

        [Tooltip("")]
        [SerializeField]
        private JoystickMap[] mJoysticks = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
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
