using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    NORMAL, 
    ATTACK,
    STUNNED, 
    DEATH
}
public class PlayerController : MonoBehaviour
{
    private PlayerState playerState; 
    
    public Rigidbody2D playerRB;
    public SpriteRenderer playerSR;
    public GameObject coinPrefab;
    public CanvasManager canvasManager;

    public float maxSpeed;
    public float jumpForce;

    private float xDirection;
    private bool isInGround;

    private float actualLife;
    public float maxLife = 3f;

    private int coins;

    public GameObject attackZoneL;
    public GameObject attackZoneR;
    public bool canAttack;

    public Vector2 actualCheckpoint;

    public Animator animator;

    void Start()
    {
        playerState = PlayerState.NORMAL;
        actualLife = maxLife;
        coins = 0;

        actualCheckpoint = transform.position;

        isInGround = false;
        canAttack = true;

        canvasManager.UpdateUILives(actualLife);
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
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                xDirection = 1;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                xDirection = -1;
            else 
                xDirection = 0;

            playerRB.velocity = new Vector2(xDirection * maxSpeed, playerRB.velocity.y);

            if (isInGround && (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow)))
            {
                playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                //Animation
                animator.SetBool("isJump", true);
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

            //ATTACK
            if (Input.GetKey(KeyCode.Space) && canAttack)
            {
                StartAttackAnimationEvent();
                canAttack = false;
                playerState = PlayerState.ATTACK;
                playerRB.velocity = Vector2.zero;
                playerRB.gravityScale = 0;
            }
        }
        if (playerState == PlayerState.ATTACK)
        {
           
        }
        else if (playerState == PlayerState.STUNNED)
        {
            playerRB.AddForce(new Vector2(xDirection / 2, 0), ForceMode2D.Force);
        }
        else if (playerState == PlayerState.DEATH)
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);

            LoseCoins();
        }


        Debug.Log(actualLife);

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            actualCheckpoint = transform.position;
        }


        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            playerState = PlayerState.DEATH;
            LoseCoins();
            TeleportToCheckPoint();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerState != PlayerState.DEATH && playerState != PlayerState.ATTACK)//Si no ha muerto
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isInGround = true;
                //Animation
                animator.SetBool("isJump", false);
                animator.SetBool("isHit", false);
                playerState = PlayerState.NORMAL; //Se quita el stun
                canAttack = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerState != PlayerState.DEATH)//Si no ha muerto
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isInGround = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerState != PlayerState.DEATH)//Si no ha muerto
        {
            if (collision.gameObject.CompareTag("Enemy") && playerState == PlayerState.NORMAL)
            {
                //LOGICA
                actualLife -= 0.5f;

                HitPlayerFeedback();

                canvasManager.UpdateUILives(actualLife);
                canvasManager.HitFeedback();
            }

            if (collision.gameObject.CompareTag("Spikes"))
            {
                actualLife = 0f;

                HitPlayerFeedback();

                canvasManager.UpdateUILives(actualLife);
                canvasManager.HitFeedback();
            }

            if (collision.gameObject.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                coins++;
            }

            
        }
    }
    private void LoseCoins()
    {
        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
            Rigidbody2D coinRB = coin.GetComponentInChildren<Rigidbody2D>();
            coinRB.AddForce(new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(1f, 5f)), ForceMode2D.Impulse);
        }

        coins = 0;
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
            attackZoneR.SetActive(false);
            attackZoneL.SetActive(false);
        

        //Animation
        animator.SetBool("isAttack", false);

        playerRB.gravityScale = 2.5f;
        playerState = PlayerState.NORMAL;
    }

    private void HitPlayerFeedback()
    {
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

        canvasManager.UpdateUILives(actualLife);
        canvasManager.HitFeedback();
    }
    public void TeleportToCheckPoint()
    {
        playerState = PlayerState.NORMAL;
        actualLife = maxLife;
        canvasManager.UpdateUILives(actualLife);

        animator.SetBool("isDeadHit", false);
        animator.SetBool("isHit", false);

        transform.position = actualCheckpoint;
    }
}
