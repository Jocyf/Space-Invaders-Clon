using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOShip : MonoBehaviour
{
    [Header("Main Configuration")]
    public float speed = 7f;
    public float Bounds = 8.5f;
    public int score = 40;

    public GameObject explosionPrefab;

    private Transform myTransform;
    private Rigidbody2D myRigidbody2D;

    private SpriteRenderer mySpriteRenderer;
    private BoxCollider2D myCollider2D;


	void Start ()
    {
        myTransform = transform;    

        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.velocity = Vector2.right * speed;
    }

    void Update()
    {
        if(IsBoundReached())
        {
            Destroy(this.gameObject);
        }
    }

    private bool IsBoundReached()
    {
        return myTransform.position.x >= Bounds;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("PlayerBullet")) return;

        if (SpaceInvadersManager.Instance.numlives < 3)
        {
            SpaceInvadersManager.Instance.AddExtraLive();  // Add extra live
        }
        else
        {
            SpaceInvadersManager.Instance.AddScore(score);
        }

        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

        Destroy(collision.gameObject);   // Destroy the bullet
        Destroy(this.gameObject);       // Destroy the UFO (this object)
    }

}
