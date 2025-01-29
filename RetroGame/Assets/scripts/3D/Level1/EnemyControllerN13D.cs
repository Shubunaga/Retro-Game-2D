using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerN13D : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform followTarget;
    [SerializeField] float followDistance = 22f;
    [SerializeField] float smoothTime = 0.2f;
    Vector3 velocity;

    void Update()
    {
        Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;
        transform.position = Vector3.SmoothDamp(current: transform.position, targetPos, ref velocity, smoothTime);

        transform.rotation = player.rotation;
    }

    void OnBecameInvisible()
    {
        // Fazer o inimigo desaparecer quando ele sair da tela
        gameObject.SetActive(false);
    }
}
