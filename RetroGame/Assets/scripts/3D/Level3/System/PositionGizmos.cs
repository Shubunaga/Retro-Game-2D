using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class PositionGizmos : MonoBehaviour
    {
        public float sphereRadius = 0.5f;  // Adjust sphere size
        public Color gizmoColor = Color.red;  // Adjust color

        void OnDrawGizmos()
        {
            // Get all child Transforms recursively
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                // Skip the main gameObject (script attached object)
                if (child != transform)
                {
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawSphere(child.position, sphereRadius);
                }
            }
        }
    } 
}
