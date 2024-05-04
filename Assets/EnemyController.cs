using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public enum EnemyState
{
    NORMAL,
    STUNNED,
    DEATH
}
public class EnemyController : MonoBehaviour
{
    private EnemyState enemyState;

    public Rigidbody2D enemyRB;
    public BoxCollider2D enemyBC;
    public SpriteRenderer enemySR;

    public float jumpForce;
    public float timeBtwnJumps;
    private int xDirection;

    private float lastJumpTime;

    public Transform pointA;
    public Transform pointB;

    private int life;

    public Animator animator;
    void Start()
    {
        enemyState = EnemyState.NORMAL;
        lastJumpTime = Time.time;
        xDirection = -1;
        life = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.NORMAL) 
        {
            if (transform.position.x < pointA.position.x && !enemySR.flipX)
            {
                xDirection = 1;
                enemySR.flipX = true;
            }

            if (transform.position.x > pointB.position.x && enemySR.flipX)
            {
                xDirection = -1;
                enemySR.flipX = false;
            }

            if (Time.time > lastJumpTime + timeBtwnJumps)
            {
                enemyRB.AddForce(new Vector2(jumpForce * xDirection, jumpForce), ForceMode2D.Impulse);
                lastJumpTime = Time.time;

                animator.SetBool("isJump", true);
                animator.SetBool("isIdle", false);
            }
            else if (Time.time < lastJumpTime + timeBtwnJumps)
            {
                animator.SetBool("isIdle", true);

            }
        }
        else if(enemyState == EnemyState.STUNNED) 
        {
            animator.SetBool("isHit", true);
        }
        else if(enemyState == EnemyState.DEATH) 
        {
            enemyRB.velocity = new Vector2(0, enemyRB.velocity.y);
            enemyBC.enabled = false;
            animator.SetBool("isDead", true);
        }

        Debug.Log(enemyState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyRB.velocity = Vector2.zero;

            if(enemyState != EnemyState.DEATH)
            {
                enemyState = EnemyState.NORMAL;


                animator.SetBool("isJump", false);
                animator.SetBool("isHit", false);
            }
                
        }
        if (collision.gameObject.CompareTag("DamageZone") && enemyState == EnemyState.NORMAL)
        {
            //LOGICA
            life -= 1;
            enemyRB.velocity = Vector2.zero;

            if (!collision.gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                enemyRB.AddForce(new Vector2(jumpForce, jumpForce ), ForceMode2D.Impulse);
            }
            else //(collision.gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                enemyRB.AddForce(new Vector2(-jumpForce , jumpForce ), ForceMode2D.Impulse);
            }

            

            //animator.SetBool("isHit", true);

            //STATE
            if (life <= 0)//Si  ha muerto
            {
                enemyState = EnemyState.DEATH;
                gameObject.tag = "Untagged";

                //FEEDBACK

                StartCoroutine(ResumeTime(0.2f));
                Time.timeScale = 0;

                animator.SetBool("isDead", true);
                animator.SetBool("isJump", false);
                animator.SetBool("isIdle", false);
            }
            else
            {
                enemyState = EnemyState.STUNNED; //Estunealo
                animator.SetBool("isHit", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJump", false);
            }

        }
    }


    public void DestroyThisGameObj()
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    IEnumerator ResumeTime(float time)
    { 
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }
}
