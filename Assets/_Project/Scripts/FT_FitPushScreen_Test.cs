/**
 * $File: FT_FitPushScreen_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_FitPushScreen_Test : MonoBehaviour 
{
    /* Variables */

    [SerializeField]
    private RectTransform mPanelRectTransform = null;


    /* Setter/Getter */

    /* Functions */

    private void Awake() 
    {
        
    }
    
    private void Update() 
    {
        

        if (JCS_Input.GetKey(KeyCode.D))
        {
            Vector3 newPos = this.mPanelRectTransform.localPosition;
            newPos.x += 10;
            this.mPanelRectTransform.localPosition = newPos;
        }
        if (JCS_Input.GetKey(KeyCode.A))
        {
            Vector3 newPos = this.mPanelRectTransform.localPosition;
            newPos.x -= 10;
            this.mPanelRectTransform.localPosition = newPos;
        }
        if (JCS_Input.GetKey(KeyCode.S))
        {
            Vector3 newPos = this.mPanelRectTransform.localPosition;
            newPos.y -= 10;
            this.mPanelRectTransform.localPosition = newPos;
        }
        if (JCS_Input.GetKey(KeyCode.W))
        {
            Vector3 newPos = this.mPanelRectTransform.localPosition;
            newPos.y += 10;
            this.mPanelRectTransform.localPosition = newPos;
        }

        FitPushScreen();
    }

    private void FitPushScreen()
    {
        Vector2 rectSize = mPanelRectTransform.sizeDelta;
        Vector3 panelPos = mPanelRectTransform.localPosition;

        float halfSlotWidth = rectSize.x / 2 * mPanelRectTransform.localScale.x;
        float halfSlotHeight = rectSize.y / 2 * mPanelRectTransform.localScale.y;

        float panelLeftBorder = panelPos.x - halfSlotWidth;
        float panelRightBorder = panelPos.x + halfSlotWidth;
        float panelTopBorder = panelPos.y + halfSlotHeight;
        float panelBottomBorder = panelPos.y - halfSlotHeight;

        Camera cam = JCS_Camera.main.GetCamera();
        Vector3 camPos = cam.transform.localPosition;

        Vector2 camPosToScreen = cam.WorldToScreenPoint(camPos);

        //RectTransform appRect = JCS_Canvas.instance.GetAppRect();
        //Vector2 screenRect = appRect.sizeDelta;
        Vector2 screenRect = new Vector2(JCS_Screen.width, JCS_Screen.height);

        float camLeftBorder = camPosToScreen.x - screenRect.x / 2;
        float camRightBorder = camPosToScreen.x + screenRect.x / 2;
        float camTopBorder = camPosToScreen.y + screenRect.y / 2;
        float camBottomBorder = camPosToScreen.y - screenRect.y / 2;



        Vector3 newShowPoint = this.mPanelRectTransform.localPosition;

        if (panelRightBorder > camRightBorder)
        {
            newShowPoint.x -= panelRightBorder - camRightBorder;
        }
        else if (panelLeftBorder < camLeftBorder)
        {
            newShowPoint.x -= panelLeftBorder - camLeftBorder;
        }

        if (panelTopBorder > camTopBorder)
        {
            newShowPoint.y -= panelTopBorder - camTopBorder;
        }
        else if (panelBottomBorder < camBottomBorder)
        {
            newShowPoint.y -= panelBottomBorder - camBottomBorder;
        }

        this.mPanelRectTransform.localPosition = newShowPoint;
    }
}
