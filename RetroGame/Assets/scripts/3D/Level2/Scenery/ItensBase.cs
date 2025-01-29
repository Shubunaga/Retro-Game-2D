using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class ItensBase : MonoBehaviour
    {
        public float speed = 1.0f; // velocidade do movimento de flutua��o
        public float height = 0.5f; // altura do movimento de flutua��o

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position; // posi��o inicial do objeto
        }

        void Update()
        {
            Vector3 newPos = startPos;
            newPos.y += Mathf.Sin(Time.time * speed) * height; // calcula a nova posi��o y
            transform.position = newPos; // aplica a nova posi��o ao objeto
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