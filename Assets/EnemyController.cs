using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
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

        //Debug.Log("Point A Position: " + pointA.position);
        //Debug.Log("Point B Position: " + pointB.position);
        //transform.localPosition = Vector3.Lerp(pointA.localPosition, pointB.localPosition, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log("Initial Position: " + transform.position);
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
            this.GetComponents<BoxCollider2D>()[0].enabled = false;
            this.GetComponents<BoxCollider2D>()[1].enabled = false;
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
        if ((collision.gameObject.CompareTag("DamageZone")|| collision.gameObject.CompareTag("Spikes")) && enemyState == EnemyState.NORMAL)
        {
            //LOGICA
            life -= 1;
            enemyRB.velocity = Vector2.zero;

            if (collision.transform.position.x > transform.position.x)
            {
                enemyRB.AddForce(new Vector2(-jumpForce, jumpForce ), ForceMode2D.Impulse);
            }
            else //(collision.gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                enemyRB.AddForce(new Vector2(jumpForce , jumpForce ), ForceMode2D.Impulse);
            }

            Debug.Log("Direction Player = " + collision.gameObject.GetComponent<SpriteRenderer>().flipX);
            //FEEDBACK
            StartCoroutine(PauseTime());

            //animator.SetBool("isHit", true);

            //STATE
            if (life <= 0)//Si  ha muerto
            {
                enemyState = EnemyState.DEATH;
                gameObject.tag = "Untagged";

                //FEEDBACK
                
                StartCoroutine(PauseTime());

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

    IEnumerator PauseTime()
    {
        Time.timeScale = 0.1f;
        float pauseEndTime = Time.realtimeSinceStartup + 0.15f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }
}
