using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float speed = 5;

    public AnimationCurve ratiobasedMult = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1.0f, 1.1f));

    private Rigidbody2D myRigidbody2D;


    void Start ()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();

        // Calculate the final velocity depending on the enemies movement ratio
        float incr = ratiobasedMult.Evaluate(1.0f-EnemiesManager.Instance.ratio);
        myRigidbody2D.velocity = -Vector2.up * speed * incr;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Untagged")) return;  // Colision con la barrera inferior de la escena.

        Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        Destroy(this.gameObject);                          // Destroy the bullet
    }
}
