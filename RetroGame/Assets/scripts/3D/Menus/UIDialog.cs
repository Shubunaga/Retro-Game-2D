using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public TMP_Text dialogText;
    public Queue<string> sentences;
    public GameObject[] dialogueScreens;
    private String[] dialogues;
    private int dialogueIndex = 0;
    private int indicatorIndex = 0;
    public int[] dialogueWithIndicator;
    public GameObject[] Indicators;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        sentences= new Queue<string>();
    }

    public void DisplayControls(String[] dialogue)
    {
        dialogues = dialogue;
        dialogueScreens[0].SetActive(true);
    }

    public void ButtonStartDialog()
    {
        dialogueScreens[0].SetActive(false);
        StartDialog(dialogues);
    }

    public void StartDialog(string[] dialogue)
    {
        Debug.Log("Start Dialog");
        dialogueScreens[1].SetActive(true);
        sentences.Clear();

        foreach(string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSetence();
    }

    public void DisplayNextSetence()
    {
        if(sentences.Count == 0) 
        {
            EndOfDialog();
            return;
        }
        if (indicatorIndex != 0)
            Indicators[indicatorIndex - 1].SetActive(false);

        dialogueIndex += 1;
        Debug.Log(dialogueIndex);
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
        Uiindicator();
    }

    private void Uiindicator()
    {
        foreach(int intWithIndicator in dialogueWithIndicator)
        {
            if (intWithIndicator == dialogueIndex)
            {
                //Indicators[indicatorIndex].SetActive(true);
                StartCoroutine(BlinkIndicator(indicatorIndex));
                indicatorIndex+= 1;
                return;
            }
        }
    }

    private IEnumerator BlinkIndicator(int index)
    {
        Indicators[index].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Indicators[index].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Indicators[index].SetActive(true);
    }

    private void EndOfDialog()
    {
        Debug.Log("End of conversation");
        foreach(GameObject screen in dialogueScreens)
        {
            screen.SetActive(false);
        }
        Time.timeScale = 1;
    }
}
