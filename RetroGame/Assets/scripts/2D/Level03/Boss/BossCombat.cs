using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public LayerMask playerLayer;
    private PlayerWalk playerDirection;

    [SerializeField] AudioManager audioManager;
    
    #region Dados de ataque
    [SerializeField] Transform punchAttack;
    [SerializeField] Transform specialAttack;
    public float punchRange = 0.5f;
    private float punchDamage = 16;
    public float repulsionForce = 100f;
    
    public float repulsionY = 0.9f;
    public float repulsionX = -4f;
    #endregion

    #region movimentação do Chefe
    private Rigidbody2D bossRb;
    private Animator bossAnim;

    [SerializeField] float speedDash;// = 3f;
    
    private Vector2 stopMoving = Vector2.zero;
    
    private float originalGravity;
    #endregion

    //LEMBRANDO QUE PRECISO DESATIVAR MOMENTANEAMENTE
    //O AGGRO DO CHEFE QUANDO ELE EESTIVER RECEBENDO GOLPES DO PLAYER (Feito)
    private void Awake()
    {
        bossRb = GameObject.Find("Enemy Test").GetComponentInParent<Rigidbody2D>();
        bossAnim = GetComponentInChildren<Animator>();
        playerDirection = GameObject.Find("Player").GetComponent<PlayerWalk>();
        audioManager= GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        originalGravity = bossRb.gravityScale;
    }

    //Dados funciondo no componente "Sprite" do boss
    public void bossBackdash()
    {

//        bossRb.gravityScale = 0f;        

        if (playerDirection.isFacingRight == true)
        {
            bossRb.velocity = new Vector2(speedDash, bossRb.velocity.y);
            //bossAnim.SetBool("Backdash", true);
        }
        else 
        {
            bossRb.velocity = new Vector2(-speedDash, bossRb.velocity.y);
            //bossAnim.SetBool("Backdash", true);
        }

    }

    public void bossAttack1()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(punchAttack.position, punchRange, playerLayer);
        
        foreach (Collider2D playerCol in hitPlayer)
        {

            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D playerRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == true && playerRb != null)
            {
                audioManager.PlaySFX(audioManager.bossPunches);
                player.TakeDamage(punchDamage / 2); //diminuimos o dano pegando a variável isDefending
                StartCoroutine(dealingDamage(0.3f));
            }
            else if (PlayerDefense.isDefending == false && playerRb != null)
            {
                float directionX;
                if(playerDirection.isFacingRight == true) { directionX = repulsionX; } else {  directionX = -repulsionX; }


                Vector2 repulsionDirection = (Vector2)playerRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                
                repulsionDirection.y += repulsionY;
                repulsionDirection.x += directionX;//repulsionX;

                player.TakeDamage(punchDamage);
                playerDirection.canMove = false;

                playerRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Force);
                StartCoroutine(dealingDamage(1f));
                audioManager.PlaySFX(audioManager.bossPunches);
            }
        }

    }

    void BossAttack2()
    {
        //Debug.Log("acertamos o ataque2");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(punchAttack.position, punchRange * 2, playerLayer);
        
        foreach (Collider2D playerCol in hitPlayer)
        {
            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D enemyRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == true && enemyRb != null)
            {
                float directionX;
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();


                repulsionDirection.y += repulsionY;
                repulsionDirection.x += 0.2f;//repulsionX;

                player.TakeDamage(punchDamage / 3);
                enemyRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Force);
                StartCoroutine(dealingDamage(0.3f));
                audioManager.PlaySFX(audioManager.bossPunches);
                return;
            }
            else if (PlayerDefense.isDefending == false && enemyRb != null)
            {
                //primeiro hit do combo
                float directionX;
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                repulsionDirection.y += 15;
                repulsionDirection.x += directionX;

                player.TakeDamage(punchDamage * 2);
                playerDirection.canMove = false;

                enemyRb.AddForce(repulsionDirection * (repulsionForce), ForceMode2D.Force);
                audioManager.PlaySFX(audioManager.bossPunches);
                StartCoroutine(dealingDamage(1f));
            }
        }
    }
    //Esse ataque vai começar com uma estocada, se acertar o primeiro golpe, seguiremos pro segundo
    //(por conta do tempo consegui fazer apenas ele dar os 3 golpes em sequência, a confirmação do primeiro golpe dá um pouco
    //de trabalho pra fazer e tinha outras coisas pra fazer como prioridade
    public void bossComboFirstHit()
    {
        
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(punchAttack.position, punchRange, playerLayer);
        
        foreach (Collider2D playerCol in hitPlayer)
        {
            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D enemyRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == true && enemyRb != null)
            {
                //fazer ele parar o ataque aqui
                audioManager.PlaySFX(audioManager.bossPunches);
                return;
            }
            else if (PlayerDefense.isDefending == false && enemyRb != null)
            {
                //primeiro hit do combo
                float directionX;
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                repulsionDirection.y += repulsionY;
                repulsionDirection.x += directionX;

                player.TakeDamage(punchDamage / 2);
                playerDirection.canMove = false;

                enemyRb.AddForce(repulsionDirection * (repulsionForce * 0.6f), ForceMode2D.Force);
                audioManager.PlaySFX(audioManager.bossPunches);
                StartCoroutine(dealingDamage(1f));
            }
        }
    }

    public void BossComboSecondHit()
    {

        
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(punchAttack.position, punchRange, playerLayer);

        foreach (Collider2D playerCol in hitPlayer)
        {
            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D enemyRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == false && enemyRb != null)
            {
                //segundo hit do combo
                float directionX;
                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                repulsionDirection.y += 8f;
                repulsionDirection.x = 0f;

                player.TakeDamage(punchDamage / 2);
                playerDirection.canMove = false;
                
                enemyRb.AddForce(repulsionDirection * (repulsionForce), ForceMode2D.Force);
                audioManager.PlaySFX(audioManager.bossPunches);
                StartCoroutine(dealingDamage(1f));
            }
        }
    }

    public void BossComboThirdHit()
    {

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(punchAttack.position, punchRange, playerLayer);

        foreach (Collider2D playerCol in hitPlayer)
        {
            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D enemyRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == false && enemyRb != null)
            {
                //hit mais forte do combo
                float directionX;
                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                repulsionDirection.y += repulsionY;
                repulsionDirection.x += directionX;

                player.TakeDamage(punchDamage);
                playerDirection.canMove = false;
                
                enemyRb.AddForce(repulsionDirection * (repulsionForce * 5f), ForceMode2D.Force);
                audioManager.PlaySFX(audioManager.bossPunches);
                StartCoroutine(dealingDamage(1f));

            }
        }
    }
    #region Ataque Especial do chefe
    [SerializeField] private GameObject[] _virtualCameras;
    [SerializeField] private float timeLeft = 3.0f;
    public void BossSpecialAttack()
    {

        //prciso travar os controles do player e o timer quando ele for soltar o special
        playerDirection.canMove = false;
        //Trocar isso aqui por uma pausa do jogo inteiro enquanto a animação do special é mostrada //StartCoroutine(dealingDamage(2f));

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(specialAttack.position, (float)specialAttack.GetComponent<CircleCollider2D>().radius, playerLayer);

        CameraChange();

        foreach (Collider2D playerCol in hitPlayer)
        {
            Life player = playerCol.GetComponent<Life>();
            //aplicar força de repulsão
            Rigidbody2D enemyRb = playerCol.GetComponent<Rigidbody2D>();

            if (PlayerDefense.isDefending == true && enemyRb != null) //vai ter repulsão mesmo na defesa por ser um special, quero passar a sensação de força
            {
                float directionX;
                if (playerDirection.isFacingRight == true) { directionX = repulsionX; } else { directionX = -repulsionX; }

                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                repulsionDirection.y += repulsionY * 10; //aumentei a altura
                repulsionDirection.x += directionX * 0.1f;

                //aplicar metade do dano do special aqui
                enemyRb.AddForce(repulsionDirection * (repulsionForce * 0.2f), ForceMode2D.Force);
                player.TakeDamage(punchDamage * 3);
                audioManager.PlaySFX(audioManager.bossSpecialATK);
                return;
            }
            else if (PlayerDefense.isDefending == false && enemyRb != null)
            {
                //preciso fazer ele chegar até uma certa distância do player que não seja "colado" com ele, pra não ficar impossível
                //de desviar ou colocar duas distâncias diferentes de ativação (mesmo princípio da troca de velocidade do chefe)
                float directionX;
                if (playerDirection.isFacingRight == true) { directionX = -repulsionX; } else { directionX = repulsionX; }

                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                repulsionDirection.y += repulsionY * 30; //aumentei a altura
                repulsionDirection.x += directionX * repulsionX;

                player.TakeDamage(punchDamage * 6);
                playerDirection.canMove = false;

                enemyRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Force);
                audioManager.PlaySFX(audioManager.bossSpecialATK);
                StartCoroutine(dealingDamage(1f));

            }
            //Pausa para animação do golpe especial
            //StartCoroutine(CameraReturn());
        }
        StartCoroutine(CameraReturn());
    }


    void CameraChange()
    {
        _virtualCameras[1].SetActive(true);
    }

    IEnumerator CameraReturn()
    {
        Time.timeScale = 0.001f;
        yield return new WaitForSecondsRealtime(timeLeft);
        _virtualCameras[1].SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion

    //Aqui eu desativo os controles do jogador quando ele estiver ativamente apanhando
    IEnumerator dealingDamage(float timeToMove)
    {
        yield return new WaitForSecondsRealtime(timeToMove);
        playerDirection.canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (punchAttack == null)
            return;

        Gizmos.DrawWireSphere(punchAttack.position, punchRange);
    }
}
