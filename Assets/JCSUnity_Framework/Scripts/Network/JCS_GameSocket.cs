using UnityEngine;
using System;
using System.Collections;
using System.ComponentModel;
using System.Net;									// Endpoint
using System.Net.Sockets;							// Socket namespace
using System.Text;									// Text encoders
using System.IO;

namespace JCSUnity
{
    public class JCS_GameSocket
    {
        private Socket mSocket = null;                      // Server connection

        private byte[] mInputBuff = new byte[JCS_NetworkConstant.INBUFSIZE];    // Recieved data buffer
        private byte[] mOutputBuff = new byte[JCS_NetworkConstant.OUTBUFSIZE];


        //-----------------------------------------
        // setter / getter
        //-----------------------------------------
        public byte[] GetInputBuffer() { return this.mInputBuff; }
        public byte[] GetOutputBuffer() { return this.mOutputBuff; }

        //-----------------------------------------
        // functions
        //-----------------------------------------
        public JCS_GameSocket()
        {
            AssignBuffer(GetInputBuffer(), (byte)'A');
            AssignBuffer(GetOutputBuffer(), (byte)'A');
        }

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
                JCS_ButtonFunctions.PopIsConnectDialogue();
            }
        }

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
                    byte[] buffer = System.Text.Encoding.Default.GetBytes(sRecieved);

                    // print it out for test
                    //PrintRecievedPacket(buffer);


                    // send buffer to decoder and get the decrypted packet
                    byte[] decryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetDecoder().Decode(buffer);

                    // print it out for test
                    PrintRecievedPacket(decryptedBuffer);


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

        public void Close()
        {
            if (mSocket != null && mSocket.Connected)
            {
                mSocket.Shutdown(SocketShutdown.Both);
                mSocket.Close();
            }
        }

        public bool isConnected()
        {
            return mSocket.Connected;
        }

        public bool SendPacket(byte[] buffer)
        {
            if (mSocket == null || !mSocket.Connected)
            {
                Debug.Log("Must be connected to Send a message");
                return false;
            }

            byte[] encryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetEncoder().Encode(buffer);

            PrintSendPacket(encryptedBuffer);

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

        private void PrintSendPacket(System.Object message)
        {
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] encryptedBuffer = (byte[])message;

            // Print out the buffer for test
            {
                Debug.Log("---------- Start printing SEND Message ----------");

                PrintBuffer(encryptedBuffer);

                Debug.Log("---------- End printing SEND Message ----------");
            }
        }
        private void PrintRecievedPacket(System.Object message)
        {
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] decryptedBuffer = (byte[])message;

            // Print out the buffer for test
            {
                Debug.Log("---------- Start printing RECIEVE Message ----------");

                PrintBuffer(decryptedBuffer);

                Debug.Log("---------- End printing RECIEVE Message ----------");
            }
        }

        public void PrintInputBuffer()
        {
            PrintBuffer(GetInputBuffer());
        }
        public void PrintOutputBuffer()
        {
            PrintBuffer(GetOutputBuffer());
        }

        private void PrintBuffer(byte[] buffer)
        {
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            for (int index = 0; index < buffer.Length; ++index)
                Debug.Log((char)buffer[index]);
        }

        private void AssignBuffer(byte[] buffer, byte val)
        {
            for (int index = 0; index < buffer.Length; ++index)
                buffer[index] = val;
        }


    }
}
