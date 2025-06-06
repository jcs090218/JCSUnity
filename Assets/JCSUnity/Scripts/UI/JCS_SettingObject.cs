﻿/**
 * $File: JCS_SettingObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Object need to get the information in order to get the 
    /// correct setting
    /// </summary>
    public class JCS_SettingObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_SettingObject)")]

        [Tooltip("Type of the GUI.")]
        [SerializeField]
        private JCS_GUIType mGUIType = JCS_GUIType.NONE;

        // Usually should be only one!
        private float mSettingValue = 0.0f;
        private string mSettingString = "";

        // list of all the type
        private Toggle mToggle = null;
        private Slider mSlider = null;
        private Scrollbar mScrollbar = null;
        private Dropdown mDropdown = null;
        private InputField mInputField = null;

        /* Setter & Getter */

        public string SettingString { get { return this.mSettingString; } }

        /* Functions */

        private void Start()
        {
            // get the corrosponding gui type!!
            InitGUIComponent();
        }

        private void Update()
        {
            switch (mGUIType)
            {
                case JCS_GUIType.NONE:
                    return;
                case JCS_GUIType.TOGGLE:
                    {
                        // ..
                    }
                    break;
                case JCS_GUIType.SLIDER:
                    {
                        // ..
                    }
                    break;
                case JCS_GUIType.SCROLL_BAR:
                    {
                        // ..
                    }
                    break;
                case JCS_GUIType.DROP_DOWN:
                    {
                        // ..
                    }
                    break;
                case JCS_GUIType.INPUT_FIELD:
                    {
                        // ..
                    }
                    break;
            }
        }

        /// <summary>
        /// Initialize GUI component.
        /// </summary>
        private void InitGUIComponent()
        {
            switch (mGUIType)
            {
                case JCS_GUIType.NONE:
                    Debug.LogError("Setting with no meaning...");
                    return;
                case JCS_GUIType.TOGGLE:
                    {
                        this.mToggle = this.GetComponent<Toggle>();
                        if (mToggle == null)
                        {
                            Debug.LogError("Toggle with no corrosdoing component...");
                            return;
                        }
                    }
                    break;
                case JCS_GUIType.SLIDER:
                    {
                        this.mSlider = this.GetComponent<Slider>();
                        if (mSlider == null)
                        {
                            Debug.LogError("Slider with no corrosdoing component...");
                            return;
                        }
                        // assign value;
                        this.mSlider.value = mSettingValue;
                    }
                    break;
                case JCS_GUIType.SCROLL_BAR:
                    {
                        this.mScrollbar = this.GetComponent<Scrollbar>();
                        if (mScrollbar == null)
                        {
                            Debug.LogError("Scrollbar with no corrosdoing component...");
                            return;
                        }
                    }
                    break;
                case JCS_GUIType.DROP_DOWN:
                    {
                        this.mDropdown = this.GetComponent<Dropdown>();
                        if (mDropdown == null)
                        {
                            Debug.LogError("Dropdown with no corrosdoing component...");
                            return;
                        }
                    }
                    break;
                case JCS_GUIType.INPUT_FIELD:
                    {
                        this.mInputField = this.GetComponent<InputField>();
                        if (mInputField == null)
                        {
                            Debug.LogError("InputField with no corrosdoing component...");
                            return;
                        }
                    }
                    break;
            }
        }
    }
}
