/**
 * $File: JCS_SelectLanguageButton.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button to select system langauge.
    /// </summary>
    public class JCS_SelectLanguageButton : JCS_Button
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_SelectLanguageButton)")]

        [Tooltip("URL to open in the default browser.")]
        [SerializeField]
        private SystemLanguage mSystemLanguage = SystemLanguage.Unknown;

        /* Setter & Getter */

        public SystemLanguage systemLanguage { get { return this.mSystemLanguage; } set { this.mSystemLanguage = value; } }

        /* Functions */

        public override void OnClick()
        {
            JCS_AppManager.instance.systemLanguage = mSystemLanguage;
        }
    }
}
