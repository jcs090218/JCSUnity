/**
 * $File: FT_ScreenSpace.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterController))]
public class FT_ScreenSpace : MonoBehaviour
{
    /* Variables */

    [SerializeField]
    private Transform mTransformShow = null;
    [SerializeField]
    private Transform mTransform = null;

    private CharacterController mCharacterController = null;
    private SpriteRenderer mSpriteRenderer = null;

    private JCS_Camera mCamera = null;

    /* Setter & Getter */

    public SpriteRenderer SpriteRenderer { get { return mSpriteRenderer; } }
    public Transform TransformTemp { get { return mTransform; } }
    public Transform TransformShow { get { return mTransformShow; } }

    /* Functions */

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mCharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        mCamera = JCS_Camera.main;
    }

    private void LateUpdate()
    {
        print(mCamera.CheckInScreenSpace(mCharacterController));
    }
}
