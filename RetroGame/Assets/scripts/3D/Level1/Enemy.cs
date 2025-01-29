using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public class Enemy : ValidatedMonoBehaviour {
        [SerializeField, Self] SplineAnimate splineAnimate;
        [SerializeField] GameObject explosionPrefab;
        [SerializeField] 
        SplineContainer flightPath;
        
        public SplineContainer FlightPath {
            get => flightPath;
            set => flightPath = value;
        }
        void Update() {
            if (splineAnimate != null && splineAnimate.ElapsedTime >= splineAnimate.Duration) {
                Destroy(gameObject);
            }

        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("PlayerBullets"))
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
            }
        }

    }
}