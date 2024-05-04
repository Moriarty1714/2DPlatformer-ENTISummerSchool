using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxMovement : MonoBehaviour
{
    [SerializeField] float paralaxMultiplay;

    Transform cameraTransform;
    Vector3 cameraPos;
    float spriteWitdh, startPos;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraPos = cameraTransform.position;
        spriteWitdh = GetComponent<SpriteRenderer>().bounds.size.x;
        startPos = transform.position.x;
    }

    private void LateUpdate()
    {
        float deltaX = (cameraTransform.position.x - cameraPos.x) * paralaxMultiplay;
        float moveAmount = cameraTransform.position.x * (1 - paralaxMultiplay);
        transform.Translate(new Vector3(deltaX, 0, 0)); 
        cameraPos = cameraTransform.position;

        if(moveAmount > startPos + spriteWitdh)
        {
            transform.Translate(new Vector3(spriteWitdh, 0, 0));
            startPos += spriteWitdh;
        }
        else if (moveAmount < startPos - spriteWitdh)
        {
            transform.Translate(new Vector3(-spriteWitdh, 0, 0));
            startPos -= spriteWitdh;
        }
    }

}
