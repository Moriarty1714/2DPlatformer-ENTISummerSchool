using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public GameObject ePanel;
    public GameObject coinPrefab;

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

        if (collision.gameObject.CompareTag("DamageZone") && !isOpen)
        {
            for (int i = 0; i < coinsAmount; i++)
            {
                GameObject coin = Instantiate(coinPrefab);
                coin.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
                Rigidbody2D coinRB = coin.GetComponentInChildren<Rigidbody2D>();
                coinRB.AddForce(new Vector2(Random.Range(-3f, 3f), Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            isOpen = true;
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
