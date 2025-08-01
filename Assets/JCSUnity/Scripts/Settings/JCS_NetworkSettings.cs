/**
 * $File: JCS_NetworkSettings.cs $
 * $Date: 2017-08-20 07:08:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;
using NUnit.Framework.Internal.Execution;

namespace JCSUnity
{
    /// <summary>
    /// Store all the network settings.
    /// </summary>
    [RequireComponent(typeof(JCS_ServerRequestProcessor))]
    [RequireComponent(typeof(JCS_PacketLostPreventer))]
    public class JCS_NetworkSettings : JCS_Settings<JCS_NetworkSettings>
    {
        /* Variables */

        [Separator("Check Variables (JCS_NetworkSettings)")]

        [Tooltip(@"Current mode this client in, should be update by the server!")]
        [ReadOnly]
        public JCS_ClientMode CLIENT_MODE = JCS_ClientMode.LOGIN_SERVER;

        [Tooltip("On switching the server?")]
        [ReadOnly]
        public bool ON_SWITCH_SERVER = false;

        [Tooltip("Flag to check if is force switching the server.")]
        [ReadOnly]
        public bool FORCE_SWITCH_SERVER = false;

        [Separator("Runtime Variables (JCS_NetworkSettings)")]

        [Tooltip("Is the current game with online mode active?")]
        public bool ONLINE_MODE = false;

        [Tooltip("Type of the client protocal.")]
        public JCS_ProtocalType PROTOCAL_TYPE = JCS_ProtocalType.TCP;

        [Tooltip("Client hostname.")]
        public string HOST_NAME = "127.0.0.1";

        [Tooltip("Client port.")]
        public int PORT = 5454;

        [Tooltip("Channel count in this game.")]
        public int CHANNEL_COUNT = 1;

        private static JCS_Socket SOCKET = null;
        private static JCS_ClientHandler PRESET_CLIENT_HANDLER = null;

        private JCS_ServerRequestProcessor mServerRequestProcessor = null;
        private JCS_PacketLostPreventer mPacketLostPreventer = null;

        /* Setter & Getter */

        public static void PresetClientHandler(JCS_ClientHandler handler) { PRESET_CLIENT_HANDLER = handler; }
        public static JCS_ClientHandler GetPresetClientHandler() { return PRESET_CLIENT_HANDLER; }
        public JCS_ServerRequestProcessor GetServerRequestProcessor() { return this.mServerRequestProcessor; }
        public JCS_PacketLostPreventer GetPacketLostPreventer() { return this.mPacketLostPreventer; }

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            this.mServerRequestProcessor = this.GetComponent<JCS_ServerRequestProcessor>();
            this.mPacketLostPreventer = this.GetComponent<JCS_PacketLostPreventer>();
        }

        private void OnApplicationQuit()
        {
            if (ONLINE_MODE)
                CloseSocket();
        }

        private void Update()
        {
            OnSwitchServer();
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
            return CreateNetwork(hostname, port, GetPresetClientHandler());
        }

        /// <summary>
        /// Create the socket and connect to the host and 
        /// port provided.
        /// </summary>
        /// <param name="hostname"> host name </param>
        /// <param name="port"> port number </param>
        /// <param name="handler"></param>
        /// <returns> Sucess or vice versa. </returns>
        public static bool CreateNetwork(string hostname, int port, JCS_ClientHandler handler)
        {
            if (SOCKET != null)
                return false;

            switch (FirstInstance().PROTOCAL_TYPE)
            {
                case JCS_ProtocalType.TCP:
                    {
                        SOCKET = new JCS_TCPSocket(handler);
                        SOCKET.Connect(hostname, port);
                    }
                    break;
                case JCS_ProtocalType.UDP:
                    {
                        SOCKET = new JCS_UDPSocket(handler);
                        SOCKET.Connect(hostname, port);
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Close the current socket the safe way.
        /// </summary>
        public static void CloseSocket()
        {
            if (SOCKET == null)
                return;

            SOCKET.Close();
            SOCKET = null;
        }

        /// <summary>
        /// Return the socket we are using.
        /// </summary>
        /// <returns> socket. </returns>
        public static JCS_Socket GetSocket()
        {
            return SOCKET;
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        public void SwitchServer()
        {
            SwitchServer(HOST_NAME, PORT);
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        public void SwitchServer(
            string hostname,
            int port)
        {
            SwitchServer(
                hostname,
                port,
                false,
                GetPresetClientHandler());
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        /// <param name="force"> force to switch server? (Default : false) </param>
        public void SwitchServer(
            string hostname,
            int port,
            bool force)
        {
            SwitchServer(
                hostname,
                port,
                force,
                GetPresetClientHandler());
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        /// <param name="force"> force to switch server? (Default : false) </param>
        /// <param name="handler"> handler handle packet income. </param>
        public void SwitchServer(
            string hostname,
            int port,
            bool force,
            JCS_ClientHandler handler)
        {
            if (HOST_NAME == hostname &&
                PORT == port &&
                handler == null)
            {
                Debug.LogError(
                    "Not need to switch server, we already in....");
                return;
            }

            // update hostname, port, and handler.
            HOST_NAME = hostname;
            PORT = port;

            if (handler != null)
                PresetClientHandler(handler);

            // start switching server.
            ON_SWITCH_SERVER = true;

            FORCE_SWITCH_SERVER = force;
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_NetworkSettings _old, JCS_NetworkSettings _new)
        {
            _new.ONLINE_MODE = _old.ONLINE_MODE;
            _new.HOST_NAME = _old.HOST_NAME;
            _new.PORT = _old.PORT;
            _new.PROTOCAL_TYPE = _old.PROTOCAL_TYPE;
            _new.CHANNEL_COUNT = _old.CHANNEL_COUNT;

            _new.ON_SWITCH_SERVER = _old.ON_SWITCH_SERVER;
            _new.FORCE_SWITCH_SERVER = _old.FORCE_SWITCH_SERVER;

            _new.CLIENT_MODE = _old.CLIENT_MODE;
        }

        /// <summary>
        /// On switching the server.
        /// </summary>
        private void OnSwitchServer()
        {
            if (!ON_SWITCH_SERVER)
                return;

            var plp = JCS_PacketLostPreventer.FirstInstance();

            // If not force, then we do need to check if we meet the 
            // requirement to swtich server.
            if (!FORCE_SWITCH_SERVER)
            {
                // Cannot switch server if we are still waiting for packet 
                // to process.
                if (plp.IsPreventing())
                    return;
            }
            else
            {
                // If force to switch the server/service. Meaning we 
                // will like to terminate the server request!
                // 
                // ATTENTION(jenchieh): Use this carefully.
                plp.ClearTracking();
            }

            // close the previous one.
            CloseSocket();

            // open the new one for next server.
            CreateNetwork(FirstInstance().HOST_NAME, FirstInstance().PORT);

            ON_SWITCH_SERVER = false;
            FORCE_SWITCH_SERVER = false;
        }
    }
}
