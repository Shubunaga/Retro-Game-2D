using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    private float offset = 1f; // Offset para destruir o objeto um pouco fora da tela
    private Camera cam;
    private bool isOffscreen = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToViewportPoint(transform.position);
        if (screenPos.x < -offset || screenPos.x > 1 + offset || screenPos.y < -offset || screenPos.y > 1 + offset)
        {
            isOffscreen = true;
        }
        else
        {
            isOffscreen = false;
        }

        if (isOffscreen)
        {
            Destroy(gameObject);
        }
    }
}
