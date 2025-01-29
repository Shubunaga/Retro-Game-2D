using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundSingleton : MonoBehaviour
{
    public static RoundSingleton Instance { get; private set; }

    //private bool startOrNot;
    public GameObject dialogPrefab;
    private void Awake()
    {
        dialogPrefab = GameObject.Find("DialogSystem");

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (FinalRound())
            {
                Reset();
            }
        }
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log($"Start - Current wins - Left: {CurrentWinsOf(Side.Left)}," +
                                        $" Right: {CurrentWinsOf(Side.Right)}");
        
        if(totalRounds == 0)
        {
            dialogPrefab.SetActive(true);
        }else dialogPrefab.SetActive(false);
        //rdNumberAnim.SetTrigger("Round Start");
    }

    void OnDestroy()
    {
        Debug.Log("Singleton OnDestroy: " + gameObject.name);
    }

    private const int WINS_REQUIRED = 2;
    int _leftWins, _rightWins;

    //void Awake() => Reset(); //isso aqui serve para usar a void reset e zerar os rounds
    void OnEnable() => Instance = this;
    //void OnDisable() => Instance = null; PROBLEMA ERA NESSE CORNO que tava nulificando o singleton
    public void Reset() { _leftWins = _rightWins = 0; }

    public void RecordRoundWinner(Side winner)
    {
        setWins(winner, getWins(winner) + 1);
        Debug.Log($"Recorded win for {winner}. Current wins of: Left: {_leftWins}, Right: {_rightWins}");
    }

    public int CurrentWinsOf(Side side) => getWins(side);

    public int totalRounds => _leftWins + _rightWins;
    
    public bool FinalRound() 
        => _leftWins == WINS_REQUIRED || _rightWins == WINS_REQUIRED;

    public Side finalWinner => _leftWins > _rightWins ? Side.Left : Side.Right;

    private int getWins(Side side)
        => side == Side.Right ? _rightWins : _leftWins;

    private void setWins(Side side, int value)
    {
        if (side == Side.Right) _rightWins = value;
        else _leftWins = value;
    }

    public enum Side
    {
        Left = 0,
        Right = 1
    }
    //Operador Ternário => "is this condition true ? yes : no"

    [SerializeField] Animator transitionAnim;
    [SerializeField] Animator rdNumberAnim;

    public IEnumerator ReloadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        //transitionAnim.SetTrigger("Round End");
        yield return new WaitForSeconds(1);
        //StartCoroutine(RoundTransition()); ATIVA ISSO AQUI MAIS NÃO MEU AMIGO PFV

        SceneManager.LoadScene(sceneName);

        //ResetRdNumberAnim();
        //transitionAnim.SetTrigger("Round Start");

        yield return new WaitForSeconds(0.5f);


        //NextRound();
        //rdNumberAnim.ResetTrigger("Round Start");
        
        //rdNumberAnim.ResetTrigger("end");
    }
}
