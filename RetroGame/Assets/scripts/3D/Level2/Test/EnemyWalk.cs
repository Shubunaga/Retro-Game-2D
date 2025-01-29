using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public float speed;
    public Transform pointA, pointB;
    private Vector3 target;

    void Start()
    {
        target = pointA.position;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            if (target == pointA.position)
                target = pointB.position;
            else
                target = pointA.position;
        }
    }
}
