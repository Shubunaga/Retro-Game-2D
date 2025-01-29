using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Duração da vibração da câmera
    public float shakeDuration = 0.15f;

    // Magnitude da vibração da câmera
    public float shakeMagnitude = 0.5f;

    // Velocidade de amortecimento
    public float dampingSpeed = 1.0f;

    // A rotação inicial da câmera
    Quaternion initialRotation;

    // Um sinalizador para verificar se a câmera deve vibrar
    bool isShaking = false;

    void Awake()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (isShaking)
        {
            if (shakeDuration > 0)
            {
                transform.localRotation = initialRotation * Quaternion.Euler(0, 0, Random.Range(-shakeMagnitude, shakeMagnitude));
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                isShaking = false;
                shakeDuration = 0f;
                transform.localRotation = initialRotation;
            }
        }
    }

    public void TriggerShake()
    {
        isShaking = true;
        shakeDuration = 0.15f;
    }
}
