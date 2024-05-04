using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    NORMAL, 
    STUNNED, 
    DEATH
}
public class PlayerController : MonoBehaviour
{
    private PlayerState playerState; 
    
    public Rigidbody2D playerRB;
    public SpriteRenderer playerSR;

    public float maxSpeed;
    public float jumpForce;

    private float xDirection;
    private bool isInGround;

    private float actualLife;
    public float maxLife = 3f;
    public GameObject attackZoneL;
    public GameObject attackZoneR;
    

    public Animator animator;

    void Start()
    {
        playerState = PlayerState.NORMAL;
        actualLife = maxLife;

        isInGround = false;
        StartAttackAnimationEvent();
    }

    // Update is called once per frame
    void Update()
    {
        //INPUT MOVEMENT
        xDirection = Input.GetAxis("Horizontal");

        //STATE
        if(playerState == PlayerState.NORMAL)
        {

            //MOVEMENT
            playerRB.velocity = new Vector2(xDirection * maxSpeed, playerRB.velocity.y);

            if (isInGround && (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow)))
            {
                playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                //Animation
                animator.SetBool("isJump", true);
            }

            //TEMPORAL ATTACK
            if (Input.GetKey(KeyCode.Z))
            {
                StartAttackAnimationEvent();
            }
            if (Input.GetKey(KeyCode.X))
            {
                StopAttackAnimationEvent();
            }

            //ANIMATIONS
            if (xDirection > 0 && playerSR.flipX)
            {
                playerSR.flipX = false;
            }
            else if (xDirection < 0 && !playerSR.flipX)
            {
                playerSR.flipX = true;
            }
            if (xDirection != 0) //No nos movemos 
            {
                //Animation
                animator.SetBool("isRun", true);
            }
            else //(xDirection == 0)
            {
                animator.SetBool("isRun", false);
            }
        }
        else if (playerState == PlayerState.STUNNED)
        {
            playerRB.AddForce( new Vector2(xDirection/2 , 0), ForceMode2D.Force);
        }
        else if (playerState == PlayerState.DEATH)
        {
            playerRB.velocity = Vector2.zero;
        }


        Debug.Log(actualLife);

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        { 
            isInGround = true;
            //Animation
            animator.SetBool("isJump", false);

            if (playerState != PlayerState.DEATH)//Si no ha muerto
            {
                animator.SetBool("isHit", false);
                playerState = PlayerState.NORMAL; //Se quita el stun
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isInGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //LOGICA
            actualLife -= 0.5f;

            if (!playerSR.flipX)
            {
                playerRB.AddForce(new Vector2(-jumpForce / 2, jumpForce / 2), ForceMode2D.Impulse);
            }
            else //(playerSR.flipX)
            {
                playerRB.AddForce(new Vector2(jumpForce / 2, jumpForce / 2), ForceMode2D.Impulse);
            }

            animator.SetBool("isHit", true);

            //STATE
            if (actualLife <= 0)//Si ha muerto
            {
                playerState = PlayerState.DEATH;
                animator.SetBool("isDeadHit", true);
            }
            else            
            { 
                playerState = PlayerState.STUNNED; //Estunealo
            }
            
        }
    }


    public void StartAttackAnimationEvent()
    {
        if (!playerSR.flipX)
        {
            attackZoneR.SetActive(true);
        }
        else if (playerSR.flipX)
        {
            attackZoneL.SetActive(true);
        }

        //Animation
        animator.SetBool("isAttack", true);

    }

    public void StopAttackAnimationEvent()
    {
        if (attackZoneR.activeSelf)
        {
            attackZoneR.SetActive(false);
        }
        else if (attackZoneL.activeSelf)
        {
            attackZoneL.SetActive(false);
        }

        //Animation
        animator.SetBool("isAttack", false);
    }
}
