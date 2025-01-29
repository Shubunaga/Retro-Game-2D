using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomChange : MonoBehaviour
{
    public Transform target;
    public Vector3 playerChange;
    private BasicCameraFollow cam; //script da câmera
    public bool needText;
    public string placeName; //nome do lugar/sala
    public GameObject text;
    public Text placeText;

    //Fade Effect
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<BasicCameraFollow>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            other.transform.position += playerChange;
            if(needText)
            {
                StartCoroutine(placeNameCo());
            }
            StartCoroutine(FadeInAndOutCO());
        }
    }
    
    private IEnumerator placeNameCo()
    {
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(6f);
        text.SetActive(false);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(playerChange, playerChange);
    }

    private IEnumerator FadeInAndOutCO()
    {
        anim.SetBool("Touch", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Touch", false);
    }
   
}
