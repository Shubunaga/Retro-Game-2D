using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform punchAttack;
    public float punchRange = 0.5f;
    public float repulsionForce = 100f;

    public float atkRate = 2f;
    public float nextAtkTime = 0f;

    public LayerMask enemyLayer;

    void Update()
    {
        if(Time.time >= nextAtkTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAtkTime= Time.time + 1 / atkRate;
            }
        }
       
    }

    void Attack()
    {
        //play attack animation
        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchAttack.position, punchRange, enemyLayer);

        foreach(Collider2D enemyCol in hitEnemies)
        {
            //aplicar força de repulsão
            Rigidbody2D enemyRb = enemyCol.GetComponent<Rigidbody2D>();
            if(enemyRb != null)
            {
                Vector2 repulsionDirection = (Vector2)enemyRb.position - (Vector2)punchAttack.position;
                repulsionDirection.Normalize();

                float repulsionY = 0.7f;
                repulsionDirection.y += repulsionY;

                enemyRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Force);
            }
        }
        //damage them
        //Debug.Log("We hit him");
        //anim.SetTrigger("Attack1);


    }

    private void OnDrawGizmosSelected()
    {
        if (punchAttack == null)
            return;

        Gizmos.DrawWireSphere(punchAttack.position, punchRange);
    }


    #region Special Attack
    private bool canMove = true;
    IEnumerator deactivate() //utilizar quando for fazer o especial
    {
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        StopCoroutine(deactivate());
    }

    #endregion
}
