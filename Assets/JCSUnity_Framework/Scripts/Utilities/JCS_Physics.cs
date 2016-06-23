/**
 * $File: JCS_Physics.cs $
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

    public class JCS_Physics
        : MonoBehaviour
    {
        public static bool TopOfBox(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            if (cH < 0)
                cH = 0;

            float rTopBound = rect.transform.position.y + rectCenter.y + (rHeight / 2.0f);
            float cBotBound = cap.transform.position.y + cCenter.y - (cH / 2.0f) - cR;

#if (UNITY_EDITOR)
            Debug.DrawLine(rect.transform.position + rectCenter, 
                new Vector3(
                    rect.transform.position.x,
                    rTopBound,
                    rect.transform.position.z)
                );

            Debug.DrawLine(cap.transform.position + cCenter,
                new Vector3(
                    cap.transform.position.x,
                    cBotBound,
                    cap.transform.position.z)
                );
#endif

            if (rTopBound <= cBotBound)
                return true;

            return false;
        }

        public static bool BottomOfBox(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            if (cH < 0)
                cH = 0;

            float cTopBound = cap.transform.position.y + cCenter.y + (cH / 2.0f) + cR;
            float rBotBound = rect.transform.position.y + rectCenter.y - (rHeight / 2.0f);

#if (UNITY_EDITOR)
            Debug.DrawLine(rect.transform.position + rectCenter,
                new Vector3(
                    rect.transform.position.x,
                    rBotBound,
                    rect.transform.position.z)
                );

            Debug.DrawLine(cap.transform.position + cCenter,
                new Vector3(
                    cap.transform.position.x,
                    cTopBound,
                    cap.transform.position.z)
                );
#endif

            if (cTopBound <= rBotBound)
                return true;

            return false;
        }

        public static bool JcsOnTriggerCheck(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;
            float widthSqr = JCS_Mathf.Sqr(rWidth);
            float heightSqr = JCS_Mathf.Sqr(rHeight);
            float diagonal = Mathf.Sqrt(widthSqr + heightSqr);

            rWidth = JCS_Mathf.AbsoluteValue(rWidth);
            rHeight = JCS_Mathf.AbsoluteValue(rHeight);

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            float offsetY = cCenter.y;
            float cHalfHeight = cH / 2.0f;

            if (cH < 0)
                cH = 0;

            Vector3 topCirclePoint = new Vector3(
                cap.transform.position.x + cCenter.x,
                cap.transform.position.y + cHalfHeight + offsetY,
                cap.transform.position.z + cCenter.z);
            Vector3 botCirclePoint = new Vector3(
                cap.transform.position.x + cCenter.x,
                cap.transform.position.y - cHalfHeight + offsetY,
                cap.transform.position.z + cCenter.z);


            float maxDistX = cR + (rWidth / 2.0f);
            float maxDistY = ((cH / 2.0f) + cR) + (rHeight / 2.0f);

            float realDistX = JCS_Mathf.DistanceOfUnitVector(
                cap.transform.position.x + cCenter.x,
                rect.transform.position.x + rectCenter.x);
            float realDistY = JCS_Mathf.DistanceOfUnitVector(
                cap.transform.position.y + cCenter.y,
                rect.transform.position.y + rectCenter.y);

            if (realDistX <= maxDistX &&
                realDistY <= maxDistY)
            {
                Vector2 rectPosOffset = new Vector2(
                    rect.transform.position.x + rectCenter.x,
                    rect.transform.position.y + rectCenter.y
                    );

                Rect r = new Rect();
                r.x = rectPosOffset.x - (rWidth / 2.0f);
                r.y = rectPosOffset.y + (rHeight / 2.0f);
                r.width = rectPosOffset.x + (rWidth / 2.0f);
                r.height = rectPosOffset.y - (rHeight / 2.0f);

                Vector2 topLeft = new Vector2(r.x, r.y);
                Vector2 topRight = new Vector2(r.width, r.y);
                Vector2 botRight = new Vector2(r.width, r.height);
                Vector2 botLeft = new Vector2(r.x, r.height);

#if (UNITY_EDITOR)
                Debug.DrawLine(topLeft, topCirclePoint);
                Debug.DrawLine(topRight, topCirclePoint);
                Debug.DrawLine(botRight, topCirclePoint);
                Debug.DrawLine(botLeft, topCirclePoint);

                Debug.DrawLine(topLeft, botCirclePoint);
                Debug.DrawLine(topRight, botCirclePoint);
                Debug.DrawLine(botRight, botCirclePoint);
                Debug.DrawLine(botLeft, botCirclePoint);
#endif

                // check if point intersect with each other
                float distance = 0;

                // top circle
                distance = Vector2.Distance(topLeft, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(topRight, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botRight, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botLeft, topCirclePoint);
                if (distance <= cR)
                    return true;

                // bot circle
                distance = Vector2.Distance(topLeft, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(topRight, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botRight, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botLeft, botCirclePoint);
                if (distance <= cR)
                    return true;
            }


            return false;
        }
    }
}
