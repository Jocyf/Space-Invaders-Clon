using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public float speed = 5;
    public GameObject explosionPrefab;


    private Rigidbody2D myRigidbody2D;


    void Start ()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.velocity = Vector2.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Untagged")) return;  // Colision con la barrera superior de la escena.

        Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        Destroy(this.gameObject);                          // Destroy the bullet
    }
}
