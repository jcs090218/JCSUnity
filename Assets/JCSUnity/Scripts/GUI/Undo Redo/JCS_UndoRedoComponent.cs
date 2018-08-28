/**
 * $File: JCS_UndoRedoComponent.cs $
 * $Date: 2018-08-25 21:30:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JCSUnity
{
    /// <summary>
    /// Undo Redo system component.
    /// </summary>
    public class JCS_UndoRedoComponent
        : JCS_GUIObject
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_UndoRedoComponent) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Undo key.")]
        [SerializeField]
        private KeyCode mUndoKey = KeyCode.A;

        [Tooltip("Redo key.")]
        [SerializeField]
        private KeyCode mRedoKey = KeyCode.S;
#endif


        [Header("** Check Variables (JCS_UndoRedoComponent) **")]

        [Tooltip("Ignore record now.")]
        [SerializeField]
        private bool mIgnoreRecord = false;


        [Header("** Runtime Variables (JCS_UndoRedoComponent) **")]

        [Tooltip("Undo redo system, if not filled will be use the " +
            "universal undo redo system instead.")]
        [SerializeField]
        private JCS_UndoRedoSystem mUndoRedoSystem = null;


        [Header("- Input Field (JCS_UndoRedoComponent)")]

        [Tooltip("Record down the previous input field data.")]
        [SerializeField]
        private JCS_InputFieldData mPrevInputFieldData = null;

        [Tooltip("Record down all the input field values.")]
        [SerializeField]
        private List<JCS_InputFieldData> mIF_Undo = null;

        [Tooltip("Record down all the input field values.")]
        [SerializeField]
        private List<JCS_InputFieldData> mIF_Redo = null;


        [Header("- Dropdown (JCS_UndoRedoComponent)")]

        [Tooltip("Record down the previous dropdown data.")]
        [SerializeField]
        private JCS_DropdownData mPrevDropdownData = null;

        [Tooltip("Record down all the dropdown values.")]
        [SerializeField]
        private List<JCS_DropdownData> mDD_Undo = null;

        [Tooltip("Record down all the dropdown values.")]
        [SerializeField]
        private List<JCS_DropdownData> mDD_Redo = null;


        [Header("- Toggle (JCS_UndoRedoComponent)")]

        [Tooltip("Record down the previous toggle data.")]
        [SerializeField]
        private JCS_ToggleData mPrevToggleData = null;

        [Tooltip("Record down all the toggle values.")]
        [SerializeField]
        private List<JCS_ToggleData> mTog_Undo = null;

        [Tooltip("Record down all the toggle values.")]
        [SerializeField]
        private List<JCS_ToggleData> mTog_Redo = null;


        [Header("- Scroll Bar (JCS_UndoRedoComponent)")]

        [Tooltip("Record down the previous scroll bar data.")]
        [SerializeField]
        private JCS_ScrollbarData mPrevScrollbarData = null;

        [Tooltip("Record down all the scroll bar values.")]
        [SerializeField]
        private List<JCS_ScrollbarData> mSB_Undo = null;

        [Tooltip("Record down all the scroll bar values.")]
        [SerializeField]
        private List<JCS_ScrollbarData> mSB_Redo = null;


        [Header("- Slider (JCS_UndoRedoComponent)")]

        [Tooltip("Record down the previous slider data.")]
        [SerializeField]
        private JCS_SliderData mPrevSliderData = null;

        [Tooltip("Record down all the slider values.")]
        [SerializeField]
        private List<JCS_SliderData> mSli_Undo = null;

        [Tooltip("Record down all the slider values.")]
        [SerializeField]
        private List<JCS_SliderData> mSli_Redo = null;



        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public JCS_UndoRedoSystem UndoRedoSystem { get { return this.mUndoRedoSystem; } set { this.mUndoRedoSystem = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        protected override void Awake()
        {
            base.Awake();

            // Use the universal one if not filled.
            if (mUndoRedoSystem == null)
                this.mUndoRedoSystem = JCS_UIManager.instance.GetGlobalUndoRedoSystem();

            // Add to get manage by the system.
            this.mUndoRedoSystem.AddUndoRedoComponentToSystem(this);

            // Register it, note we need to register as soon as possible 
            // so we don't miss any default setting record by the script.
            RegisterUndoEvent();
        }

        private void Start()
        {
            // Record down the previous data. This we try 
            // to be as late as possible. Cuz any script change 
            // the UI value after this will not be record...
            RecordPrevData();
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mUndoKey))
                Undo();

            if (JCS_Input.GetKeyDown(mRedoKey))
                Redo();
        }
#endif

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Undo this component.
        /// </summary>
        public void Undo()
        {
            // What ever we do in undo/redo, we don't
            // record anything down.
            mIgnoreRecord = true;

            bool sameData = false;

            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        JCS_InputFieldData ifd = JCS_Utility.ListPopBack(mIF_Undo);

                        if (ifd == null)
                            break;

                        sameData = CheckSameData(ifd);

                        PasteData(ifd);

                        mIF_Redo.Add(ifd);
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        JCS_DropdownData ddd = JCS_Utility.ListPopBack(mDD_Undo);

                        if (ddd == null)
                            break;

                        sameData = CheckSameData(ddd);

                        PasteData(ddd);

                        mDD_Redo.Add(ddd);
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        JCS_SliderData sd = JCS_Utility.ListPopBack(mSli_Undo);

                        if (sd == null)
                            break;

                        sameData = CheckSameData(sd);

                        PasteData(sd);

                        mSli_Redo.Add(sd);
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        JCS_ScrollbarData sbd = JCS_Utility.ListPopBack(mSB_Undo);

                        if (sbd == null)
                            break;

                        sameData = CheckSameData(sbd);

                        PasteData(sbd);

                        mSB_Redo.Add(sbd);
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        JCS_ToggleData td = JCS_Utility.ListPopBack(mTog_Undo);

                        if (td == null)
                            break;

                        sameData = CheckSameData(td);

                        PasteData(td);

                        mTog_Redo.Add(td);
                    }
                    break;
            }

            if (sameData)
                Undo();

            RecordPrevData();

            // Unlock ignore record.
            mIgnoreRecord = false;
        }

        /// <summary>
        /// Redo this compnent.
        /// </summary>
        public void Redo()
        {
            // What ever we do in undo/redo, we don't
            // record anything down.
            mIgnoreRecord = true;

            bool sameData = false;

            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        JCS_InputFieldData ifd = JCS_Utility.ListPopBack(mIF_Redo);

                        if (ifd == null)
                            break;

                        sameData = CheckSameData(ifd);

                        PasteData(ifd);

                        mIF_Undo.Add(ifd);
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        JCS_DropdownData ddd = JCS_Utility.ListPopBack(mDD_Redo);

                        if (ddd == null)
                            break;

                        sameData = CheckSameData(ddd);

                        PasteData(ddd);

                        mDD_Undo.Add(ddd);
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        JCS_SliderData sd = JCS_Utility.ListPopBack(mSli_Redo);

                        if (sd == null)
                            break;

                        sameData = CheckSameData(sd);

                        PasteData(sd);

                        mSli_Undo.Add(sd);
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        JCS_ScrollbarData sbd = JCS_Utility.ListPopBack(mSB_Redo);

                        if (sbd == null)
                            break;

                        sameData = CheckSameData(sbd);

                        PasteData(sbd);

                        mSB_Undo.Add(sbd);
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        JCS_ToggleData td = JCS_Utility.ListPopBack(mTog_Redo);

                        if (td == null)
                            break;

                        sameData = CheckSameData(td);

                        PasteData(td);

                        mTog_Undo.Add(td);
                    }
                    break;
            }

            if (sameData)
                Redo();

            RecordPrevData();

            // Unlock ignore record.
            mIgnoreRecord = false;
        }

        /// <summary>
        /// Clear all the redo list.
        /// </summary>
        public void ClearAllRedo()
        {
            mDD_Redo.Clear();
            mIF_Redo.Clear();
            mSli_Redo.Clear();
            mSB_Redo.Clear();
            mTog_Redo.Clear();
        }

        /// <summary>
        /// Record down the previous data before we do 
        /// undo/redo action.
        /// </summary>
        public void RecordPrevData()
        {
            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        mPrevInputFieldData = new JCS_InputFieldData
                        {
                            text = mInputField.text
                        };
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        mPrevDropdownData = new JCS_DropdownData
                        {
                            value = mDropdown.value
                        };
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        mPrevSliderData = new JCS_SliderData
                        {
                            value = mSlider.value
                        };
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        mPrevScrollbarData = new JCS_ScrollbarData
                        {
                            value = mScrollBar.value
                        };
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        bool tmpIsOn = false;

                        if (mToggle != null)
                            tmpIsOn = mToggle.isOn;
                        else if (mJCSToggle != null)
                            tmpIsOn = mJCSToggle.IsOn;

                        mPrevToggleData = new JCS_ToggleData
                        {
                            isOn = tmpIsOn
                        };
                    }
                    break;
            }
        }

        /// <summary>
        /// Stop recording undo/redo.
        /// </summary>
        public void StopRecording()
        {
            this.mIgnoreRecord = true;
        }

        /// <summary>
        /// Start recording undo/redo.
        /// </summary>
        public void StartRecording()
        {
            this.mIgnoreRecord = false;
        }

        /// <summary>
        /// Is current component recording undo/redo action?
        /// </summary>
        /// <returns>
        /// true, is recording.
        /// false, not recording.
        /// </returns>
        public bool IsRecording()
        {
            return (!this.mIgnoreRecord);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Register the undo event base on different GUI type.
        /// </summary>
        private void RegisterUndoEvent()
        {
            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        mInputField.onEndEdit.AddListener(delegate
                        {
                            RecordOnce();
                        });
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        mDropdown.onValueChanged.AddListener(delegate
                        {
                            RecordOnce();
                        });
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        mSlider.onValueChanged.AddListener(delegate
                        {
                            RecordOnce();
                        });
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        mScrollBar.onValueChanged.AddListener(delegate
                        {
                            RecordOnce();
                        });
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        if (mToggle != null)
                        {
                            mToggle.onValueChanged.AddListener(delegate
                            {
                                RecordOnce();
                            });
                        }
                        else if (mJCSToggle != null)
                        {
                            mJCSToggle.onValueChanged += RecordOnce;
                        }
                        else
                        {
                            JCS_Debug.LogError("Cannot record toggle object " +
                                "without any toggle object component attached..");
                        }
                    }
                    break;
            }
        }

        /// <summary>R
        /// Record down the GUI component once.
        /// </summary>
        private void RecordOnce()
        {
            if (mIgnoreRecord)
                return;

            mUndoRedoSystem.ClearRedoComp();
            ClearAllRedo();

            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        mIF_Undo.Add(mPrevInputFieldData);


                        JCS_InputFieldData ifd = new JCS_InputFieldData
                        {
                            text = mInputField.text
                        };

                        mPrevInputFieldData = ifd;

                        mIF_Redo.Add(ifd);
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        mDD_Undo.Add(mPrevDropdownData);


                        JCS_DropdownData ddd = new JCS_DropdownData
                        {
                            value = mDropdown.value
                        };

                        mPrevDropdownData = ddd;

                        mDD_Redo.Add(ddd);
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        mSli_Undo.Add(mPrevSliderData);


                        JCS_SliderData sd = new JCS_SliderData
                        {
                            value = mSlider.value
                        };

                        mPrevSliderData = sd;

                        mSli_Redo.Add(sd);
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        mSB_Undo.Add(mPrevScrollbarData);


                        JCS_ScrollbarData sbd = new JCS_ScrollbarData
                        {
                            value = mScrollBar.value
                        };

                        mPrevScrollbarData = sbd;

                        mSB_Redo.Add(sbd);
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        mTog_Undo.Add(mPrevToggleData);


                        JCS_ToggleData td = new JCS_ToggleData();

                        if (mToggle != null)
                            td.isOn = mToggle.isOn;
                        else if (mJCSToggle != null)
                            td.isOn = mJCSToggle.IsOn;
                        else
                        {
                            JCS_Debug.LogError("Cannot record toggle object " +
                                "without any toggle object component attached..");
                        }

                        mPrevToggleData = td;

                        mTog_Redo.Add(td);
                    }
                    break;
            }

            // Add it to next undo component.
            mUndoRedoSystem.AddUndoComponent(this);
        }

        /// <summary>
        /// Paste the data.
        /// </summary>
        private void PasteData(JCS_InputFieldData ifd)
        {
            mInputField.text = ifd.text;
        }
        /// <summary>
        /// Paste the data.
        /// </summary>
        /// <param name="ddd"></param>
        private void PasteData(JCS_DropdownData ddd)
        {
            mDropdown.value = ddd.value;
        }
        /// <summary>
        /// Paste the data.
        /// </summary>
        /// <param name="sbd"></param>
        private void PasteData(JCS_ScrollbarData sbd)
        {
            mScrollBar.value = sbd.value;
        }
        /// <summary>
        /// Paste the data.
        /// </summary>
        /// <param name="sd"></param>
        private void PasteData(JCS_SliderData sd)
        {
            mSlider.value = sd.value;
        }
        /// <summary>
        /// Paste the data.
        /// </summary>
        /// <param name="td"></param>
        private void PasteData(JCS_ToggleData td)
        {
            if (mToggle != null)
                mToggle.isOn = td.isOn;
            else if (mJCSToggle != null)
                mJCSToggle.IsOn = td.isOn;
        }

        /// <summary>
        /// Check if the data are the same?
        /// </summary>
        /// <param name="ifd"></param>
        /// <returns>
        /// true, same data.
        /// false, not same data.
        /// </returns>
        private bool CheckSameData(JCS_InputFieldData ifd)
        {
            return (mInputField.text == ifd.text);
        }
        /// <summary>
        /// Check if the data are the same?
        /// </summary>
        /// <param name="ddd"></param>
        /// <returns>
        /// true, same data.
        /// false, not same data.
        /// </returns>
        private bool CheckSameData(JCS_DropdownData ddd)
        {
            return (mDropdown.value == ddd.value);
        }
        /// <summary>
        /// Check if the data are the same?
        /// </summary>
        /// <param name="sbd"></param>
        /// <returns>
        /// true, same data.
        /// false, not same data.
        /// </returns>
        private bool CheckSameData(JCS_ScrollbarData sbd)
        {
            return (mScrollBar.value == sbd.value);
        }
        /// <summary>
        /// Check if the data are the same?
        /// </summary>
        /// <param name="sd"></param>
        /// <returns>
        /// true, same data.
        /// false, not same data.
        /// </returns>
        private bool CheckSameData(JCS_SliderData sd)
        {
            return (mSlider.value == sd.value);
        }
        /// <summary>
        /// Check if the data are the same?
        /// </summary>
        /// <param name="td"></param>
        /// <returns>
        /// true, same data.
        /// false, not same data.
        /// </returns>
        private bool CheckSameData(JCS_ToggleData td)
        {
            if (mToggle != null)
                return (mToggle.isOn == td.isOn);
            else if (mJCSToggle != null)
                return (mJCSToggle.IsOn == td.isOn);

            return false;
        }
    }
}
