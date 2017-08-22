using UnityEngine;
using System;
using System.Collections;
using System.ComponentModel;
using System.Net;                                    // Endpoint
using System.Net.Sockets;                            // Socket namespace
using System.Text;                                    // Text encoders
using System.IO;

namespace JCSUnity
{
    /// <summary>
    /// Socket Descriptor holder.
    /// </summary>
    public class JCS_GameSocket
    {
        private Socket mSocket = null;                      // Server connection

        private byte[] mInputBuff = new byte[JCS_NetworkConstant.INBUFSIZE];    // Recieved data buffer
        private byte[] mOutputBuff = new byte[JCS_NetworkConstant.OUTBUFSIZE];

        private JCS_ClientHandler mClientHandler = null;
        

        //-----------------------------------------
        // functions
        //-----------------------------------------
        public JCS_GameSocket(JCS_ClientHandler handler = null)
        {
            if (handler != null)
                SetHandler(handler);
            else
                SetHandler(new JCS_DefaultClientHandler());
        }

        /// <summary>
        /// Try to connect once to the setting server.
        /// </summary>
        /// <param name="hostname"> Internet Protocal </param>
        /// <param name="port"> Port Number </param>
        public void Connect(string hostname, int port)
        {
            try
            {
                // Close the socket if it is still open
                if (mSocket != null && mSocket.Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    mSocket.Close();
                }

                // Create the socket object
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Define the Server address and port
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(hostname), port);

                // Connect to the server blocking method and setup callback for recieved data
                //mSocket.Connect(epServer);
                //SetupRecieveCallback(mSocket);

                // Connect to server non-Blocking method
                mSocket.Blocking = false;
                AsyncCallback onconnect = new AsyncCallback(OnConnect);

                mSocket.BeginConnect(epServer, onconnect, mSocket);
            }
            catch (Exception ex)
            {
                Debug.LogError("Server Connect failed: " + ex.Message);
                JCS_UtilityFunctions.PopIsConnectDialogue();
            }
        }

        /// <summary>
        /// Call back when on connect.
        /// </summary>
        /// <param name="ar"> async state. </param>
        public void OnConnect(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we were sucessfull
            try
            {
                sock.EndConnect(ar);
                if (sock.Connected)
                    SetupRecieveCallback(sock);
                else
                    Debug.LogError("Unable to connect to remote machine, Connected Failed");
            }
            catch (Exception ex)
            {
                Debug.LogError("Unusual error during Connect!: " + ex.Message);
                JCS_NetworkManager.SERVER_CLOSE = true;
            }
        }

        /// <summary>
        /// Get the new data and send it out to all other connections. 
        /// Note: If not data was recieved the connection has probably 
        /// died.
        /// </summary>
        /// <param name="ar"></param>
        public void OnRecievedData(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we got any data
            try
            {
                int nBytesRec = sock.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    // Wrote the data to the List
                    string sRecieved = Encoding.UTF8.GetString(mInputBuff, 0, nBytesRec);

                    // Set decode Charset!
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sRecieved);

                    // send buffer to decoder and get the decrypted packet
                    byte[] decryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetDecoder().Decode(buffer);

                    // invoke data to handler.
                    if (mClientHandler != null)
                        mClientHandler.MessageReceived(decryptedBuffer);

                    // WARNING : The following line is NOT thread safe. Invoke is
                    //m_lbRecievedData.Items.Add( sRecieved );
                    //Invoke(m_AddMessage, new string[] { sRecieved });

                    // If the connection is still usable restablish the callback
                    SetupRecieveCallback(sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    Console.WriteLine("Client {0}, disconnected", sock.RemoteEndPoint);
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Unusual error druing Recieve!: " + ex.Message);

                if (!SocketConnected(mSocket))
                {
                    mSocket.Close();
                    JCS_NetworkManager.SERVER_CLOSE = true;
                }
            }
        }

        /// <summary>
        /// Check if the socket already connected?
        /// </summary>
        /// <param name="s"> socket descriptor </param>
        /// <returns> boolean to check is connected? </returns>
        public bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        public void SetupRecieveCallback(Socket sock)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
                sock.BeginReceive(mInputBuff, 0, mInputBuff.Length, SocketFlags.None, recieveData, sock);
            }
            catch (Exception ex)
            {
                Debug.LogError("Setup Recieve Callback failed!: " + ex.Message);
            }
        }

        /// <summary>
        /// Close the socekt the safe way.
        /// </summary>
        public void Close()
        {
            if (mSocket != null && mSocket.Connected)
            {
                mSocket.Shutdown(SocketShutdown.Both);
                mSocket.Close();
            }
        }

        /// <summary>
        /// Check is connected to the server or not.
        /// </summary>
        /// <returns></returns>
        public bool isConnected()
        {
            return mSocket.Connected;
        }

        /// <summary>
        /// Send a sequence of byte to server.
        /// </summary>
        /// <param name="buffer"> byte array to send. </param>
        /// <returns> boolean to check if success. </returns>
        public bool SendPacket(byte[] buffer)
        {
            if (mSocket == null || !mSocket.Connected)
            {
                Debug.Log("Must be connected to Send a message");
                return false;
            }

            if (buffer == null)
                return false;

            byte[] encryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetEncoder().Encode(buffer);

            if (mClientHandler != null)
                mClientHandler.MessageSent(encryptedBuffer);

            try
            {
                mSocket.Send(encryptedBuffer, encryptedBuffer.Length, 0);
            }
            catch (Exception ex)
            {
                Debug.Log("Send Message Failed!: " + ex.Message);
                return false;
            }

            return true;
        }

        //-----------------------------------------
        // setter / getter
        //-----------------------------------------

        public byte[] GetInputBuffer() { return this.mInputBuff; }
        public byte[] GetOutPutBuffer() { return this.mOutputBuff; }

        /// <summary>
        /// Set the client handler.
        /// </summary>
        /// <param name="handler"> handler. </param>
        public void SetHandler(JCS_ClientHandler handler)
        {
            this.mClientHandler = handler;
        }

    }
}
