using System.Net;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public static class FlightPathFactory {
        public static SplineContainer GenerateFlightPath(Square[] squares)
        {
            Vector3[] pathPoints = new Vector3[squares.Length];

            for (int i = 0; i < squares.Length; i++)
            {
                pathPoints[i] = squares[i].GetRandomPoint();
            }

            return CreateFlightPath(pathPoints);
        }

        static SplineContainer CreateFlightPath(Vector3[] pathPoints)
        {
            GameObject flightPath = new GameObject("Flight Path");

            var container = flightPath.AddComponent<SplineContainer>();
            var spline = container.AddSpline();
            var knots = new BezierKnot[pathPoints.Length];

            for (int i = 0; i < pathPoints.Length; i++)
            {
                knots[i] = new BezierKnot(
                    pathPoints[i],
                    -30 * Vector3.forward,
                    30 * Vector3.forward);
            }

            spline.Knots = knots;

            return container;
        }
    }

}