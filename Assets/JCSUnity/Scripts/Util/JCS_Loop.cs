/**
 * $File: JCS_Loop.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Loop operations.
    /// </summary>
    public static class JCS_Loop
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Loops a specific number of times.
        /// </summary>
        /// <param name="times"> Times to loop over. </param>
        /// <param name="action"> Action to execute each times. </param>
        public static void Times(int times, Action<int> action)
        {
            for (int count = 0; count < times; ++count)
            {
                if (action != null)
                    action.Invoke(count);
            }
        }
    }
}
