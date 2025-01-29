using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour //, Damage
{
    public float life = 100;
    public float special = 25;
    public Image lifeBar;
    public Image specialBar;
    public bool fainted;

    private void Start()
    {
        fainted = false;
    }
    private void Update()
    {
        lifeBar.fillAmount = life / 200f;
        specialBar.fillAmount = special/ 100f;
    }

    public void TakeDamage(float damage)
    {
        life -= damage;

        //LifeBar.fillAmount = life / 100;

        special += damage * 2f;
        //Debug.Log("Vida: " + life + " Special bar: " + special);

        if (special >= 100)
        {
            special = 100;
        }

        if(life <= 0)
        {
            //pegar em qual gameobject que isso tá e decidir a vitória com o singleton
            //if(this.gameObject.name == "Player")
            //{
            //    RoundSingleton.Instance.RecordRoundWinner(RoundSingleton.Side.Right);
            //    Debug.Log("Player morreu");
            //} else if(this.gameObject.name == "Enemy Test")
            //{
            //    RoundSingleton.Instance.RecordRoundWinner(RoundSingleton.Side.Left);
            //    Debug.Log("boss morreu");
            //}
            Debug.Log("Game over pra esse aqui");
            fainted = true;
        }
        
    }
}
