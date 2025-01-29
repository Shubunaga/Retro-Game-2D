using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class DynamicSpriteSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assumes your player object has the tag "Player"
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = (int)(playerTransform.position.y - transform.position.y) * 100;
    }
}

