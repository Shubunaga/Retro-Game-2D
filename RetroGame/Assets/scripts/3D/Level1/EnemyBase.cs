using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public class EnemyBase : ValidatedMonoBehaviour {
        [SerializeField, Self] SplineAnimate splineAnimate;
        [SerializeField] GameObject explosionPrefab;
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
            if (other.CompareTag("PlayerBullets") && GetComponent<EnemyMomSet>() == null)
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
            }
        }
        void OnDestroy()
        {
            if (flightPath != null)
            {
                Destroy(flightPath.gameObject);
            }
        }

    }
}