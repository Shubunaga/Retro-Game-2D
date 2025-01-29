using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class StonesMovement : MonoBehaviour
    {
        public GameObject[] stones; // Array of your stones
        public GameObject[] stoneshalf; // Array of your stones
        public float moveSpeed = 2f; // Speed at which stones move
        public float moveDistance = 10f; // Distance stones move along their local Z axis
        public float waitTime = 4f; // Time to wait before moving the stone back
        public int total;
        private bool half = false;
        private int count = 0;
        void Start()
        {
            total = stones.Length;
            stoneshalf = stones;
            StartCoroutine(MoveStones());
        }

        IEnumerator MoveStones()
        {
            while (true) // Loop the movement
            {
                foreach (GameObject stone in stones)
                {
                    // Store the stone's initial position
                    Vector3 initialPosition = stone.transform.position;

                    // Calculate the stone's target position
                    Vector3 targetPosition = stone.transform.position + (-stone.transform.forward) * moveDistance;

                    // While the stone is not at the target position
                    while (stone.transform.position != targetPosition)
                    {
                        // Move the stone towards the target position
                        stone.transform.position = Vector3.MoveTowards(stone.transform.position, targetPosition, moveSpeed * Time.deltaTime);

                        // Wait for the next frame
                        yield return null;
                    }
                    count++;
                    if (count == total / 2 && !half)
                    {
                        half = true;
                        StartCoroutine(MoveStones2());
                    }
                    // Start the coroutine to move the stone back to its initial position
                    StartCoroutine(MoveStoneBack(stone, initialPosition));

                    // Wait for the stone to move back before moving the next stone
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }

        IEnumerator MoveStones2()
        {
            while (true) // Loop the movement
            {
                foreach (GameObject stone in stoneshalf)
                {
                    // Store the stone's initial position
                    Vector3 initialPosition = stone.transform.position;

                    // Calculate the stone's target position
                    Vector3 targetPosition = stone.transform.position + (-stone.transform.forward) * moveDistance;

                    // While the stone is not at the target position
                    while (stone.transform.position != targetPosition)
                    {
                        // Move the stone towards the target position
                        stone.transform.position = Vector3.MoveTowards(stone.transform.position, targetPosition, moveSpeed * Time.deltaTime);

                        // Wait for the next frame
                        yield return null;
                    }
                    // Start the coroutine to move the stone back to its initial position
                    StartCoroutine(MoveStoneBack(stone, initialPosition));

                    // Wait for the stone to move back before moving the next stone
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }

        IEnumerator MoveStoneBack(GameObject stone, Vector3 initialPosition)
        {
            // Wait for a moment before moving the stone back
            yield return new WaitForSeconds(waitTime + 1);

            // Move the stone back to its initial position
            while (stone.transform.position != initialPosition)
            {
                // Move the stone towards the initial position
                stone.transform.position = Vector3.MoveTowards(stone.transform.position, initialPosition, moveSpeed * Time.deltaTime);

                // Wait for the next frame
                yield return null;
            }
        }
    } 
}
