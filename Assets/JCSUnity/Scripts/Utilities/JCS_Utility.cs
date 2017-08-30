/**
 * $File: JCS_Utility.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using PeterVuorela.Tweener;
using System.Collections.Generic;


namespace JCSUnity
{
    // Function pointer
    public delegate int JCS_Range(int min, int max);
    public delegate void EventTriggerEvent(PointerEventData data);

    /// <summary>
    /// All the utility function put here.
    /// </summary>
    public class JCS_Utility
        : MonoBehaviour
    {

        /// <summary>
        /// Spawn a gmae object.
        /// </summary>
        /// <param name="objectPath"> path of the game object </param>
        /// <param name="position"> position of the game object spawn </param>
        /// <param name="rotation"> rotation of the game object spawn </param>
        /// <returns></returns>
        public static GameObject SpawnGameObject(string objectPath, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            return Instantiate(Resources.Load<GameObject>(objectPath), position, rotation) as GameObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Object SpawnGameObject(Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (trans == null)
                return null;

            return Instantiate(trans, position, rotation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string HeaderDecorator(string desc)
        {
            return "** " + desc + " **";
        }

        /// <summary>
        /// Check the value within the range plus acceptable range.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="acceptRange"></param>
        /// <param name="currentVal"></param>
        /// <returns></returns>
        public static bool WithInAcceptRange(float range, float acceptRange, float currentVal)
        {
            return WithInRange(range - acceptRange, range + acceptRange, currentVal);
        }

        /// <summary>
        /// Check the value within the range.
        /// </summary>
        /// <param name="minRange"></param>
        /// <param name="maxRange"></param>
        /// <param name="currentVal"></param>
        /// <returns></returns>
        public static bool WithInRange(float minRange, float maxRange, float currentVal)
        {
            if (currentVal >= minRange &&
                currentVal <= maxRange)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Vector2 GetImageRect(Image img)
        {
            RectTransform rt = img.transform.GetComponent<RectTransform>();
            if (rt == null)
            {
                JCS_Debug.LogError("JCS_UsefulFunctions", "No RectTransform on ur image!");
                return Vector2.one;
            }

            float width = rt.sizeDelta.x * rt.localPosition.x;
            float height = rt.sizeDelta.y * rt.localPosition.y;

            return new Vector2(width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Vector2 GetSpriteRendererRectWithNoScale(SpriteRenderer sr)
        {
            float width = sr.bounds.extents[0] * 2;
            float height = sr.bounds.extents[1] * 2;

            return new Vector2(width, height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Vector2 GetSpriteRendererRect(SpriteRenderer sr)
        {
            float width = sr.bounds.extents[0] * 2 * sr.transform.localScale.x;
            float height = sr.bounds.extents[1] * 2 * sr.transform.localScale.y;

            return new Vector2(width, height);
        }

        /// <summary>
        /// Solve the flash problem! (JCS_CheckableObject)
        /// 
        /// Check if the mouse still on top of the image!
        /// </summary>
        /// <returns></returns>
        public static bool MouseOverGUI(RectTransform imageRect, RectTransform rootPanel = null)
        {
            Vector2 mousePos = JCS_Input.MousePositionOnGUILayer();
            Vector2 checkPos = imageRect.localPosition;

            if (rootPanel != null)
                checkPos += new Vector2(rootPanel.localPosition.x, rootPanel.localPosition.y);

            // this item image size
            Vector2 slotRect = imageRect.sizeDelta;

            float halfSlotWidth = slotRect.x / 2 * imageRect.localScale.x;
            float halfSlotHeight = slotRect.y / 2 * imageRect.localScale.y;

            float leftBorder = checkPos.x - halfSlotWidth;
            float rightBorder = checkPos.x + halfSlotWidth;
            float topBorder = checkPos.y + halfSlotHeight;
            float bottomBorder = checkPos.y - halfSlotHeight;

            // Basic AABB collide math
            if (mousePos.x <= rightBorder &&
                mousePos.x >= leftBorder &&
                mousePos.y <= topBorder &&
                mousePos.y >= bottomBorder)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Spawn an animate object.
        /// </summary>
        /// <param name="anim"> anim assign </param>
        /// <param name="orderLayer"> sorting order </param>
        /// <returns> object spawned. </returns>
        public static GameObject SpawnAnimateObject(RuntimeAnimatorController anim, int orderLayer = 15)
        {
            GameObject gm = new GameObject();

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = orderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = anim;

            return gm;
        }

        /// <summary>
        /// Spawn a animate object with the death event, 
        /// so after the animate was played serval loop times
        /// the object will be destroyed.
        /// </summary>
        /// <param name="anim"> animation u want to play </param>
        /// <param name="orderLayer"> sorting order  </param>
        /// <param name="loopTimes"> loop times </param>
        /// <returns> object spawned. </returns>
        public static GameObject SpawnAnimateObjectDeathEvent(RuntimeAnimatorController anim, int orderLayer = 15, int loopTimes = 1)
        {
            GameObject gm = SpawnAnimateObject(anim, orderLayer);

            JCS_DestroyAnimEndEvent daee = gm.AddComponent<JCS_DestroyAnimEndEvent>();

            daee.LoopTimes = loopTimes;

            return gm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 SetVecX(ref Vector3 val, float x)
        {
            return SetVec3(ref val, x, val.y, val.z);
        }
        public static Vector3 SetVecY(ref Vector3 val, float y)
        {
            return SetVec3(ref val, val.x, y, val.z);
        }
        public static Vector3 SetVecZ(ref Vector3 val, float z)
        {
            return SetVec3(ref val, val.x, val.y, z);
        }
        public static Vector3 SetVec3(ref Vector3 val, float x, float y, float z)
        {
            Vector3 newVec = val;

            newVec.x = x;
            newVec.y = y;
            newVec.z = z;

            val = newVec;

            return newVec;
        }
        public static Vector3 IncVecX(Vector3 val, float x)
        {
            return IncVec3(val, x, 0, 0);
        }
        public static Vector3 IncVecY(Vector3 val, float y)
        {
            return IncVec3(val, 0, y, 0);
        }
        public static Vector3 IncVecZ(Vector3 val, float z)
        {
            return IncVec3(val, 0, 0, z);
        }
        public static Vector3 IncVec3(Vector3 val, float x, float y = 0, float z = 0)
        {
            Vector3 newVec = val;

            newVec.x += x;
            newVec.y += y;
            newVec.z += z;

            val = newVec;

            return newVec;
        }

        /// <summary>
        /// Active all the child in a transform.
        /// </summary>
        /// <param name="trans"> transform to do the effect. </param>
        /// <param name="act"> action to the effect </param>
        public static void SetActiveToAllChildren(Transform trans, bool act)
        {
            Transform child = null;

            for (int index = 0;
                index < trans.childCount;
                ++index)
            {
                child = trans.GetChild(index);
                child.gameObject.SetActive(act);
            }
        }

        /// <summary>
        /// Move the object to the last child of the Unty's
        /// tree system(Hierarchy) transform.
        /// </summary>
        /// <param name="trans"></param>
        public static void MoveToTheLastChild(Transform trans)
        {
            Transform parent = trans.parent;

            Vector3 recordPos = trans.localPosition;
            Vector3 recordScale = trans.localScale;
            Quaternion recordRot = trans.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                trans.SetParent(null);
                trans.SetParent(parent);
            }

            // here we set it back!
            trans.localPosition = recordPos;
            trans.localScale = recordScale;
            trans.localRotation = recordRot;
        }

        /// <summary>
        /// Set the transform to onther transform in the
        /// hierarchy and without losing the info.
        /// Info: 
        ///     position, rotation, scale, etc.
        /// </summary>
        /// <param name="trans"> transform to set to parent transform </param>
        /// <param name="parent"> parent transform </param>
        public static void SetParentWithoutLosingInfo(Transform trans, Transform parent)
        {
            Vector3 recordPos = trans.localPosition;
            Vector3 recordScale = trans.localScale;
            Quaternion recordRot = trans.localRotation;

            {
                trans.SetParent(parent);
            }

            // here we set it back!
            trans.localPosition = recordPos;
            trans.localScale = recordScale;
            trans.localRotation = recordRot;
        }

        /// <summary>
        /// Add Event to Unity's Event Trigger(Script)
        /// </summary>
        /// <param name="te"></param>
        /// <param name="type"></param>
        /// <param name="func"></param>
        public static void AddEventTriggerEvent(EventTrigger te, EventTriggerType type, EventTriggerEvent func)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { func((PointerEventData)data); });
            te.triggers.Add(entry);
        }

        /// <summary>
        /// Return direction of Unity's Vector system.
        /// </summary>
        /// <param name="direction"> direction u wants. </param>
        /// <returns> vector value. </returns>
        public static Vector3 VectorDirection(JCS_Vector3Direction direction)
        {
            switch (direction)
            {
                case JCS_Vector3Direction.CENTER:
                    return Vector3.zero;

                case JCS_Vector3Direction.UP:
                    return Vector3.up;
                case JCS_Vector3Direction.DOWN:
                    return Vector3.down;

                case JCS_Vector3Direction.FORWARD:
                    return Vector3.forward;
                case JCS_Vector3Direction.BACK:
                    return Vector3.back;

                case JCS_Vector3Direction.RIGHT:
                    return Vector3.right;
                case JCS_Vector3Direction.LEFT:
                    return Vector3.left;

                case JCS_Vector3Direction.FORWARD_LEFT:
                    return new Vector3(-1, 0, 1);
                case JCS_Vector3Direction.FORWARD_RIGHT:
                    return new Vector3(1, 0, 1);

                case JCS_Vector3Direction.BACK_LEFT:
                    return new Vector3(-1, 0, -1);
                case JCS_Vector3Direction.BACK_RIGHT:
                    return new Vector3(1, 0, -1);

                case JCS_Vector3Direction.UP_LEFT:
                    return new Vector3(-1, 1, 0);
                case JCS_Vector3Direction.UP_RIGHT:
                    return new Vector3(1, 1, 0);

                case JCS_Vector3Direction.FORWARD_UP:
                    return new Vector3(0, 1, 1);
                case JCS_Vector3Direction.FORWARD_DOWN:
                    return new Vector3(0, -1, 1);

                case JCS_Vector3Direction.BACK_UP:
                    return new Vector3(0, 1, -1);
                case JCS_Vector3Direction.BACK_DOWN:
                    return new Vector3(0, -1, -1);

                case JCS_Vector3Direction.DOWN_LEFT:
                    return new Vector3(-1, -1, 0);
                case JCS_Vector3Direction.DOWN_RIGHT:
                    return new Vector3(1, -1, 0);

                case JCS_Vector3Direction.FORWARD_UP_LEFT:
                    return new Vector3(-1, 1, 1);
                case JCS_Vector3Direction.FORWARD_UP_RIGHT:
                    return new Vector3(1, 1, 1);

                case JCS_Vector3Direction.FORWARD_DOWN_LEFT:
                    return new Vector3(-1, -1, 1);
                case JCS_Vector3Direction.FORWARD_DOWN_RIGHT:
                    return new Vector3(1, -1, 1);

                case JCS_Vector3Direction.BACK_UP_LEFT:
                    return new Vector3(-1, 1, -1);
                case JCS_Vector3Direction.BACK_UP_RIGHT:
                    return new Vector3(1, 1, -1);

                case JCS_Vector3Direction.BACK_DOWN_LEFT:
                    return new Vector3(-1, -1, -1);
                case JCS_Vector3Direction.BACK_DOWN_RIGHT:
                    return new Vector3(1, -1, -1);
            }

            JCS_Debug.LogError(
                "JCS_Utility",

                "This cannot happed.");

            // this cannot happens
            return Vector3.zero;
        }

        /// <summary>
        /// Add random value to Vector3
        /// </summary>
        /// <param name="trans"> transfrorm u want to apply. </param>
        /// <param name="randVec"> value for each axis. </param>
        /// <param name="checks"> check for eaxh axis. </param>
        /// <returns> transform result. </returns>
        public static Vector3 ApplyRandVector3(Vector3 trans, Vector3 randVec, JCS_Bool3 checks)
        {
            Vector3 tempVec = trans;

            if (checks.check1)
            {
                float val = JCS_Random.Range(-randVec.x, randVec.x);
                tempVec = IncVecX(tempVec, val);
            }

            if (checks.check2)
            {
                float val = JCS_Random.Range(-randVec.y, randVec.y);
                tempVec = IncVecY(tempVec, val);
            }

            if (checks.check3)
            {
                float val = JCS_Random.Range(-randVec.z, randVec.z);
                tempVec = IncVecZ(tempVec, val);
            }

            trans = tempVec;

            return tempVec;
        }


        /// <summary>
        /// Check if the object are the same tribe.
        /// </summary>
        /// <param name="liveObj1"> obj one </param>
        /// <param name="liveObj2"> obj two </param>
        /// <returns>
        /// true: same tribe
        /// false: not the same tribe
        /// </returns>
        public static bool IsSameTribe(JCS_2DLiveObject liveObj1, JCS_2DLiveObject liveObj2)
        {
            if (liveObj1 == null || liveObj2 == null)
                return false;

            // if both player does not need to add in to list.
            // or if both enemy does not need to add in to list.
            return (liveObj1.IsPlayer == liveObj2.IsPlayer);
        }

        /// <summary>
        /// Enum typed version casting.
        /// Source: http://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values
        /// </summary>
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Set eabled/disabled to all component in a transform.
        /// </summary>
        /// <param name="trans"> transform to apply the effect. </param>
        /// <param name="act"> enable or disable? </param>
        public static void SetEnableAllComponents(Transform trans, bool act)
        {
            foreach (var component in trans.GetComponents<MonoBehaviour>())
            {
                component.enabled = act;
            }
        }

        /// <summary>
        /// Destroy all the 'TYPE' object in the scene.
        /// </summary>
        public static void DestroyAllTypeObjectInScene<T>()
            where T : MonoBehaviour
        {
            // Destroy all the live object in the scene.
            T[] rrEnemy = Resources.FindObjectsOfTypeAll<T>();

            foreach (T e in rrEnemy)
            {
                // NOTE(JenChieh): kill the object that are clone!
                // or else it will effect the prefab object...
                if (e.gameObject.name.Contains("(Clone)"))
                    Destroy(e.gameObject);
            }
        }

        /// <summary>
        /// Find all the objects that are clone in the scene by type.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> Type array. </returns>
        public static T[] FindCloneObjectsOfTypeAllI<T>()
            where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (obj.gameObject.name.Contains("(Clone)"))
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        /// <summary>
        /// Find all the objects that are not clone in the scene by type.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> Type array. </returns>
        public static T[] FindNotCloneObjectsOfTypeAllI<T>()
            where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (!obj.gameObject.name.Contains("(Clone)"))
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        /// <summary>
        /// Find all objects that only ACTIVE in hierarchy.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> type array, object only in Hierarcrchy. </returns>
        public static T[] FindObjectsOfTypeAllInHierarchy<T>()
             where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        /// <summary>
        /// Return the easing function pointer base on tweener type.
        /// </summary>
        /// <param name="type"> type of the tween formula </param>
        /// <returns> function pointer </returns>
        public static TweenDelegate GetEasing(JCS_TweenType type)
        {
            TweenDelegate easing = null;

            switch (type)
            {
                // default to linear
                case JCS_TweenType.LINEAR:
                    easing = Easing.Linear;
                    break;

                case JCS_TweenType.EASE_IN_SINE:
                    easing = Easing.SineEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CUBIC:
                    easing = Easing.CubicEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUINT:
                    easing = Easing.QuintEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CIRC:
                    easing = Easing.CircEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BACK:
                    easing = Easing.BackEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CIRC:
                    easing = Easing.CircEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BACK:
                    easing = Easing.BackEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CIRC:
                    easing = Easing.CircEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BACK:
                    easing = Easing.BackEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_QUAD:
                    easing = Easing.QuadEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUART:
                    easing = Easing.QuartEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_EXPO:
                    easing = Easing.ExpoEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_ELASTIC:
                    easing = Easing.ElasticEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BOUNCE:
                    easing = Easing.BounceEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUART:
                    easing = Easing.QuartEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_ELASTIC:
                    easing = Easing.ElasticEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BOUNCE:
                    easing = Easing.BounceEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUART:
                    easing = Easing.QuartEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_ELASTIC:
                    easing = Easing.ElasticEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BOUNCE:
                    easing = Easing.BounceEaseInOut;
                    break;
            }

            return easing;
        }

        /// <summary>
        /// Remove the empty slot in the list.
        /// </summary>
        /// <typeparam name="T"> Type of the List. </typeparam>
        /// <param name="inList"> List object. </param>
        /// <returns> Cleaned up List object. </returns>
        public static List<T> RemoveEmptySlot<T>(List<T> inList)
        {
            List<T> newArray = new List<T>(inList.Count);

            for (int index = 0;
               index < inList.Count;
               ++index)
            {
                // remove itself.
                if (inList[index] != null)
                    newArray.Add(inList[index]);
            }

            return newArray;
        }


        /// <summary>
        /// Multiply all the parent localEulerAngles to get the correct 
        /// description of the transform information. 
        /// 
        /// ATTENTION(jenchieh): This will cause some performance, use 
        /// it wisely.
        /// </summary>
        /// <param name="trans"> transform we want to get from and use 
        /// it for parent. </param>
        /// <param name="inEulerAngles"> use to store the result. </param>
        /// <returns> Accumilate the result. </returns>
        public static Vector3 GetFinalLocalEulerAngles(Transform trans, ref Vector3 inEulerAngles)
        {
            if (trans.parent == null)
                return inEulerAngles;

            Vector3 parentVal = trans.parent.transform.localEulerAngles;

            inEulerAngles = SetVec3(
                ref inEulerAngles,
                parentVal.x * inEulerAngles.x,
                parentVal.y * inEulerAngles.y,
                parentVal.z * inEulerAngles.z);

            return GetFinalLocalEulerAngles(trans.parent, ref inEulerAngles);
        }

        /// <summary>
        /// Merge multiple arrays into one array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] MergeArrays<T>(params T[][] arrList)
        {
            if (arrList.Length <= 1)
            {
                JCS_Debug.Log(
                    "JCS_Utility",
                    "You trying to merge the array less then two array?");
            }

            int arrLen = 0;
            foreach (var arr in arrList)
                arrLen += arr.Length;

            // first combine the first two array.
            T[] data = MergeArrays2<T>(arrList[0], arrList[1]);

            // combine the rest.
            for (int index = 2;
                index < arrList.Length;
                ++index)
            {
                data = MergeArrays2<T>(data, arrList[index]);
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
        public static T[] MergeArrays2<T>(T[] arr1, T[] arr2)
        {
            T[] data = new T[arr1.Length + arr2.Length];

            System.Array.Copy(arr1, data, arr1.Length);
            System.Array.Copy(arr2, 0, data, arr1.Length, arr2.Length);

            return data;
        }

        /// <summary>
        /// Copy byte array to another byte array memory space.
        /// </summary>
        /// <param name="inBuf"> byte array to copy. </param>
        /// <param name="start"> Starting index to copy. </param>
        /// <param name="len"> Length to copy. </param>
        /// <returns> byte array that are created in new memroy space. </returns>
        public static byte[] CopyByteArray(byte[] inBuf, int start, int len)
        {
            byte[] bytes = new byte[len];

            for (int count = 0;
                count < len;
                ++count)
            {
                bytes[count] = inBuf[count];
            }

            return bytes;
        }

    }
}
