/**
 * $File: JCS_Array.cs $
 * $Date: 2025-06-09 14:18:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JCSUnity
{
    /// <summary>
    /// Array utilities.
    /// </summary>
    public static class JCS_Array
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Loop in a collection.
        /// </summary>
        /// <typeparam name="T"> Type. </typeparam>
        /// <param name="index"> Index </param>
        /// <param name="arr"> List. </param>
        /// <returns> index that looped. </returns>
        public static int LoopIn(int index, ICollection arr)
        {
            // loop through the array, if at the tail of the array set it to head.
            if (index < 0)
                index = arr.Count - 1;
            // loop through the array, if at head of the array we set it to the tail.
            else if (index >= arr.Count)
                index = 0;

            return index;
        }

        /// <summary>
        /// Merge multiple arrays into one array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] Merge<T>(params T[][] arrList)
        {
            if (arrList.Length == 0)
                return null;

            if (arrList.Length == 1)
                return arrList[0];

            int arrLen = 0;

            foreach (var arr in arrList)
            {
                arrLen += arr.Length;
            }

            // first combine the first two array.
            T[] data = Merge2(arrList[0], arrList[1]);

            // combine the rest.
            for (int index = 2; index < arrList.Length; ++index)
            {
                data = Merge2(data, arrList[index]);
            }
            return data;
        }

        /// <summary>
        /// Merging two array and return the new array.
        /// </summary>
        /// <typeparam name="T"> Type of the array. </typeparam>
        /// <param name="arr1"> First array. </param>
        /// <param name="arr2"> Second array. </param>
        /// <returns> Merged array. </returns>
        public static T[] Merge2<T>(T[] arr1, T[] arr2)
        {
            T[] data = new T[arr1.Length + arr2.Length];

            Array.Copy(arr1, data, arr1.Length);
            Array.Copy(arr2, 0, data, arr1.Length, arr2.Length);

            return data;
        }

        /// <summary>
        /// Check if the list empty.
        /// </summary>
        public static bool IsEmpty(string[] list)
        {
            for (int index = 0; index < list.Length; ++index)
            {
                if (list[index] != "")
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Pop the last value from the list.
        /// </summary>
        public static T PopFront<T>(List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            T data = list[0];

            list.RemoveAt(0);

            return data;
        }

        /// <summary>
        /// Pop the last value from the list.
        /// </summary>
        public static T PopBack<T>(List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            int lastIndex = list.Count - 1;

            T data = list[lastIndex];

            list.RemoveAt(lastIndex);

            return data;
        }

        /// <summary>
        /// Fill slots with initialize value type by length.
        /// </summary>
        /// <typeparam name="T"> Type from `inArray`. </typeparam>
        /// <param name="inArray"> Array you would like to fill out. </param>
        /// <param name="len"> Target length to initialize. </param>
        /// <param name="with"> Initialize object type. </param>
        /// <returns> Return filled array. </returns>
        public static T[] Fill<T>(T[] inArray, int len, T with)
        {
            return Fill(inArray.ToList(), len, with).ToArray();
        }
        public static List<T> Fill<T>(List<T> inList, int len, T with)
        {
            for (int index = inList.Count; index < len; ++index)
                inList.Add(with);
            return inList;
        }

        /// <summary>
        /// Remove the empty slot in the array.
        /// </summary>
        /// <typeparam name="T"> Type of the List. </typeparam>
        /// <param name="inArray"> Array list. </param>
        /// <returns> Cleaned up Array object. </returns>
        public static T[] RemoveEmpty<T>(T[] inArray)
        {
            return RemoveEmpty(inArray.ToList()).ToArray();
        }
        public static List<T> RemoveEmpty<T>(List<T> inList)
        {
            List<T> newArray = new(inList.Count);

            for (int index = 0; index < inList.Count; ++index)
            {
                // Add itself if exists.
                if (inList[index] != null)
                    newArray.Add(inList[index]);
            }

            return newArray;
        }

        /// <summary>
        /// Remove the empty slot in the list including remove 
        /// the missing game object too. 
        /// 
        /// I guess Unity do the CG collection later a while when 
        /// you call 'Destory()' function. Before scripting layer 
        /// acknowledge this game object is destory might be too 
        /// late in some situation. This will avoid this type of 
        /// issue/circumstance.
        /// </summary>
        /// <typeparam name="T"> Type of the List. </typeparam>
        /// <param name="inList"> List object. </param>
        /// <returns> Cleaned up List object. </returns>
        public static T[] RemoveEmptyMissing<T>(T[] inArray)
            where T : UnityEngine.Object
        {
            return RemoveEmptyMissing(inArray.ToList()).ToArray();
        }
        public static List<T> RemoveEmptyMissing<T>(List<T> inList)
            where T : UnityEngine.Object
        {
            List<T> newArray = new(inList.Count);

            for (int index = 0; index < inList.Count; ++index)
            {
                // Add itself if exists.
                // 
                // SOURCE(jenchieh): https://answers.unity.com/questions/131158/how-can-i-check-if-an-object-is-null.html
                // INFORMATION(jenchieh): https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
                if (inList[index] ?? false)
                    newArray.Add(inList[index]);
            }

            return newArray;
        }
    }
}
