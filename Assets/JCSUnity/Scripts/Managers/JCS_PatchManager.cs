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

    /// <summary>
    /// 
    /// </summary>
    public class JCS_PatchManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PatchManager instance = null;

        //----------------------
        // Private Variables
        [SerializeField]
        private string mNextLevel = "JCS_LogoScene";

        [Tooltip("What do the server time out?")]
        [SerializeField] [Range(0, 600)]        // 0 sec ~ 600 sec
        private float mConnectTimeOut = 30;

        // timer for connection.
        private float mConnectTimer = 0;

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
        }

        private void Update()
        {
            // if is online game
            if (JCS_Startup.InitializeApplication())
            {
                Debug.Log("Server Connected!");

                LoadNextLevel();
            }

            DoTimeOut();
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Load the next level.
        /// </summary>
        public void LoadNextLevel()
        {
            JCS_SceneManager.instance.LoadScene(mNextLevel);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do the connection time out.
        /// </summary>
        private void DoTimeOut()
        {
            // add up timer.
            mConnectTimer += Time.deltaTime;

            // check if connection time out?
            if (mConnectTimer < mConnectTimeOut)
                return;

            // reset timer.
            mConnectTimer = 0;

            Debug.Log("Server Connection Time Out, Quit Application.");

            // quit the application smoothly.
            JCS_ApplicationManager.instance.Quit(true);
        }
    }
}
