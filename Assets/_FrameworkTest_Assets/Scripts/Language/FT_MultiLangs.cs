/**
 * $File: FT_MultiLangs.cs $
 * $Date: 2021-02-16 01:33:57 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JCSUnity;

/// <summary>
/// This test contains the following functionalities.
///
///   1. At least two buttons for different languages.
///   2. Assign callback and call language refresh.
/// </summary>
public class FT_MultiLangs 
    : MonoBehaviour 
{
    /* Variables */

    [Tooltip("")]
    [SerializeField]
    private Button mBtn1 = null;

    [Tooltip("")]
    [SerializeField]
    private SystemLanguage mLang1 = SystemLanguage.Unknown;

    [Tooltip("")]
    [SerializeField]
    private Button mBtn2 = null;

    [Tooltip("")]
    [SerializeField]
    private SystemLanguage mLang2 = SystemLanguage.Unknown;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        this.mBtn1.onClick.AddListener(Btn1_Click);
        this.mBtn2.onClick.AddListener(Btn2_Click);
    }

    private void Btn1_Click()
    {
        JCS_ApplicationManager.instance.systemLanguage = mLang1;
    }

    private void Btn2_Click()
    {
        JCS_ApplicationManager.instance.systemLanguage = mLang2;
    }
}
