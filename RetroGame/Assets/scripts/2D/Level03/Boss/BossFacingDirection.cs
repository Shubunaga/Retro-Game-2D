using Schema.Builtin.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFacingDirection : MonoBehaviour
{

    public GameObject player;

    //troca de lados
    void Update()
    {
        Vector3 scale = transform.localScale;

        if (player.transform.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1;
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    //fightersDistance = Vector3.Distance(transform.position, Enemy.transform.position); //tentar usar na IA do boss
}