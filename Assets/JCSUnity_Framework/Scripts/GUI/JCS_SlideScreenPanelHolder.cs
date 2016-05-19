/**
 * $File: JCS_SlideScreenPanelHolder.cs $
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

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(JCS_IgnoreDialogueObject))]
    public class JCS_SlideScreenPanelHolder
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        [Header("** Initialize Variables **")]
        [SerializeField] private float mSlideFrictionX = 0.2f;
        [SerializeField] private float mSlideFrictionY = 0.2f;

        [SerializeField] private RectTransform[] mSlidePanels = null;
        private JCS_SlidePanel[] mSlidePanelsComponent = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float SlideFrictionX { get { return this.mSlideFrictionX; } set { this.mSlideFrictionX = value; } }
        public float SlideFrictionY { get { return this.mSlideFrictionY; } set { this.mSlideFrictionY = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            // apply default value
            if (mSlideFrictionX == 0)
                mSlideFrictionX = 0.2f;
            if (mSlideFrictionY == 0)
                mSlideFrictionY = 0.2f;

            mSlidePanelsComponent = new JCS_SlidePanel[mSlidePanels.Length];

            for (int index = 0;
                index < mSlidePanels.Length;
                ++index)
            {
                // add the component to the slide panel
                mSlidePanelsComponent[index] =
                    mSlidePanels[index].gameObject.AddComponent<JCS_SlidePanel>();

                // set friction
                mSlidePanelsComponent[index].SlideFrictionX = mSlideFrictionX;
                mSlidePanelsComponent[index].SlideFrictionY = mSlideFrictionY;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void AddForce(Vector3 pos)
        {
            foreach (JCS_SlidePanel sp in mSlidePanelsComponent)
            {
                Vector3 tempPos = sp.GetTargetPosition() - pos;
                sp.SetTargetPosition(tempPos);
            }
        }
        public void AddForce(float force, JCS_Axis axis)
        {
            if (force == 0.0f)
                return;

            Vector3 tempPos = Vector3.zero;

            switch (axis)
            {
                case JCS_Axis.AXIS_X:
                    tempPos.x = force;
                    break;
                case JCS_Axis.AXIS_Y:
                    tempPos.y = force;
                    break;
                case JCS_Axis.AXIS_Z:
                    tempPos.z = force;
                    break;

            }

            AddForce(tempPos);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
