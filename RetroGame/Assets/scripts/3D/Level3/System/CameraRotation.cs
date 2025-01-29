using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace Level3
{
    public class CameraRotation : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        private CinemachineOrbitalTransposer orbital;
        public KeyCode rotateLeftKey = KeyCode.I;
        public KeyCode rotateRightKey = KeyCode.P;
        public float rotationSpeed = 10f;
        private bool isRotating = false;
        public bool camMode = false;

        void Start()
        {
            orbital = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        void Update()
        {
            if (Input.GetKey(rotateRightKey) || Input.GetKey(rotateLeftKey))
            {
                isRotating = true;
                orbital.m_RecenterToTargetHeading.m_enabled = false;

                if (Input.GetKey(rotateRightKey))
                {
                    orbital.m_XAxis.Value -= rotationSpeed * Time.deltaTime;
                    camMode= true;
                }
                else if (Input.GetKey(rotateLeftKey))
                {
                    orbital.m_XAxis.Value += rotationSpeed * Time.deltaTime;
                    camMode= true;
                }
            }
            else if (isRotating)
            {
                isRotating = false;
                StartCoroutine(DisableRecenteringForSeconds(2));
            }
        }

        IEnumerator DisableRecenteringForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            camMode= false;
            orbital.m_RecenterToTargetHeading.m_enabled = true;
        }
    }
}
