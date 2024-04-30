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

    public SpriteRenderer SpriteRenderer { get { return this.mSpriteRenderer; } }
    public Transform TransformTemp { get { return this.mTransform; } }
    public Transform TransformShow { get { return this.mTransformShow; } }

    /* Functions */

    private void Awake()
    {
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();
        mCharacterController = this.GetComponent<CharacterController>();
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
