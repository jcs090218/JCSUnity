using UnityEngine;
using System.Collections;

public class JCS_GameSettings : MonoBehaviour
{
    public static JCS_GameSettings instance = null;

    public static bool DEBUG_MODE = true;
    [SerializeField] public bool LEVEL_DESIGN_MODE = true;

    //--------------------------------
    // setter / getter
    //--------------------------------

    //--------------------------------
    // Unity's functions
    //--------------------------------
    private void Awake() { instance = this; }

}
