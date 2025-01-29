using Level2;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class RoundCount : MonoBehaviour
{
    //dados dos personagens para quando acabar o round
    [SerializeField] Life playerData;
    [SerializeField] Life bossData;
    private TimeCounter timeCount;
    float resultsTime = 4f;

    [SerializeField] Transform playerTransform;
    [SerializeField] Transform bossTransform;
    //
    private PlayerWalk playerWalk;
    private PlayerCombo playerCombo;
    //

    public Image[] lifeIcon; //tentar fazer servir pro player e pro boss, economizando espa�o
    public Sprite full;
    public Sprite hollow;

    //transi��o de um round pro outro
    //public Image roundEndEffect;
    public Animator roundEndEffect;
    public Image roundStartEffect;

    private bool isUpdateEnabled;

    public GameObject victoryScreen;
    public GameObject gameOverScreen;

    public Animator roundTransitionAnim;
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1f;
        playerData = GameObject.Find("Player").GetComponent<Life>();
        bossData = GameObject.Find("Enemy Test").GetComponent<Life>();
        timeCount = GameObject.Find("Main Camera").GetComponent<TimeCounter>();

        //bossMov = GameObject.Find("Enemy Test").GetComponentInChildren<BossMovimentation>();//n�o vou conseguir parar ele com isso
        playerWalk = GameObject.Find("Player").GetComponent<PlayerWalk>();
        playerCombo = GameObject.Find("Player").GetComponent<PlayerCombo>();

        Debug.Log($"Scene Reloaded - Player and boss Initialized");

        
        if (RoundSingleton.Instance.FinalRound() && (RoundSingleton.Instance.finalWinner == RoundSingleton.Side.Left))
        {
            isUpdateEnabled = false;
            victoryScreen.SetActive(true);//ATIVAR A TELA FINAL AQUI
        }
        else if (RoundSingleton.Instance.FinalRound() && (RoundSingleton.Instance.finalWinner == RoundSingleton.Side.Right))
        {
            isUpdateEnabled = false;
            gameOverScreen.SetActive(true);//ATIVAR TELA DE DERROTA AQUI
            RoundSingleton.Instance.Reset();
        }
        else isUpdateEnabled = true;
        //StartCoroutine(roundStartCutscene());
        //FUN��O PARA CUIDAR DAS MUDAN�AS NOS CORA��ES
        UpdateHUD();

        //Debug.Log($"LifeIcon length: {lifeIcon.Length}");
        //foreach (var icon in lifeIcon)
        //{
        //    Debug.Log(icon?.name ?? "null");
        //}
    }


    // Update is called once per frame
    void Update()
    {
        if (timeCount.timer <= 0)
        {
            float resultsTime = 4f;
            RoundCheckByTime();

            for (float i = 0; i < resultsTime; i += Time.deltaTime)
            {
                playerWalk.gameObject.SetActive(false);
                playerCombo.gameObject.SetActive(false);
            }
            playerWalk.gameObject.SetActive(true);
            playerCombo.gameObject.SetActive(true);

            timeCount = GameObject.Find("Main Camera").GetComponent<TimeCounter>();
            timeCount.timer += 61f;
        }

        if (isUpdateEnabled)
        {
            GameSet();

        }

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Time.timeScale = 1f;
        //}
    }

    void GameSet()
    {
        if (playerData.fainted || bossData.fainted)//(playerData.life <= 0 || bossData.life <= 0)
        {
            GettingRoundResults();
            isUpdateEnabled = false; //para de rodar no update

            //if (RoundSingleton.Instance.FinalRound())
            //{
            //    //Time.timeScale = 0.01f; //vai ficar essa c�mera lenta no final mesmo, at� a tela de game over aparecer
            //    Debug.Log("Ultimo round"); //Funcionando
            //                               //Time.timeScale = 0.2f; //vai ficar essa c�mera lenta no final mesmo, at� a tela de game over aparecer
                
            //    victoryScreen.SetActive(true);//ATIVAR A TELA FINAL AQUI AQUI                 

            //    isUpdateEnabled = false; //para de rodar no update
            //    //UpdateHUD(); //se n�o for ou for, tentar trocar pela corrotina aqui
            //    return;
            //}

            Debug.Log("sa�mos do for");

        }

    }

    private void GettingRoundResults()
    {

        RoundCheck();

        //playerWalk.gameObject.SetActive(false);//O PROBLEMA PROVAVELMENTE TAVA NESSAS
        //playerCombo.gameObject.SetActive(false);//DUAS LINHAS, estavam desativando os gameObjects por inteiro
        playerWalk.enabled = false; //o comando assim desativa SOMENTE o SCRIPT(valor que t� nessa vari�vel no momento)
        playerCombo.enabled = false;

        //StartCoroutine(RoundTransition()); //ESSA DAQUI � A DO ROUNDCOUNT E N�O DO SINGLETON
        RoundTransition();
    }

    private void RoundCheckByTime()
    {


        //round1Ended = true; <-isso aqui n�o funciona!

        //fazer uma pausa, desativar a camada superior do cora��o de quem perdeu o round e
        //resetar a cena(manualmente encher as vidas e retornar os personagens �s suas posi��es iniciais sobreescrevendo o transform)
        if (playerData.life < bossData.life)
        {
            lifeIcon[0].sprite = hollow;

            //lifeIcon[0].enabled = false;
            //�cone posicionado mais a esquerda, o �ndice 1 ser� a �ltima "vida" do jogador e
            //os �ndices 2 e 3 servir�o da mesma forma para o chefe, sendo o �ndice 2 sua �ltima vida)
            if (RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Right) > RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Left))// round1Ended == true)
            {
                lifeIcon[1].sprite = hollow;
                RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Right);
                return; //por enquanto!!!
                //tela de game over e restart aqui - PLAYER PERDEU
            }
            else
                RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Right);
            return;
        }
        else if (bossData.life < playerData.life)                       //lembrando que preciso fazer isso pros 2 rounds(ganhos)
        {                                                               //da� vou precisar de uma bool nesses IF's
            lifeIcon[3].sprite = hollow;


            if (RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Right) < RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Left))//round1Ended == true)
            {
                lifeIcon[2].sprite = hollow;
                RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Left);
                return; //por enquanto!!!
                //tela de vit�ria aqui
            }
            else
                RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Left);
            return;
        }
    }


    private void RoundCheck()
    {
        if (playerData.fainted == true)
        {

            RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Right);
            playerData.fainted = false;

        }

        else if (bossData.fainted == true) //lembrando que preciso fazer isso pros 2 rounds(ganhos)
        {

            RoundSingleton.Instance.RecordRoundWinner(winner: RoundSingleton.Side.Left);
            bossData.fainted = false;

        }
    }

    void UpdateHUD()
    {
       if(lifeIcon != null && lifeIcon.Length > 0)
        {
            int rightWins = RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Right);
            int leftWins = RoundSingleton.Instance.CurrentWinsOf(RoundSingleton.Side.Left);

            Debug.Log($"Victory on boss side: " + rightWins);
            Debug.Log($"Victory on Player side: " + leftWins);

            lifeIcon[0].sprite = rightWins > 0 ? hollow : full;
            lifeIcon[1].sprite = rightWins > 1 ? hollow : full;
            lifeIcon[3].sprite = leftWins > 0 ? hollow : full;
            lifeIcon[2].sprite = leftWins > 1 ? hollow : full;

            Debug.Log("HUD updated");
        }
        else Debug.LogError("lifeIcon array retornou null");
    }

    IEnumerator roundStartCutscene()//Defasado por enquanto
    {
        roundStartEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.04f);//0.99f);
        roundStartEffect.gameObject.SetActive(false);

    }

    IEnumerator roundEndCutscene()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;
        victoryScreen.SetActive(true);
    }

    public Animator bossAnim;
    void RoundTransition()
    {                             //enquanto a transi��o est� ativa
        //NOVO

        StartCoroutine(RoundSingleton.Instance.ReloadScene()); //Colocamos a transi��o de cena inteira dentro de um objeto vazio
                                                               //e colocamos esse objeto dentro do singleton para que a anima��o
                                                               //continuasse mesmo no momento de recarregamento da cena \o/
        //ANTIGO
        //roundEndEffect.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1.1f);//0.99f);

        //recarregar a cena e reativar os scripts
        //RoundSingleton.Instance.ReloadScene();
        //
    }

}
