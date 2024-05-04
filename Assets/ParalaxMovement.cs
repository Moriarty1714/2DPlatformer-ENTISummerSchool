using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] bool scrollLeft;

    float textureWidht;
    void Start()
    {
        TextureValues();
        
    }

     void TextureValues()
     {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureWidht = sprite.texture.width / sprite.pixelsPerUnit;
     }

    void Scroll()
    {
        float velX = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(velX, 0, 0);
    }

    void CheckReset() 
    { 
        if( (Mathf.Abs(transform.position.x) - textureWidht) > 0)
        {

        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
