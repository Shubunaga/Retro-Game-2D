using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Level3
{
    public class CamRigLevel3 : MonoBehaviour
    {
        public Transform player; // Reference to the player object (set in Inspector)
        public Vector3 localOffset; // Offset from player in local space (set in Inspector)

        void Update()
        {
            if (player != null) // Check if player reference is assigned
            {
                // Apply offset in world space considering player's position and rotation
                transform.position = player.transform.position + player.transform.rotation * localOffset;
            }
        }
    }
}
