using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RailShooter
{
    public class RailFollower : MonoBehaviour
    {
        [SerializeField] Transform player;
        [SerializeField] Transform followTarget;
        [SerializeField] float followDistance = 22f;
        [SerializeField] float smoothTime = 0.2f;
        Vector3 velocity;

        void Update()
        {
            Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;
            transform.position = Vector3.SmoothDamp(current: transform.position, targetPos, ref velocity, smoothTime);

            transform.rotation = player.rotation;
        }

    }
}
