/**
 * $File: JCS_GUIUtil.cs $
 * $Date: 2018-07-16 13:28:22 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JCSUnity
{
    /// <summary>
    /// Store all the GUI related utilities function here.
    /// </summary>
    public class JCS_GUIUtil
    {
        /// <summary>
        /// Returns item vlaue by index.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="index"> item name. </param>
        /// <returns></returns>
        public static string Dropdown_GetItemValue(Dropdown dd, int index)
        {
            return dd.options[index].text;
        }

        /// <summary>
        /// Get the current selected value of the Dropdown object.
        /// </summary>
        /// <param name="dd"> drop down object. </param>
        /// <returns> current selected text value. </returns>
        public static string Dropdown_GetSelectedValue(Dropdown dd)
        {
            return Dropdown_GetItemValue(dd, dd.value);
        }

        /// <summary>
        /// Return the index of item in the dropdown's item.
        /// If not found, return -1.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="itemName"> item name. </param>
        /// <returns>
        /// Index of the item value found.
        /// If not found, will returns -1.
        /// </returns>
        public static int Dropdown_GetItemIndex(Dropdown dd, string itemName)
        {
            for (int index = 0;
                index < dd.options.Count;
                ++index)
            {
                if (itemName == Dropdown_GetItemValue(dd, index))
                    return index;
            }

            return -1;
        }
    }
}
