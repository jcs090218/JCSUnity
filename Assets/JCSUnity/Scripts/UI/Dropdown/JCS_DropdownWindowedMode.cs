using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEditor;

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the windowed mode.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class JCS_DropdownWindowedMode : MonoBehaviour
    {
        /* Variables */

        private TMP_Dropdown mDropdown = null;

        [Separator("Initialize Variables (JCS_DropdownWindowedMode)")]

        [Tooltip("A list of windowed option to use.")]
        [SerializeField]
        [ReadOnly]
        private List<string> mOptions = new List<string>()
        {
            "Full Screen",
            "Windowed",
        };

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        /* Setter & Getter */

        public List<string> Options { get { return mOptions; } set { this.mOptions = value; } }
        public bool RemoveAllOptions { get { return mRemoveAllOptions; } set { this.mRemoveAllOptions = value; } }

        /* Functions */

        private void Awake()
        {
            this.mDropdown = GetComponent<TMP_Dropdown>();

            Refresh();

            AddListener();
        }

        private void AddListener()
        {
            mDropdown.onValueChanged.AddListener(delegate
            {
                OnValueChanged(mDropdown);
            });

            // Run once.
            OnValueChanged(mDropdown);
        }

        /// <summary>
        /// Refresh all options once.
        /// </summary>
        public void Refresh()
        {
            if (mRemoveAllOptions)
                mDropdown.ClearOptions();

            foreach (string option in mOptions)
            {
                JCS_UIUtil.Dropdown_AddOption(mDropdown, option);
            }

            // Default to the current windowed mode.
            {
                string text = ModeToString(Screen.fullScreenMode);

                JCS_UIUtil.Dropdown_SetSelection(mDropdown, text);
            }
        }

        private void OnValueChanged(TMP_Dropdown dropdown)
        {
            string text = JCS_UIUtil.Dropdown_GetSelectedValue(dropdown);

            FullScreenMode mode = StringToMode(text);

            Resolution res = Screen.currentResolution;

            int width = res.width;
            int height = res.height;

            Screen.SetResolution(width, height, mode);
        }

        private string ModeToString(FullScreenMode mode)
        {
            switch (mode)
            {
                case FullScreenMode.FullScreenWindow:
                    return "Full Screen";
                case FullScreenMode.Windowed:
                    return "Windowed";
            }

            return JCS_UIUtil.Dropdown_GetSelectedValue(mDropdown);
        }

        private FullScreenMode StringToMode(string text)
        {
            switch (text)
            {
                case "Full Screen":
                    return FullScreenMode.FullScreenWindow;
                case "Windowed":
                    return FullScreenMode.Windowed;
            }

            return Screen.fullScreenMode;
        }
    }
}
