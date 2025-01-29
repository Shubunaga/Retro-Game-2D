using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Level3
{
    public class BrigdeRotate : MonoBehaviour
    {
        public float rotationSpeed = 2f;
        private Transform bridge;
        private Rigidbody platformRigidbody;
        private Vector3 lastPosition;
        private Vector3 playerOffset; // Stores player's offset relative to platform center
        CharacterController cc;
        // Start is called before the first frame update
        void Start()
        {
            bridge = GetComponent<Transform>();
            platformRigidbody = GetComponent<Rigidbody>();
            // Calculate player offset on entering platform (assuming initial contact is on top)
            if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    playerOffset = hit.point - transform.position;
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Quaternion targetRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            platformRigidbody.MoveRotation(bridge.rotation * targetRotation);
            // If player is on platform, adjust their position based on rotation
            if (cc != null)
            {
                Vector3 playerPositionRelativeToPlatform = bridge.InverseTransformPoint(cc.transform.position);
                playerPositionRelativeToPlatform = targetRotation * playerPositionRelativeToPlatform;
                cc.transform.position = bridge.TransformPoint(playerPositionRelativeToPlatform);
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player") // Check if colliding with player
            {
                cc = other.gameObject.GetComponent<CharacterController>();
                other.GetComponent<PlayerControl>().inBridge = true;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player") // Check if player leaving
            {
                cc.Move(platformRigidbody.velocity * Time.deltaTime);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player") // Check if colliding with player
            {
                cc = null;
                other.GetComponent<PlayerControl>().inBridge = false;
            }
        }
    }

}