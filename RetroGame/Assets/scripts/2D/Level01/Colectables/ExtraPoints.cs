using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPoints : MonoBehaviour
{
    private SpriteRenderer sr;
    private CircleCollider2D circle;
    
    public GameObject ExtraCollected;
    public int Score;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            sr.enabled = false;
            circle.enabled = false;
            ExtraCollected.SetActive(true);
            AudioController.current.PlayMusic(AudioController.current.ExtraPoints);

            EnvironmentController.instance.playerScore +=  Score;
            EnvironmentController.instance.UpdateScoreText();
            //
            Destroy(gameObject, 0.3f);

        }
    }
}
