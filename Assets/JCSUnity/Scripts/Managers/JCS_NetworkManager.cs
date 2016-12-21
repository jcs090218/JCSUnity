/**
 * $File: JCS_NetworkManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_NetworkManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_NetworkManager instance = null;
        public static bool SERVER_CLOSE = false;

        //----------------------
        // Private Variables
        private static JCS_GameSocket GAME_SOCKET = null;
        private float mConnectionCounter = 0.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (!JCS_ApplicationManager.ONLINE_MODE)
                return;

#if (UNITY_EDITOR)
            Test();
#endif

            if (JCS_ApplicationManager.APP_PAUSE)
                return;

            CheckConnectionWithTime();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.P))
            {
                byte[] bytesSend = new byte[5];
                bytesSend[0] = (byte)'H';
                bytesSend[1] = (byte)'E';
                bytesSend[2] = (byte)'l';
                bytesSend[3] = (byte)'L';
                bytesSend[4] = (byte)'o';
                GAME_SOCKET.SendPacket(bytesSend);
            }

            if (JCS_Input.GetKeyDown(KeyCode.O))
            {
                byte[] bytesSend = new byte[10];
                bytesSend[0] = (byte)'Z';
                bytesSend[1] = (byte)'X';
                bytesSend[2] = (byte)'C';
                bytesSend[3] = (byte)'V';
                bytesSend[4] = (byte)'B';
                bytesSend[5] = (byte)'N';
                bytesSend[6] = (byte)'M';
                bytesSend[7] = (byte)',';
                bytesSend[8] = (byte)'.';
                bytesSend[9] = (byte)'/';
                GAME_SOCKET.SendPacket(bytesSend);
            }

        }
#endif

        private void OnApplicationQuit()
        {
            if (JCS_ApplicationManager.ONLINE_MODE)
                JCS_NetworkManager.GAME_SOCKET.Close();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public static bool CreateKey()
        {
            // Create keys
            JCS_NetworkConstant.DECODE_BUFFER[0] = (byte)'J';
            JCS_NetworkConstant.DECODE_BUFFER[1] = (byte)'C';
            JCS_NetworkConstant.DECODE_BUFFER[2] = (byte)'S';
            JCS_NetworkConstant.DECODE_BUFFER[3] = (byte)'D';

            JCS_NetworkConstant.ENCODE_BUFFER[0] = (byte)'J';
            JCS_NetworkConstant.ENCODE_BUFFER[1] = (byte)'C';
            JCS_NetworkConstant.ENCODE_BUFFER[2] = (byte)'S';
            JCS_NetworkConstant.ENCODE_BUFFER[3] = (byte)'E';

            return true;
        }
        public static bool CreateNetwork(string hostname, int port)
        {
            if (GAME_SOCKET != null)
                return false;

            GAME_SOCKET = new JCS_GameSocket();
            GAME_SOCKET.Connect(hostname, port);

            return true;
        }
        public static JCS_GameSocket GetGameSocket()
        {
            return GAME_SOCKET;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true : connect, false: disconnect</returns>
        public void CheckConnectionWithTime()
        {
            // do the following script only when is online mode
            if (!JCS_ApplicationManager.ONLINE_MODE)
                return;

            mConnectionCounter += Time.deltaTime;

            if (mConnectionCounter < JCS_NetworkConstant.CONNECT_TIME)
                return;
            else
            {
                if (SERVER_CLOSE)
                {
                    Debug.Log("Server End!");
                    JCS_UtilityFunctions.PopIsConnectDialogue();
                }
                else if (JCS_ApplicationManager.FIRST_LOGIN)
                {
                    JCS_ApplicationManager.FIRST_LOGIN = false;
                    JCS_PatchManager.instance.LoadNextLevel();
                }
                mConnectionCounter = 0;
            }
        }
        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
