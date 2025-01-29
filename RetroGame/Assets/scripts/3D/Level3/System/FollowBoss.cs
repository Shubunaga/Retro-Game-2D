using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class FollowBoss : MonoBehaviour
    {
        public Transform targetObject; // The object to follow
        private Vector3 offset; // The initial offset
        private float initialY; // The initial Y position

        void Start()
        {
            // Calculate the initial offset.
            offset = transform.position - targetObject.position;
            // Store the initial Y position.
            initialY = transform.position.y;
        }

        void LateUpdate()
        {
            // Update the position of the object to follow the target
            Vector3 newPosition = new Vector3(targetObject.position.x, initialY, targetObject.position.z);
            transform.position = newPosition;
            // Update the rotation of the object to match the target
            Quaternion newRotation = Quaternion.Euler(targetObject.rotation.eulerAngles.x, targetObject.rotation.eulerAngles.y - 180, targetObject.rotation.eulerAngles.z);
            transform.rotation = newRotation;
        }
    } 
}
