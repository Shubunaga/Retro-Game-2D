using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RailShooter
{
    public class FastShipSet : MonoBehaviour
    {
        public float rotationSpeed = 10f; // velocidade de movimento da nave

        void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}