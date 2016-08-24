/**
 * $File: BF_InitCharacterInGame.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class BF_InitCharacterInGame 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [Tooltip("Facing direction starting of the game.")]
    [SerializeField]
    private JCS_2DFaceType mFacing 
        = JCS_2DFaceType.FACE_RIGHT;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public JCS_2DFaceType Facing { get { return mFacing; } }

    //========================================
    //      Unity's function
    //------------------------------
    
    
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
