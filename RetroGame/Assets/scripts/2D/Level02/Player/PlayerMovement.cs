using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlayerState
{
    walk,
    attack,
    interact
}

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private int mechanismCount;
    [SerializeField] private GameObject bossPath;//abertura da passagem pra sala do chefe

    public PlayerState currentState;

    public float speed;
    public Rigidbody2D rbody;
    private Vector3 change;
    private Animator animator;

    void Start()
    {
        Application.targetFrameRate = 60;
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }

    }

    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (currentState == PlayerState.walk)
        {
            UpddateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        rbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(.33f);
        currentState = PlayerState.walk;
        AudioController.current.PlayMusic(AudioController.current.shooting);
    }

    void UpddateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("Horizontal", change.x);
            animator.SetFloat("Vertical", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }


    void MoveCharacter()
    {

        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * speed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        //isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
    }

    public void MechanismActivation()
    {
        mechanismCount += 1;
        Debug.Log("Mechanism count: " + mechanismCount);

        if(mechanismCount >= 6)
        {
            bossPath.SetActive(false);
            //dar play na cutscene aqui de alguma forma
        }
    }

    // void OnDrawGizmos() 
    // {
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     float verticalInput = Input.GetAxis("Vertical");
    //     Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine (transform.position, (Vector2)transform.position + inputVector * grabbingDistance);
    // }

}