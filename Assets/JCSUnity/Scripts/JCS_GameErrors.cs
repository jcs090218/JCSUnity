using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

namespace JCSUnity
{

    public class JCS_GameErrors 
        : MonoBehaviour
    {

        public static void JcsErrors(string script, string desc)
        {
            //string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            Log("Errors", script, currentLine, desc);
        }
        public static void JcsErrors(Object script, string desc)
        {
            JcsErrors(script.GetType().Name, desc);
        }


        public static void JcsReminders(string script, string desc)
        {
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            Log("Reminders", script, currentLine, desc);
        }
        public static void JcsReminders(Object script, string desc)
        {
            JcsReminders(script.GetType().Name, desc);
        }


        public static void JcsWarnings(string script, string desc)
        {
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            Log("Warnings", script, currentLine, desc);
        }
        public static void JcsWarnings(Object script, string desc)
        {
            JcsWarnings(script.GetType().Name, desc);
        }


        private static void Log(string type, string script, int line = -1, string desc = "")
        {
            Debug.Log("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Debug.Log("=-= JCSUnity." + type + " - [" + script + "](" + line + ")" + desc + " =-=");
            Debug.Log("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
        }


    }
}
