using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    public TextMeshProUGUI comboText;
    //public TextMeshPro comboText;
    public float comboDrop = 0.9f;
    private Animator comboTextAnimator;
    private int comboCount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        comboTextAnimator = comboText.GetComponent<Animator>();
    }

    public void SetCombo()
    {
        comboCount++;
        comboText.text = comboCount + "\n Hits";
        comboTextAnimator.SetTrigger("Hit");

        CancelInvoke();
        Invoke("ResetCombo", comboDrop);
    }

    void ResetCombo()
    {
        comboCount = 0;
    }
}
