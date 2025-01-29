using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //SceneManager.LoadScene("Nivel 3 - 3D");
            SceneManager.LoadScene("EndUINivel2-3D");
        }
    }
}

