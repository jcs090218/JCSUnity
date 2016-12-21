/**
 * $File: JCS_Pair.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    public class JCS_Pair<T, U>
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private T[] mPair1 = null;
        private U[] mPair2 = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int length { get { return mPair1.Length; } }
        public bool empty { get { return (length == 0); } }

        public T[] GetPair1() { return this.mPair1; }
        public U[] GetPair2() { return this.mPair2; }

        //========================================
        //      Constructor
        //------------------------------
        public JCS_Pair(int length = 0)
        {
            mPair1 = new T[length];
            mPair2 = new U[length];
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
