using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RailShooter
{
    public class EnemyUFOSet : MonoBehaviour
    {

        public float rotationSpeed = 10f;

        void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}
