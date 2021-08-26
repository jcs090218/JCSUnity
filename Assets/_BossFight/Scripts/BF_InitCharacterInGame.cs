/**
 * $File: BF_InitCharacterInGame.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class BF_InitCharacterInGame : MonoBehaviour
{
    /* Variables */

    [Tooltip("Facing direction starting of the game.")]
    [SerializeField]
    private JCS_2DFaceType mFacing = JCS_2DFaceType.FACE_RIGHT;

    /* Setter & Getter */
    
    public JCS_2DFaceType Facing { get { return mFacing; } }

    /* Functions */

}
