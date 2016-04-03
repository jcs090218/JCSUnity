using UnityEngine;
using System.Collections;

/// <summary>
/// Make sure u have this execute first!!!
/// 
/// by Jen-Chieh Shen<lkk440456@gmail.com>
/// </summary>
public class JCS_GameManager : MonoBehaviour
{
    public static JCS_GameManager instance = null;

    private JCS_Camera mJCSCamera = null;
    private JCS_Player mJCSPlayer = null;


    //--------------------------------
    // setter / getter
    //--------------------------------
    public void SetJCSCamera(JCS_Camera cam) { this.mJCSCamera = cam; }
    public JCS_Camera GetJCSCamera() { return this.mJCSCamera; }
    public void SetJCSPlayer(JCS_Player player) { this.mJCSPlayer = player; }
    public JCS_Player GetJCSPlayer() { return this.mJCSPlayer; }

    //--------------------------------
    // Unity's functions
    //--------------------------------
    private void Awake()
    {
        instance = this;
    }
}
