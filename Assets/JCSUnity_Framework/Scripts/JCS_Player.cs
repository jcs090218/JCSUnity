using UnityEngine;
using System.Collections;

public class JCS_Player : MonoBehaviour
{

    protected virtual void Start()
    {
        // set Execute order lower than "JCS_GameManager"
        JCS_GameManager.instance.SetJCSPlayer(this);
    }

}
