public static class RC_Glob
{
    /* Variables */

    public static RC_AppSettings apps => RC_AppSettings.FirstInstance();
    public static RC_GameSettings games => RC_GameSettings.FirstInstance();

    public static RC_GameManager gamem => RC_GameManager.FirstInstance();
    public static RC_UIManager uim => RC_UIManager.FirstInstance();

    /* Setter & Getter */

    /* Functions */

}
