using UnityEngine;

namespace RailShooter
{
    [System.Serializable]
    public class Square
    {
        public float distance; // Distance of the square center from the spawner
        public float sideLength; // Side length of the square

        public Vector3 GetRandomPoint()
        {
            // Random x and y coordinates within the square
            float x = Random.Range(-sideLength / 2, sideLength / 2);
            float y = Random.Range(-sideLength / 2, sideLength / 2);

            // Return the point
            return new Vector3(x, y, distance);
        }
    }
}
