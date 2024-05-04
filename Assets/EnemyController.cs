using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D enemyRB;
    public SpriteRenderer enemySR;

    public float jumpForce;
    public float timeBtwnJumps;
    private int xDirection;

    private float lastJumpTime;

    public Transform pointA;
    public Transform pointB;


    void Start()
    {
        lastJumpTime = Time.time;
        xDirection = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < pointA.position.x && !enemySR.flipX)
        {
            xDirection = 1;
            enemySR.flipX = true;
        }

        if (transform.position.x > pointB.position.x && enemySR.flipX )
        {
            xDirection = -1;
            enemySR.flipX = false;
        }

        
       
        if (  Time.time > lastJumpTime + timeBtwnJumps)
        {
            enemyRB.AddForce(new Vector2(jumpForce*xDirection, jumpForce), ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyRB.velocity = Vector2.zero;
        }
    }


}
