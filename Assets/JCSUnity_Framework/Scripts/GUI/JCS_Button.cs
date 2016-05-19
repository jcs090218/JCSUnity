/**
 * $File: JCS_Button.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public abstract class JCS_Button : MonoBehaviour
    {
        [SerializeField] protected int mDialogueIndex = -1;

        public abstract void JCS_ButtonClick();
    }
}
