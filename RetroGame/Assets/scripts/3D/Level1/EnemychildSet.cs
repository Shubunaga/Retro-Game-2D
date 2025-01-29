using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RailShooter
{
    public class EnemyChildSet : MonoBehaviour
    {
        public float moveSpeed = 50f; // velocidade de movimento do inimigo filho
        public float instabilityFactor = 0.5f; // fator de instabilidade do movimento
        public Transform playerTransform; // transform do jogador

        private Vector3 targetPosition; // posição alvo para o inimigo filho se mover

        void Start()
        {
            // destrói o inimigo filho após 5 segundos
            Destroy(gameObject, 3f);

            // define a posição alvo inicial como a posição do jogador
            targetPosition = playerTransform.position;
        }

        void Update()
        {
            if (targetPosition != null)
            {
                // move em direção à posição alvo
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // atualiza a posição alvo com uma pequena variação aleatória para criar instabilidade no movimento
                targetPosition = playerTransform.position + Random.insideUnitSphere * instabilityFactor;
            }
        }
        void OnDisable()
        {
            if (!this.gameObject.scene.isLoaded) return;
            // Instantiate objects here
        }

    }
}