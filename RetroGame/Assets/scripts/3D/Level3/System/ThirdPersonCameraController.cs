using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Level3
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        public bool camMode = true;
        public Transform[] prePositions;
        private int currentPositionIndex = 0;
        public Transform target;
        public float distance = 5.0f;
        public float height = 2.0f;
        public float damping = 5.0f;
        public bool smoothRotation = true;
        public float rotationDamping = 10.0f;
        public float deadZone = 0.3f; // Adicione um valor para a "zona morta"
        public float minDistance = 3.0f; // Adicione uma distância mínima
        public float maxDistance = 10.0f; // Adicione uma distância máxima
        private Vector3 lastPlayerPosition;
        private float timeSinceLastMove;
        private float lastArrowKeyDownTime;
        private Vector3 lastTargetPosition;
        private float timeToMove;

        private Vector3 wantedPosition;
        private Coroutine moveCoroutine;
        private Vector3 currentPosition;
        private bool isPlayerStopped;

        void Update()
        {
            wantedPosition = target.TransformPoint(0, height, -distance);
            currentPosition = transform.position;
            

            if (target.position != lastPlayerPosition)
            {
                lastPlayerPosition = target.position;
                timeSinceLastMove = 0;
                isPlayerStopped = false;
            }
            else
            {
                timeSinceLastMove += Time.deltaTime;
                if(timeSinceLastMove > 2.0f)
                    isPlayerStopped= true;
            }
            ManualControl();
            if (Time.time > lastArrowKeyDownTime + 2.0f && !isPlayerStopped)
            {
                StopAllCoroutines();
                AutoControl();
            }
        }

        private void AutoControl()
        {
            camMode = true;
            // Verifique se o personagem se moveu para fora da "zona morta"
            if (Vector3.Distance(lastTargetPosition, target.position) > deadZone)
            {
                currentPosition = Vector3.Lerp(currentPosition, wantedPosition, Time.deltaTime * damping);
                lastTargetPosition = target.position;
                timeToMove = Time.time + 0.1f; // Adicione um atraso para a câmera começar a se mover
            }

            if (Time.time > timeToMove)
            {
                Vector3 targetPosition = Vector3.Lerp(currentPosition, target.position + new Vector3(0, height, 0), Time.deltaTime * damping);
                // Verifique se a câmera não está muito perto do personagem
                if (Vector3.Distance(targetPosition, target.position) > minDistance)
                {
                    currentPosition = targetPosition;
                }
                else
                {
                    Vector3 behindTarget = target.position - target.forward * minDistance + new Vector3(0, height, 0);
                    currentPosition = Vector3.Lerp(currentPosition, behindTarget, Time.deltaTime * damping);
                }
            }

            // Verifique se o personagem está muito longe da câmera
            if (Vector3.Distance(currentPosition, target.position) > maxDistance)
            {
                currentPosition = Vector3.Lerp(currentPosition, target.position + new Vector3(0, height, 0), Time.deltaTime * damping);
            }

            transform.position = Vector3.Lerp(transform.position, currentPosition, Time.deltaTime * damping);

            // Verifique se o jogador está abaixo da câmera
            if (target.position.y < transform.position.y)
            {
                if (smoothRotation)
                {
                    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                }
                else transform.LookAt(target, target.up);
            }
            if (target.position.y >= transform.position.y)
            {
                // Keep the current horizontal position, but update the vertical position to be higher than the character
                // Use a different damping value for faster vertical movement
                float verticalDamping = damping * 3; // Adjust this value as needed
                float newY = Mathf.Lerp(currentPosition.y, target.position.y + height, Time.deltaTime * verticalDamping);
                currentPosition = new Vector3(currentPosition.x, newY, currentPosition.z);
                // Look at the target again
                if (smoothRotation)
                {
                    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                }
                else transform.LookAt(target, target.up);
            }
                CheckCameraCollision();
        }

        private void CheckCameraCollision()
        {
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                // If the linecast hits something, set the camera's position to the hit point
                transform.position = hit.point;
            }
        }

        void ManualControl()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                camMode = false;
                currentPositionIndex--;
                if (currentPositionIndex < 0)
                {
                    currentPositionIndex = prePositions.Length - 1;
                }
                StartMoveToPosition(prePositions[currentPositionIndex].position, prePositions[currentPositionIndex].rotation);
                lastArrowKeyDownTime = Time.time;
                CheckCameraCollision();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                camMode = false;
                currentPositionIndex++;
                if (currentPositionIndex >= prePositions.Length)
                {
                    currentPositionIndex = 0;
                }
                StartMoveToPosition(prePositions[currentPositionIndex].position, prePositions[currentPositionIndex].rotation);
                lastArrowKeyDownTime = Time.time;
                CheckCameraCollision();
            }
            // Verifique se o personagem se moveu para fora da "zona morta"
            if (Vector3.Distance(lastTargetPosition, target.position) > deadZone)
            {
                currentPosition = Vector3.Lerp(currentPosition, wantedPosition, Time.deltaTime * damping);
                lastTargetPosition = target.position;
                timeToMove = Time.time + 0.1f; // Adicione um atraso para a câmera começar a se mover
            }
        }

        void StartMoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
        {
            // If a move coroutine is already running, stop it
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            // Start a new move coroutine
            moveCoroutine = StartCoroutine(MoveToPosition(targetPosition, targetRotation));
        }

        IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
        {
            float timeSinceStarted = 0f;
            while (true)
            {
                timeSinceStarted += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPosition, timeSinceStarted);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, timeSinceStarted);

                // If the object has arrived, stop the coroutine
                if (transform.position == targetPosition && transform.rotation == targetRotation)
                {
                    yield break;
                }

                // Otherwise, continue next frame
                yield return null;
            }
        }
    }
}