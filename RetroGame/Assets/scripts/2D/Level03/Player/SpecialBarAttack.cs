using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialBarAttack : MonoBehaviour
{
    private Life spBar;
    public Transform spInstance;
    public GameObject bltStrikePrefab;
    public float spSpeed;
    public bool spIsActive;
    private PlayerWalk facingDir;
    private Animator anim;

    //Special variables
    [SerializeField] private float timeLeft;
    [SerializeField] private GameObject[] _virtualCameras;

    public AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        spBar = GameObject.Find("Player").GetComponent<Life>();
        facingDir = GameObject.Find("Player").GetComponent<PlayerWalk>();
        anim = GameObject.Find("Player").GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Barra de especial atual: " + spBar.specialBar);
        }


    }

    private void FixedUpdate()
    {
        if (spBar.special >= 100 && Input.GetKey(KeyCode.R)) //lembrar que o special é no R
        {
            SpecialATK();
            StartCoroutine(CameraReturn());
            //anim.ResetTrigger("Special");
        }
        
    }

    void SpecialATK()
    {
        //specBar = spBar.specialBar;
        //StartCoroutine(CameraChange());
        CameraChange();
        anim.SetTrigger("Special");
        audioManager.PlaySFX(audioManager.specialATK);

        spBar.special = 0;
        Debug.Log("Ativamos o special!");
        spIsActive = true; //pegar o script de movimentação do player e desativar a movimentação, aqui ou lá

        GameObject bullet = Instantiate(bltStrikePrefab, spInstance.position, transform.rotation);

        if (facingDir.isFacingRight == true)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(spSpeed, 0);
        }
        else// if (facingDir.isFacingRight == false) 
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-spSpeed, 0);
        }

    }

    //IEnumerator CameraChange()
    //{
    //    _virtualCameras[1].SetActive(true);
    //    yield return new WaitForSeconds(0.6f);
    //}
    void CameraChange()
    {
        _virtualCameras[1].SetActive(true);
    }

    IEnumerator CameraReturn()
    {
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(timeLeft);
        _virtualCameras[1].SetActive(false);
        Time.timeScale = 1f;
    }
}
