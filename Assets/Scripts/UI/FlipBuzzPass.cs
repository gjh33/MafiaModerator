using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class FlipBuzzPass : BaseGamePhase
    {
        public GameObject TouchOverlay;
        public float OrientationThreshold = 35f;

        protected override void OnBegin()
        {
            TouchOverlay.SetActive(true);
            StartCoroutine(PassCoroutine());
        }

        protected override void OnComplete()
        {
            TouchOverlay.SetActive(false);
        }

        public IEnumerator PassCoroutine()
        {
            if (!SystemInfo.supportsGyroscope)
            {
                yield return new WaitForSecondsRealtime(5);
                yield break;
            }
            yield return new WaitUntil(() => DeviceIsUpsideDown(OrientationThreshold));
            yield return new WaitForSecondsRealtime(3);
            Handheld.Vibrate();
            yield return new WaitUntil(() => DeviceIsRightSideUp(OrientationThreshold));
            Complete();
        }

        private bool DeviceIsUpsideDown(float threshold)
        {
            return Vector3.Angle(Input.gyro.gravity, Vector3.forward) <= threshold;
        }

        private bool DeviceIsRightSideUp(float threshold)
        {
            return Vector3.Angle(Input.gyro.gravity, Vector3.back) <= threshold;
        }
    }
}
