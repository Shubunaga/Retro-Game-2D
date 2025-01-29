using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicalStrikeDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    public EnemySecond _enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        _enemy = GetComponentInParent<EnemySecond> ();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            Debug.Log("Entrei no physical");
            _enemy.SetPhysicalATKdistanceBool(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            _enemy.SetPhysicalATKdistanceBool(false);
        }
    }
}
