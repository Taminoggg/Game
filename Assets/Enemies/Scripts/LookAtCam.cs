using UnityEngine;

namespace Enemies.Scripts
{
    public class LookAtCam : MonoBehaviour
    {
        public Transform cam;

        private void Awake()
        {
            cam = GameObject.Find("Main Camera").transform;
        }

        void LateUpdate()
        {
            transform.LookAt(cam);
        }
    }
}
