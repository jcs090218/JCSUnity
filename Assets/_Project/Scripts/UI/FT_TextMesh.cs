/**
 * $File: FT_TextMesh.cs $
 * $Date: 2025-03-03 05:10:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using TMPro;

/// <summary>
/// Test text mesh components.
/// </summary>
public class FT_TextMesh : MonoBehaviour
{
    /* Variables */

    public TextMeshPro textMesh = null;

    public TMP_Text tmpText = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        this.textMesh = GetComponent<TextMeshPro>();
        this.tmpText = GetComponent<TMP_Text>();
    }
}
