using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMonitor : MonoBehaviour
{
    Transform mainCamTransform; // Armazena a transforma��o da c�mera FPS
    private bool visible = true;
    public float distanceToAppear = 8;
    Renderer objRenderer;

    private void Start()
    {
        mainCamTransform = Camera.main.transform; // Obter refer�ncia de transforma��o da c�mera
        objRenderer = gameObject.GetComponent<Renderer>(); // Obter refer�ncia do renderizador
    }

    private void Update()
    {
        disappearChecker();
    }

    private void disappearChecker()
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);

        // Alcan�amos a dist�ncia para habilitar o objeto
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

