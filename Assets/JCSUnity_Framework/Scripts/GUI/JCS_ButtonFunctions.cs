/**
 * $File: JCS_ButtonFunctions.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{

    public class JCS_ButtonFunctions 
        : MonoBehaviour
    {
        private static string IS_CONNECT_DIALOGUE = "JCSUnity_Framework_Resources/JCS_ForceDialogue/JCS_MessageBox_YesNo";
        private static string SETTING_PANEL = "JCSUnity_Framework_Resources/JCS_InGameDialogue/Setting_Panel";
        private static string TALK_DIALOGUE = "JCSUnity_Framework_Resources/JCS_InGameDialogue/Conversation_Dialogue";

        public static void PauseGame(bool act = true)
        {
            JCS_GameManager.instance.GAME_PAUSE = act;
        }

        public static void PasueApplication(bool act = true)
        {
            JCS_ApplicationManager.APP_PAUSE = act;
        }

        public static void ToOfficailWebpage()
        {
            Application.OpenURL(JCS_NetworkConstant.OFFICIAL_WEBSITE);
        }

        public static void QuitApplication()
        {
#if UNITY_EDITOR
            SceneManager.LoadScene("JCS_ApplicationCloseSimulateScene");
            JCS_ApplicationManager.APP_PAUSE = true;
#endif

            Application.Quit();
        }

        public static void DestoryCurrentDialogue(JCS_DialogueType type)
        {
            if (JCS_GameManager.instance.GAME_PAUSE)
                return;

            JCS_UIManager.instance.HideTheLastOpenDialogue();

            switch (type)
            {
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                case JCS_DialogueType.NPC_DIALOGUE:
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    PauseGame(false);
                    break;
            }
        }

        private static bool CheckIfOkayToSpawnGameUI(JCS_DialogueType type)
        {
            if (JCS_UIManager.instance.GetJCSDialogue(type) != null)
            {
                JCS_GameErrors.JcsErrors("JCS_ButtonFunctions", -1, "(" + type.ToString() + ")No able to spawn Game UI cuz there are multiple GameUI in the scene...");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if enable to spawn the dialogue or not
        /// </summary>
        /// <returns></returns>
        private static bool CheckIfOkayToSpawnDialogue(JCS_DialogueType type)
        {
            // Force Diaglogue have higher priority, 
            // so it will block the lower priority dialogue type
            if (JCS_UIManager.instance.GetJCSDialogue(type) != null)
            {
                JCS_GameErrors.JcsErrors("JCS_ButtonFunctions", -1, "(" + type.ToString() + ")No able to spawn Dialogue cuz there are multiple dialogue in the scene...");
                return false;
            }


            if (JCS_UIManager.instance.GetJCSCanvas() == null)
            {
                JCS_GameErrors.JcsErrors("JCS_ButtonFunctions", -1, "No able to spawn Dialogue cuz Canvas are null...");
                return false;
            }

            return true;
        }

        public static void PopJCSBlackScreen()
        {
            string path = JCS_GameSettings.BLACK_SCREEN_PATH;
            JCS_BlackScreen bs = JCS_UsefualFunctions.SpawnGameObject(path).GetComponent<JCS_BlackScreen>();

            if (bs == null)
            {
                JCS_GameErrors.JcsErrors("JCS_ButtonFunctions", -1, "GameObject without \"JCS_BlackScreen\" Component attached!!!");
                return;
            }

            JCS_SceneManager.instance.SetJCSBlackScreen(bs);
        }

        public static void PopJCSWhiteScreen()
        {
            string path = JCS_GameSettings.WHITE_SCREEN_PATH;
            JCS_WhiteScreen ws = JCS_UsefualFunctions.SpawnGameObject(path).GetComponent<JCS_WhiteScreen>();

            if (ws == null)
            {
                JCS_GameErrors.JcsErrors("JCS_ButtonFunctions", -1, "GameObject without \"JCS_WhiteScreen\" Component attached!!!");
                return;
            }

            JCS_SceneManager.instance.SetJCSWhiteScreen(ws);
        }

        //** Game UI (Game Layer)
        public static void PopInGameUI()
        {
            if (!CheckIfOkayToSpawnGameUI(JCS_DialogueType.GAME_UI))
                return;

            //string path = JCS_GameSettings.instance.GAME_UI_PATH;
            //JCS_UsefualFunctions.SpawnGameObject(path);
        }

        //** (Application Layer)
        public static void PopIsConnectDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.SYSTEM_DIALOGUE))
                return;

            JCS_UsefualFunctions.SpawnGameObject(IS_CONNECT_DIALOGUE);

            PauseGame(true);
        }

        //** In Game Dialogue (Game Layer)
        public static void PopSettingDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_UsefualFunctions.SpawnGameObject(SETTING_PANEL);

            //PauseGame(true);
        }

        public static void PopTalkDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_UsefualFunctions.SpawnGameObject(TALK_DIALOGUE);

            PauseGame(true);
        }


    }
}
