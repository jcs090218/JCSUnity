using UnityEngine;
using System.Collections;

public class JCS_GameErrors : MonoBehaviour
{

    public static void JcsErrors(string script, int line = -1, string desc = "")
    {
        Debug.Log("*** [" + script + "](" + line + ")" + desc + " ***");
    }
}
