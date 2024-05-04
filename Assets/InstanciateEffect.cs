using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateEffect : MonoBehaviour
{
    public GameObject effect;
    public bool destroy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InstanciatePrefab();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InstanciatePrefab();
        }
    }

    void InstanciatePrefab()
    {
        GameObject prefab = Instantiate(effect);
        prefab.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1f);

        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}
