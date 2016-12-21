/**
 * $File: JCS_Startup.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_Startup
    {

        /// <summary>
        /// Initialize the whole application. (In Unity layer.)
        /// </summary>
        /// <returns></returns>
        public static bool InitializeApplication()
        {
            if (!JCS_ApplicationManager.ONLINE_MODE)
                return false;

            Debug.Log("Is Online Game!");

            // Create Connection
            if (!JCS_NetworkManager.CreateNetwork(JCS_NetworkConstant.HOST_NAME, JCS_NetworkConstant.PORT))
            {
                // Faild handle
                return false;
            }

            // Create keys
            JCS_NetworkManager.CreateKey();

            // 這裡無法判別
            return true;
        }

    }
}
