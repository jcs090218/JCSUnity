/**
 * $File: JCS_UtilityFunctions.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

// Starting Version 5.3.0 Unity use SceneManageement 
// instead of Application on load function call.
#if (UNITY_5_3_OR_NEWER)
using UnityEngine.SceneManagement;
#endif


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_UtilityFunctions 
        : MonoBehaviour
    {
        private static string IS_CONNECT_DIALOGUE = "JCSUnity_Resources/JCS_ForceDialogue/JCS_MessageBox_YesNo";
        private static string SETTING_PANEL = "JCSUnity_Resources/JCS_InGameDialogue/Setting_Panel";
        private static string TALK_DIALOGUE = "JCSUnity_Resources/JCS_InGameDialogue/Conversation_Dialogue";

        /// <summary>
        /// Pause the game.
        /// </summary>
        /// <param name="act"></param>
        public static void PauseGame(bool act = true)
        {
            JCS_GameManager.instance.GAME_PAUSE = act;
        }

        /// <summary>
        /// pause the app.
        /// </summary>
        /// <param name="act"></param>
        public static void PasueApplication(bool act = true)
        {
            JCS_ApplicationManager.APP_PAUSE = act;
        }

        /// <summary>
        /// Open an url in defualt 
        /// JCS_NetworkConstant.OFFICIAL_WEBSITE constant.
        /// </summary>
        public static void ToOfficailWebpage()
        {
            ToOfficailWebpage(JCS_NetworkConstant.OFFICIAL_WEBSITE);
        }

        /// <summary>
        /// Open an url.
        /// </summary>
        public static void ToOfficailWebpage(string url)
        {
            Application.OpenURL(url);
        }

        /// <summary>
        /// Close the application directly.
        /// </summary>
        public static void QuitApplication()
        {
#if (UNITY_EDITOR)
            SceneManager.LoadScene("JCS_ApplicationCloseSimulateScene");
            JCS_ApplicationManager.APP_PAUSE = true;
#endif

            Application.Quit();
        }

        /// <summary>
        /// This will switch the scene then close the application.
        /// </summary>
        public static void QuitApplicationWithSwithScene()
        {
            JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulateScene");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckIfOkayToSpawnGameUI(JCS_DialogueType type)
        {
            if (JCS_UIManager.instance.GetJCSDialogue(type) != null)
            {
                JCS_Debug.LogError("JCS_UtilityFunctions",   "(" + type.ToString() + ")No able to spawn Game UI cuz there are multiple GameUI in the scene...");
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
                JCS_Debug.LogError("JCS_UtilityFunctions",   "(" + type.ToString() + ")No able to spawn Dialogue cuz there are multiple dialogue in the scene...");
                return false;
            }


            if (JCS_Canvas.instance == null)
            {
                JCS_Debug.LogError("JCS_UtilityFunctions",   "No able to spawn Dialogue cuz Canvas are null...");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopJCSBlackScreen()
        {
            string path = JCS_GameSettings.BLACK_SCREEN_PATH;
            JCS_BlackScreen bs = JCS_Utility.SpawnGameObject(path).GetComponent<JCS_BlackScreen>();

            if (bs == null)
            {
                JCS_Debug.LogError("JCS_UtilityFunctions",   "GameObject without \"JCS_BlackScreen\" Component attached!!!");
                return;
            }

            JCS_SceneManager.instance.SetJCSBlackScreen(bs);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopJCSBlackSlideScreen()
        {
            string path = JCS_GameSettings.BLACK_SLIDE_SCREEN_PATH;
            JCS_BlackSlideScreen bs = JCS_Utility.SpawnGameObject(path).GetComponent<JCS_BlackSlideScreen>();

            if (bs == null)
            {
                JCS_Debug.LogError("JCS_UtilityFunctions", "GameObject without \"JCS_BlackScreen\" Component attached!!!");
                return;
            }

            JCS_SceneManager.instance.SetJCSBlackSlideScreen(bs);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopJCSWhiteScreen()
        {
            string path = JCS_GameSettings.WHITE_SCREEN_PATH;
            JCS_WhiteScreen ws = JCS_Utility.SpawnGameObject(path).GetComponent<JCS_WhiteScreen>();

            if (ws == null)
            {
                JCS_Debug.LogError("JCS_UtilityFunctions",   "GameObject without \"JCS_WhiteScreen\" Component attached!!!");
                return;
            }

            JCS_SceneManager.instance.SetJCSWhiteScreen(ws);
        }

        //** Game UI (Game Layer)

        /// <summary>
        /// 
        /// </summary>
        public static void PopInGameUI()
        {
            if (!CheckIfOkayToSpawnGameUI(JCS_DialogueType.GAME_UI))
                return;

            //string path = JCS_GameSettings.instance.GAME_UI_PATH;
            //JCS_Utility.SpawnGameObject(path);
        }

        //** (Application Layer)

        /// <summary>
        /// 
        /// </summary>
        public static void PopIsConnectDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.SYSTEM_DIALOGUE))
                return;

            JCS_Utility.SpawnGameObject(IS_CONNECT_DIALOGUE);

            PauseGame(true);
        }

        //** In Game Dialogue (Game Layer)

        /// <summary>
        /// 
        /// </summary>
        public static void PopSettingDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_Utility.SpawnGameObject(SETTING_PANEL);

            //PauseGame(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PopTalkDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_Utility.SpawnGameObject(TALK_DIALOGUE);

            PauseGame(true);
        }


    }
}
