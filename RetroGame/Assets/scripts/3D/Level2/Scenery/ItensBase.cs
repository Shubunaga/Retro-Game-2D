using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class ItensBase : MonoBehaviour
    {
        public float speed = 1.0f; // velocidade do movimento de flutuação
        public float height = 0.5f; // altura do movimento de flutuação

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position; // posição inicial do objeto
        }

        void Update()
        {
            Vector3 newPos = startPos;
            newPos.y += Mathf.Sin(Time.time * speed) * height; // calcula a nova posição y
            transform.position = newPos; // aplica a nova posição ao objeto
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}