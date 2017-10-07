#if (UNITY_EDITOR)
/**
 * $File: JCS_InputController.cs $
 * $Date: 2017-10-07 06:12:14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace JCSUnity
{
    /// <summary>
    /// Control the Unity's 'InputManager' settings.
    /// 
    /// SOURCE(jenchieh): http://plyoung.appspot.com/blog/manipulating-input-manager-in-script.html
    /// </summary>
    public class JCS_InputController
        : MonoBehaviour
    {
        /// <summary>
        /// Get the serialize property from Unity's 'InputManager' 
        /// element structure.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisName"></param>
        /// <returns></returns>
        private static bool AxisDefined(string axisName)
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            axesProperty.Next(true);
            axesProperty.Next(true);
            while (axesProperty.Next(false))
            {
                SerializedProperty axis = axesProperty.Copy();
                axis.Next(true);
                if (axis.stringValue == axisName) return true;
            }
            return false;
        }

        /// <summary>
        /// Input manager's element struct.
        /// </summary>
        public class InputAxis
        {
            public string name;
            public string descriptiveName;
            public string descriptiveNegativeName;
            public string negativeButton;
            public string positiveButton;
            public string altNegativeButton;
            public string altPositiveButton;

            public float gravity;
            public float dead;
            public float sensitivity;

            public bool snap = false;
            public bool invert = false;

            public JCS_AxisType type;

            public int axis;
            public int joyNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        private static void AddAxis(InputAxis axis)
        {
            if (AxisDefined(axis.name)) return;

            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            axesProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// Clear all the input manager settings.
        /// 
        /// ATTENTION(jenchieh): this will clear everything out from
        /// Unity's 'InputManager'. Use it carefully.
        /// </summary>
        private static void ClearInputManagerSettings()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
            axesProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Design the input we want to add here.
        /// </summary>
        public static void SetupInputManager()
        {
            // Add gamepad definitions
            SetupXBox360Joystick();
        }

        public static void SetupXBox360Joystick()
        {
            JCS_InputSettings jcsis = JCS_InputSettings.instance;

            int gamePadCount = jcsis.Joysticks.Length;

            float defalutSenstivity = JCS_InputSettings.DEFAULT_SENSITIVITY;
            float defaultDead = JCS_InputSettings.DEFAULT_DEAD;

            for (int joystickNum = 0; joystickNum < gamePadCount; ++joystickNum)
            {
                foreach (JCS_JoystickButton val in JCS_Utility.GetValues<JCS_JoystickButton>())
                {
                    if (val == JCS_JoystickButton.NONE)
                        continue;

                    // add axis definition.
                    AddAxis(new InputAxis
                    {
                        name = JCS_InputSettings.GetJoystickButtonName(val),
                        positiveButton = JCS_InputSettings.GetPositiveNameByLabel(val),
                        dead = defaultDead,
                        sensitivity = defalutSenstivity,
                        type = JCS_InputSettings.GetAxisType(val),
                        invert = JCS_InputSettings.IsInvert(val),
                        axis = (int)JCS_InputSettings.GetAxisChannel(val),
                        joyNum = joystickNum,
                    });
                }
            }
        }

    }
}
#endif
