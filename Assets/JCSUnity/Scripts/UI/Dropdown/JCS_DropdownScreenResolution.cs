using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the screen resolution.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class JCS_DropdownScreenResolution : MonoBehaviour
    {
        /* Variables */

        private TMP_Dropdown mDropdown = null;

        [Separator("Initialize Variables (JCS_DropdownScreenResolution)")]

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        /* Setter & Getter */

        public bool RemoveAllOptions { get { return mRemoveAllOptions; } set { this.mRemoveAllOptions = value; } }

        /* Functions */

        private void Awake()
        {
            this.mDropdown = GetComponent<TMP_Dropdown>();

            Refresh();

            AddListener();
        }

        private void Start()
        {
            var screens = JCS_ScreenSettings.instance;

            screens.onChangedResolution += Refresh;
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

            foreach (Resolution res in Screen.resolutions.Reverse())
            {
                string text = FormatName(res.width, res.height);

                int index = JCS_UIUtil.Dropdown_GetItemIndex(mDropdown, text);

                // Prevent adding the same options.
                if (index == -1)
                    JCS_UIUtil.Dropdown_AddOption(mDropdown, text);
            }

            // Default to the current screen resolution.
            {
                string res = FormatName(Screen.width, Screen.height);

                JCS_UIUtil.Dropdown_SetSelection(mDropdown, res);
            }
        }

        private void OnValueChanged(TMP_Dropdown dropdown)
        {
            string text = JCS_UIUtil.Dropdown_GetSelectedValue(dropdown);

            string[] data = text.Split("x");

            Resolution res = Screen.currentResolution;

            int width = JCS_Util.Parse(data[0], res.width);
            int height = JCS_Util.Parse(data[1], res.height);

            Screen.SetResolution(width, height, Screen.fullScreenMode);

#if UNITY_EDITOR
            GameViewUtils.AddAndSelectCustomSize(
                GameViewUtils.GameViewSizeType.FixedResolution,
                GameViewSizeGroupType.Standalone, width, height,
                text);
#endif
        }

        private string FormatName(int width, int height)
        {
            return width + "x" + height;
        }
    }
}
