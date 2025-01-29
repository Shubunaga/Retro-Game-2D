using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMonitor : MonoBehaviour
{
    Transform mainCamTransform; // Armazena a transformação da câmera FPS
    private bool visible = true;
    public float distanceToAppear = 8;
    Renderer objRenderer;

    private void Start()
    {
        mainCamTransform = Camera.main.transform; // Obter referência de transformação da câmera
        objRenderer = gameObject.GetComponent<Renderer>(); // Obter referência do renderizador
    }

    private void Update()
    {
        disappearChecker();
    }

    private void disappearChecker()
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);

        // Alcançamos a distância para habilitar o objeto
        if (distance < distanceToAppear)
        {
            if (!visible)
            {
                objRenderer.enabled = true; // Mostrar objeto
                visible = true;
                Debug.Log("Visible");
            }
        }
        else if (visible)
        {
            objRenderer.enabled = false; // Ocultar objeto
            visible = false;
            Debug.Log("InVisible");
        }
    }
}

