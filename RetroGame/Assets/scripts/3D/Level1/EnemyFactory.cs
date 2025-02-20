﻿using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public static class EnemyFactory {
        public static Enemy GenerateEnemy(Enemy enemyPrefab, SplineContainer flightPath, Transform enemyParent, Transform flightPathParent) {
            return new EnemyBuilder()
                .withPrefab(enemyPrefab)
                .withFlightPath(flightPath)
                .withFlightPathParent(flightPathParent)
                .build(enemyParent);
        }

        public static EnemyBase GenerateEnemyBase(EnemyBase enemyPrefab, SplineContainer flightPath, Transform enemyParent, Transform flightPathParent)
        {
            return new EnemyBuilderBase()
                .withPrefab(enemyPrefab)
                .withFlightPath(flightPath)
                .withFlightPathParent(flightPathParent)
                .build(enemyParent);
        }
    }
}