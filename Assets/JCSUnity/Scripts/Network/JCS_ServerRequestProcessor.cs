/**
 * $File: JCS_ServerRequestProcessor.cs $
 * $Date: 2017-08-24 17:08:45 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;


namespace JCSUnity
{
    public delegate void ServerRequest(BinaryReader br, JCS_Client client);

    /// <summary>
    /// Process the all server request as callback.
    /// </summary>
    public class JCS_ServerRequestProcessor
        : JCS_Settings<JCS_ServerRequestProcessor>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private List<ServerRequest> mServerRequest = new List<ServerRequest>();
        private List<JCS_Client> mClient = new List<JCS_Client>();
        private List<BinaryReader> mBinaryReader = new List<BinaryReader>();

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Update()
        {
            ProcessServerRequest();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Register request from server.
        /// </summary>
        public void RegisterRequest(ServerRequest request, BinaryReader br, JCS_Client client)
        {
            mServerRequest.Add(request);
            mBinaryReader.Add(br);
            mClient.Add(client);
        }

        /// <summary>
        /// Deregister the request from server.
        /// </summary>
        public void DeresgisterRequest(ServerRequest request, BinaryReader br, JCS_Client client)
        {
            mServerRequest.Remove(request);
            mBinaryReader.Remove(br);
            mClient.Remove(client);
        }

        //----------------------
        // Protected Functions

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
        protected override void TransferData(JCS_ServerRequestProcessor _old, JCS_ServerRequestProcessor _new)
        {
            // before transfer, do all the packet request.
            ProcessServerRequest();
        }

        //----------------------
        // Private Functions

        /// <summary>
        /// Process through the server request.
        /// </summary>
        private void ProcessServerRequest()
        {

            for (int index = 0;
                index < mServerRequest.Count;
                ++index)
            {
                BinaryReader br = mBinaryReader[index];
                JCS_Client client = mClient[index];

                mServerRequest[index].Invoke(br, client);
            }

            // done all request, wait for next frame's request.
            mServerRequest.Clear();
            mClient.Clear();
            mBinaryReader.Clear();
        }
    }
}
