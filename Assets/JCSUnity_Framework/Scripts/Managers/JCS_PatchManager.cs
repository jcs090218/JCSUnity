/**
 * $File: JCS_PatchManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{
    public class JCS_PatchManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PatchManager instance = null;

        //----------------------
        // Private Variables
        [SerializeField] private string mNextLevel = "JCS_LogoScene";

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public string NextLevel() { return this.mNextLevel; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {

            if (!JCS_ApplicationManager.ONLINE_MODE)
            {
                if (JCS_ApplicationManager.FIRST_LOGIN)
                {
                    JCS_ApplicationManager.FIRST_LOGIN = false;
                    LoadNextLevel();
                }

                return;
            }

            // if is online game
            if (!JCS_Startup.InitializeApplication())
            {
                // Fail handler
                return;
            }

            //LoadNextLevel();

            Debug.Log("Server Connected!");
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void LoadNextLevel()
        {
            SceneManager.LoadScene(mNextLevel);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
    }
}
