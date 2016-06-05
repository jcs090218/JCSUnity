/**
 * $File: ScreenSpace_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterController))]
public class ScreenSpace_Test 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [SerializeField]
    private Transform mTransformShow = null;
    [SerializeField]
    private Transform mTransform = null;

    private CharacterController mCharacterController = null;
    private SpriteRenderer mSpriteRenderer = null;

    private JCS_Camera jcsCam = null;
    

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
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();
        mCharacterController = this.GetComponent<CharacterController>();
    }

    private void Start()
    {
        jcsCam = JCS_Camera.main;
    }

    private void LateUpdate()
    {
        print(jcsCam.CheckInScreenSpace(mCharacterController));
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
