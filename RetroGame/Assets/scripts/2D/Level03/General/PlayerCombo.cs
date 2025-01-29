using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    public Combo[] combos;

    public AttackData attackData;

    public List<string> currentCombo;

    private Animator anim;
    private bool startCombo;
    private Hit currentHit, nextHit;
    private float comboTimer;
    
    private bool canHit = true;
    private bool resetCombo;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        for (int i = 0; i < combos.Length; i++)
        {
            if(combos[i].hits.Length > currentCombo.Count)
            {
                if (Input.GetButtonDown(combos[i].hits[currentCombo.Count].inputButton))
                {
                    if (currentCombo.Count == 0)
                    {
                        Debug.Log("Primeiro hit foi adicionado");
                        PlayHit(combos[i].hits[currentCombo.Count]);
                        break;
                    }
                    else
                    {
                        bool comboMatch = false;
                        for (int y = 0; y < currentCombo.Count; y++)
                        {
                            if (currentCombo[y] != combos[i].hits[y].inputButton)
                            {
                                Debug.Log("Input não pertence ao combo atual");
                                comboMatch = false;
                                break;
                            }
                            else
                            {
                                comboMatch = true;
                            }
                        }

                        if (comboMatch && canHit)
                        {
                            Debug.Log("Hit adicionado ao combo");
                            canHit = false;
                            nextHit = combos[i].hits[currentCombo.Count];
                            break;
                        }
                    }

                }
            }
        }
        if (startCombo)
        {
            comboTimer += Time.deltaTime;
            if(comboTimer >= currentHit.animationTime && !canHit)
            {
                PlayHit(nextHit);
            }

            if(comboTimer >= currentHit.resetTime)
            {
                ResetCombo();
            }
        }
    }

    void PlayHit(Hit hit)
    {
        comboTimer = 0f;
        attackData.SetAttack(hit);
        startCombo = true;
        currentCombo.Add(hit.inputButton);
        currentHit = hit;
        anim.Play(hit.animation);
        canHit = true;
    }

    void ResetCombo()
    {
        startCombo = false;
        comboTimer = 0f;
        currentCombo.Clear();
        anim.Rebind();
        canHit = true;
    }
}
