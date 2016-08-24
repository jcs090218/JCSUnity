#if (UNITY_EDITOR)
/**
 * $File: ScriptTester.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class ScriptTester
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [Header("** Test Variables (JCS_DialogueSystem) **")]

    [Tooltip("Script to run the current text box. (test)")]
    [SerializeField]
    private JCS_DialogueScript mTestDialogueScript = null;

    public KeyCode DisposeKey = KeyCode.A;
    public KeyCode RunScriptKey = KeyCode.D;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {

    }

    private void Update()
    {
        if (mTestDialogueScript == null)
            return;
        JCS_DialogueSystem ds = JCS_UtilitiesManager.instance.GetDialogueSystem();

        if (JCS_Input.GetKeyDown(DisposeKey))
            ds.Dispose();
        if (JCS_Input.GetKeyDown(RunScriptKey))
            ds.ActiveDialogue(mTestDialogueScript);
    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
#endif
