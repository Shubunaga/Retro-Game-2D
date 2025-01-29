using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundName : MonoBehaviour
{
    [SerializeField] Animator rdNumberAnim;

    [SerializeField] TextMeshProUGUI roundText;
    private int currentRound; //pegar valor no RoundSingleton
    private int maxRounds = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForInfo());
    }

 
    private void NextRound()
    {
        currentRound = RoundSingleton.Instance.totalRounds;
        currentRound++;

        if (currentRound <= maxRounds)
        {
            //currentRound = RoundSingleton.Instance.totalRounds + 1;
            //Debug.Log(currentRound);
            UpdateRoundText();

            rdNumberAnim.SetTrigger("Round Start");
        }
        else Debug.Log("Maximum rounds reached.");
    }

    private void UpdateRoundText()
    {
        roundText.text = $"Round {currentRound}\nFight!";
    }

    private void ResetRdNumberAnim()
    {
        rdNumberAnim.Rebind();  // Reinicia o Animator
        rdNumberAnim.Update(0);  // Garante que o Animator esteja no estado inicial
    }

    IEnumerator WaitForInfo()
    {
        ResetRdNumberAnim();
        yield return new WaitForSeconds(0.5f);
        NextRound();
    }
}
