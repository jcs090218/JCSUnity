using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_GameErrors : MonoBehaviour
    {

        public static void JcsErrors(string script, int line = -1, string desc = "")
        {
            Debug.Log("****");
            Debug.Log("*** JCSUnity.Errors - [" + script + "](" + line + ")" + desc + " ***");
            Debug.Log("****");
        }

        public static void JcsReminders(string script, int line = -1, string desc = "")
        {
            Debug.Log("****");
            Debug.Log("*** JCSUnity.Reminders - [" + script + "](" + line + ")" + desc + " ***");
            Debug.Log("****");
        }

        public static void JcsWarnings(string script, int line = -1, string desc = "")
        {
            Debug.Log("****");
            Debug.Log("*** JCSUnity.Warnings - [" + script + "](" + line + ")" + desc + " ***");
            Debug.Log("****");
        }
    }
}
