using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class FieldOfView : MonoBehaviour
    {
        public float radius;
        [Range(0, 360)]
        public float angle;

        public GameObject playerRef;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public bool canSeePlayer;
        public bool start;
        //FogField of view


        // Start is called before the first frame update
        void Start()
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(FOVRoutine());
            Debug.Log("Começo");
        }

        private IEnumerator FOVRoutine()
        {
            float delay = 0.1f;
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                //Debug.Log("Checks");
                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    //Debug.Log("Angle");
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        canSeePlayer = true;
                        //print("Vi player");
                    }
                    else
                        canSeePlayer = false;

                }
                else
                    canSeePlayer = false;
            }
            else if (canSeePlayer)
                canSeePlayer = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(!start)
                StartCoroutine(FOVRoutine()); start = true;
        }
    } 
}
