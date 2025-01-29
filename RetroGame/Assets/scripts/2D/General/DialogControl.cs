using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogObj;
    //public Image profile;
    public Text speechText;
    public Text actorNameText;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int  index;

    public void Speech(string[] txt, string actorName)//, sprite profile){
    {
        //Debug.Log("apertei pra abrir");
        dialogObj.SetActive(true);
        //profile.sprite = p;
        sentences = txt;
        actorNameText.text = actorName;
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if(speechText.text == sentences[index])
        {
            if(index < sentences.Length - 1) //ainda há textos
            {
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            else //lido quando acaba os textos
            {
                //Debug.Log("acabei");
                speechText.text  = "";
                index = 0;
                dialogObj.SetActive(false);
            }
        }
    }
}
