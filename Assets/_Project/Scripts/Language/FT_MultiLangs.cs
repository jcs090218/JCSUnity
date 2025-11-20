/**
 * $File: FT_MultiLangs.cs $
 * $Date: 2021-02-16 01:33:57 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using JCSUnity;

/// <summary>
/// This test contains the following functionalities.
///
///   1. At least two buttons for different languages.
///   2. Assign callback and call language refresh.
/// </summary>
public class FT_MultiLangs : MonoBehaviour
{
    /* Variables */

    [Tooltip("Button 1 for language 1.")]
    [SerializeField]
    private Button mBtn1 = null;

    [Tooltip("Language 1 to display after clicking button 1.")]
    [SerializeField]
    private SystemLanguage mLang1 = SystemLanguage.Unknown;

    [Tooltip("Button 2 for language 2.")]
    [SerializeField]
    private Button mBtn2 = null;

    [Tooltip("Language 2 to display after clicking button 2.")]
    [SerializeField]
    private SystemLanguage mLang2 = SystemLanguage.Unknown;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        mBtn1.onClick.AddListener(Btn1_Click);
        mBtn2.onClick.AddListener(Btn2_Click);
    }

    private void Btn1_Click()
    {
        JCS_AppManager.FirstInstance().systemLanguage = mLang1;
    }

    private void Btn2_Click()
    {
        JCS_AppManager.FirstInstance().systemLanguage = mLang2;
    }
}
