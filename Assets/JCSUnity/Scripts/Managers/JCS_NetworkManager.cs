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
    /// Handle the networking of the game.
    /// 
    /// NOTE(jenchieh): To use this so the game could possibly become 
    /// the MO/MMO game.
    /// </summary>
    public class JCS_NetworkManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_NetworkManager instance = null;
        public static bool SERVER_CLOSE = false;

        public static bool FIRST_LOGIN = true;

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
            if (!JCS_NetworkSettings.instance.ONLINE_MODE)
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
            if (JCS_NetworkSettings.instance.ONLINE_MODE)
                JCS_NetworkManager.GAME_SOCKET.Close();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Spawn the security key for the packet.
        /// 
        /// NOTE(jenchieh): This is the most basic key, will recommand to 
        /// switch something more secure.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create the socket and connect to the host and 
        /// port provided.
        /// </summary>
        /// <param name="hostname"> host name </param>
        /// <param name="port"> port number </param>
        /// <returns> Sucess or vice versa. </returns>
        public static bool CreateNetwork(string hostname, int port)
        {
            if (GAME_SOCKET != null)
                return false;

            GAME_SOCKET = new JCS_GameSocket();
            GAME_SOCKET.Connect(hostname, port);

            return true;
        }

        /// <summary>
        /// Close the current socket the safe way.
        /// </summary>
        public static void CloseSocket()
        {
            if (GAME_SOCKET == null)
                return;

            GAME_SOCKET.Close();
        }

        /// <summary>
        /// Return the Game socket we are using.
        /// </summary>
        /// <returns> socket. </returns>
        public static JCS_GameSocket GetGameSocket()
        {
            return GAME_SOCKET;
        }

        /// <summary>
        /// Do ping pong packet action.
        /// </summary>
        /// <returns>true : connect, false: disconnect</returns>
        public void CheckConnectionWithTime()
        {
            // do the following script only when is online mode
            if (!JCS_NetworkSettings.instance.ONLINE_MODE)
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
                else if (FIRST_LOGIN)
                {
                    FIRST_LOGIN = false;
                    JCS_PatchManager.instance.LoadNextLevel();
                }
                mConnectionCounter = 0;
            }
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        public void SwitchServer()
        {
            SwitchServer(
                JCS_NetworkSettings.instance.HOST_NAME, 
                JCS_NetworkSettings.instance.PORT);
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        public void SwitchServer(string hostname, int port)
        {
            // update hostname and port.
            JCS_NetworkSettings.instance.HOST_NAME = hostname;
            JCS_NetworkSettings.instance.PORT = port;

            // close the previous one.
            CloseSocket();

            // open the new one for next server.
            CreateNetwork(hostname, port);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
