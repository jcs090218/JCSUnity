/**
 * $File: JCS_InactiveWithTime.cs $
 * $Date: 2021-08-20 22:37:15 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Inactive the gameobject after a certain time.
    /// </summary>
    public class JCS_InactiveWithTime : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Runtime Variables (JCS_InactiveWithTime) **")]

        [Tooltip("Time before inactive the gameobject.")]
        [SerializeField]
        private float mTime = 2.0f;

        /* Setter & Getter */

        public float time { get { return this.mTime; } set { this.mTime = value; } }

        /* Functions */

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mTime < mTimer)
            {
                // reset timer.
                mTimer = 0.0f;

                // disable this object
                this.gameObject.SetActive(false);
            }
        }
    }
}
