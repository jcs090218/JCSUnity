/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

namespace InfinityCode.OnlineMapsDemos
{
    public class Aircraft : MonoBehaviour
    {
        public GameObject container;
        public float altitude = 1000; // meters
        public float speed = 900; // km/h
        public Vector3 cameraOffset = new Vector3(-10, -3, 0);

        public float tiltSpeed = 1;

        private double px, py;
        public float tilt = 0;

        public float rotateSpeed = 1;

        private void Start()
        {
            OnlineMaps api = OnlineMaps.instance;

            Vector3 position = OnlineMapsTileSetControl.instance.GetWorldPosition(api.position);
            position.y = altitude *
                         OnlineMapsTileSetControl.instance.GetBestElevationYScale(api.topLeftPosition,
                             api.bottomRightPosition) * OnlineMapsTileSetControl.instance.elevationScale;
            gameObject.transform.position = position;

            api.GetPosition(out px, out py);
        }

        void Update()
        {
            const float maxTilt = 50;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                tilt -= Time.deltaTime * tiltSpeed * maxTilt;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                tilt += Time.deltaTime * tiltSpeed * maxTilt;
            }
            else if (tilt != 0)
            {
                float tiltOffset = Time.deltaTime * tiltSpeed * maxTilt;
                if (Mathf.Abs(tilt) > tiltOffset) tilt -= tiltOffset * Mathf.Sign(tilt);
                else tilt = 0;
            }

            tilt = Mathf.Clamp(tilt, -maxTilt, maxTilt);
            container.transform.localRotation = Quaternion.Euler(tilt, 0, 0);

            if (tilt != 0)
            {
                transform.Rotate(Vector3.up, tilt * rotateSpeed * Time.deltaTime);
            }

            OnlineMaps api = OnlineMaps.instance;

            double tlx, tly, brx, bry, dx, dy;

            api.GetTopLeftPosition(out tlx, out tly);
            api.GetBottomRightPosition(out brx, out bry);

            OnlineMapsUtils.DistanceBetweenPoints(tlx, tly, brx, bry, out dx, out dy);

            double mx = (brx - tlx) / dx;
            double my = (tly - bry) / dy;

            double v = (double)speed * Time.deltaTime / 3600.0;

            double ox = mx * v * Math.Cos(transform.rotation.eulerAngles.y * OnlineMapsUtils.Deg2Rad);
            double oy = my * v * Math.Sin((360 - transform.rotation.eulerAngles.y) * OnlineMapsUtils.Deg2Rad);

            px += ox;
            py += oy;

            api.SetPosition(px, py);

            Vector3 pos = transform.position;
            pos.y = altitude * OnlineMapsTileSetControl.instance.GetBestElevationYScale(api.topLeftPosition, api.bottomRightPosition) * OnlineMapsTileSetControl.instance.elevationScale;
            transform.position = pos;

            Camera.main.transform.position = transform.position - transform.rotation * cameraOffset;
            Camera.main.transform.LookAt(transform);
        }
    }
}