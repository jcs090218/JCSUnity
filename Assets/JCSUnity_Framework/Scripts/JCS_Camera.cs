using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class JCS_Camera : MonoBehaviour
{
    private Camera mCamera = null;

    private void Awake()
    {
        mCamera = this.GetComponent<Camera>();

        // set Execute order lower than "JCS_GameManager"
        JCS_GameManager.instance.SetJCSCamera(this);
    }

	
}
