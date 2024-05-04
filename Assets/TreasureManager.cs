using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public GameObject ePanel;
    public int coinsAmount;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { 
            ePanel.SetActive(true);
        }

        if (collision.gameObject.CompareTag("DamageZone"))
        {
            Debug.Log(isOpen);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ePanel.SetActive(false);
        }
    }
}
