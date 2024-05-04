using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public SpriteRenderer playerSR;

    public float maxSpeed;
    public float jumpForce;

    private  bool isInGround;

    public GameObject attackZoneL;
    public GameObject attackZoneR;

    public Animator animator;

    void Start()
    {
        isInGround = false;

        StartAttackAnimationEvent();
    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        playerRB.velocity = new Vector2(xDirection * maxSpeed, playerRB.velocity.y);

        if ( xDirection > 0 && playerSR.flipX) 
        { 
            playerSR.flipX = false;
            //Animation
            animator.SetBool("isRun", true);
        }
        else if (xDirection < 0 && !playerSR.flipX)
        {
            playerSR.flipX = true;
            //Animation
            animator.SetBool("isRun", true);
        }
        else if(xDirection != 0) //No nos movemos 
        {
            //Animation
            animator.SetBool("isRun", true);
        }else if (xDirection == 0)
        {
            animator.SetBool("isRun", false);
        }

        if (isInGround && (Input.GetKeyDown(KeyCode.W) ||
         Input.GetKeyDown(KeyCode.UpArrow)))
        {
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //Animation
            animator.SetBool("isJump", true);
        }

        //TEmporal, la teca de ataque sera space
        if (Input.GetKey(KeyCode.Z))
        {
            StartAttackAnimationEvent();
        }
        if (Input.GetKey(KeyCode.X))
        {
            StopAttackAnimationEvent();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        { 
            isInGround = true;
            //Animation
            animator.SetBool("isJump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isInGround = false;
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
