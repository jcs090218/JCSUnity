/**
 * $File: JCS_GameWindowHandler.cs $
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

    /// <summary>
    /// Object to create the instance of Game Window.
    /// </summary>
    public class JCS_GameWindowHandler 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_GameWindowHandler instance = null;


        //----------------------
        // Private Variables

        // System UI
        [Header("** System Dialogue **")]
        [SerializeField] private JCS_DialogueObject[] mSystemUI = null;

        // Game UI
        [Header("** Game User Interface **")]
        [SerializeField] private JCS_DialogueObject mGameUI = null;

        // NPC Dialogue
        [Header("** NPC Dialogue **")]
        [SerializeField]
        private JCS_DialogueObject mNPCDialogue = null;

        // List of all the Game Window we are going to use in the game
        [Header("** Player Dialogue **")]
        [SerializeField] private JCS_DialogueObject[] mPlayerDialogue = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_DialogueObject GetPlayerDialogueAt(int index) { return this.mPlayerDialogue[index]; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            if (instance != null)
            {
                // [IMPORTANT] Only accept one this object!!!
                DestroyImmediate(this.gameObject);
                return;
            }

            instance = this;

        }

        private void Start()
        {
            PopSystemUI();
            PopGameUI();
            PopNPCDialogue();
            PopPlayerDialogue();
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
        
        private void PopNPCDialogue()
        {
            PopDialogue(mNPCDialogue);
        }
        private void PopGameUI()
        {
            PopDialogue(mGameUI);
        }
        private void PopPlayerDialogue()
        {
            PopDialogue(mPlayerDialogue);
        }
        private void PopSystemUI()
        {
            PopDialogue(mSystemUI);
        }
        private void PopDialogue(JCS_DialogueObject obj)
        {
            if (obj == null)
                return;

            obj = (JCS_DialogueObject)JCS_UsefualFunctions.SpawnGameObject(obj);
            obj.ShowDialogue();
            obj.SetKeyCode(KeyCode.None);
        }
        private void PopDialogue(JCS_DialogueObject[] list)
        {
            if (list.Length == 0)
            {
                JCS_GameErrors.JcsErrors("JCS_GameWindowHandler", -1, "No Game Window in the scene!");
                return;
            }

            for (int index = 0;
                index < list.Length;
                ++index)
            {
                if (list[index] == null)
                    continue;

                list[index] = (JCS_DialogueObject)JCS_UsefualFunctions.SpawnGameObject(list[index]);
                list[index].HideDialogue();
            }
        }


    }
}
