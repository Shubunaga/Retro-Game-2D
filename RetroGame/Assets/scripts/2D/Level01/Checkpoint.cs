using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Player playerChecked;
    Animator checkPointAnim;
    public Transform respawnPoint;

    BoxCollider2D checkColl;
    private void Awake()
    {
        playerChecked = GameObject.FindWithTag("Player").GetComponent<Player>();
        checkPointAnim = GetComponent<Animator>();

        checkColl = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerChecked.UpdateCheckpoint(respawnPoint.position);
            checkPointAnim.SetTrigger("CheckPoint");
            AudioController.current.PlayMusic(AudioController.current.ExtraPoints);
            
            checkColl.enabled = false;
        }
    }
}
