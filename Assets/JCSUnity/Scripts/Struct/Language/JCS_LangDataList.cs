/**
 * $File: JCS_LangDataList.cs $
 * $Date: 2021-02-16 00:20:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// List of language data for multiple languages support.
    ///
    /// You should treat this data structure as a multiple languages database.
    /// </summary>
    [System.Serializable]
    public class JCS_LangDataList
    {
        /* Variables */

        [Separator("Initialize (JCS_LangDataList)")]

        [Tooltip("List of language data.")]
        [SerializeField]
        private List<JCS_LangData> mLangData = null;

        /* Setter & Getter */

        public List<JCS_LangData> langData { get { return mLangData; } }

        /* Functions */

    }
}
