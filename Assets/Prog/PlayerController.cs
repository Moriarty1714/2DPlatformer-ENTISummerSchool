using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerRB;
    public SpriteRenderer playerSR;

    public float maxSpeed;
    public float jumpForce;

    public float maxJumpTime;
    private float jumpTimming;
    private  bool isInGround;

    public GameObject attackZoneL;
    public GameObject attackZoneR;

    void Start()
    {
        jumpTimming = maxJumpTime;
        isInGround = false;

        StartAttackAnimationEvent();
    }

    private void FixedUpdate()
    {

        if (isInGround && (Input.GetKeyDown(KeyCode.W) ||
         Input.GetKeyDown(KeyCode.UpArrow)))
        {
            jumpTimming = 0;
        }

        if (jumpTimming < maxJumpTime)
        {
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTimming += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.W) ||
           Input.GetKeyUp(KeyCode.UpArrow))
        {
            // Carga más fuerza mientras se mantiene presionada la tecla
            jumpTimming = maxJumpTime;
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
    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        playerRB.velocity = new Vector2(xDirection * maxSpeed, playerRB.velocity.y);

        if (MathF.Sign(xDirection) == 1 && playerSR.flipX) 
        { 
            playerSR.flipX = false; 
        }
        else if (MathF.Sign(xDirection) == -1 && !playerSR.flipX)
        {
            playerSR.flipX = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        { 
            isInGround = true;
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
    }
}
