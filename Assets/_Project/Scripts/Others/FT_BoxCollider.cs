/**
 * $File: FT_BoxCollider.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

[RequireComponent(typeof(BoxCollider))]
public class FT_BoxCollider : MonoBehaviour
{
    /* Variables */

    private BoxCollider mBoxCollider = null;
    [Separator("Check Variables")]

    [SerializeField] 
    private float mWidth = 0;

    [SerializeField] 
    private float mHeight = 0;

    /* Setter & Getter */

    public float Width { get { return this.mWidth; } }
    public float Height { get { return this.mHeight; } }

    /* Functions */

    private void Awake()
    {
        mBoxCollider = this.GetComponent<BoxCollider>();

        Vector2 widthHeight = JCSUnity.JCS_Physics.GetColliderWidthHeight(mBoxCollider);

        mWidth = widthHeight.x;
        mHeight = widthHeight.y;
    }

    private void Update()
    {
        Vector3 pos = this.transform.position;

        Debug.DrawLine(new Vector3(pos.x, pos.y - (Height / 2), pos.z),
            new Vector3(pos.x, pos.y + (Height / 2), pos.z));

        Debug.DrawLine(new Vector3(pos.x + (Width / 2), pos.y, pos.z),
            new Vector3(pos.x - (Width / 2), pos.y, pos.z));
    }
}
