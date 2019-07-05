using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [Header("Main Configuration")]
    public Sprite[] enemySprites;
    public float Bounds = 7.5f;
    public int score = 10;
    [Space(5)]
    public AudioClip destroyClip;

    [Header("Fire Configuration")]
    public GameObject[] bulletPrefab;
    public float fireRatio = 5.0f;
    [Range(1, 100)] public int fireProbability = 10;
    public float initialFireTime = 15.0f;

    

    private Transform myTransform;
    private SpriteRenderer mySpriteRenderer;
    private BoxCollider2D myCollider2D;


    public bool MoveEnemy(Vector3 displ)
    {
        if (myTransform.position.x >= -Bounds && myTransform.position.x <= Bounds)
        {
            myTransform.Translate(displ);
            ChangeSprite();
        }

        return IsBoundReached(displ.y);
    }

	IEnumerator Start ()
    {
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<BoxCollider2D>();

        float waitTime = Random.Range(initialFireTime * 0.1f, initialFireTime);
        yield return new WaitForSeconds(waitTime);

        StartCoroutine("_EnemyFireTimed");
    }

    private bool IsBoundReached(float displY)
    {
        return (Mathf.Abs(myTransform.position.x) == Mathf.Abs(Bounds)) && displY == 0;
    }

    private void ChangeSprite()
    {
        mySpriteRenderer.sprite = mySpriteRenderer.sprite == enemySprites[0] ? enemySprites[1] : enemySprites[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("PlayerBullet") && !collision.collider.CompareTag("Player")) return;


        SpaceInvadersManager.Instance.AddScore(score);
        mySpriteRenderer.enabled = false;
        myCollider2D.enabled = false;
        EnemiesManager.Instance.DestroyEnemy(this.transform);  // Destroy the enemy and update the enemy count

        if(collision.collider.CompareTag("Player"))
        {
            SpaceInvadersManager.Instance.numlives = 1;
            SpaceInvadersManager.Instance.Die();
        }

        AudioManager.Instance.Play(destroyClip, this.transform);
        Destroy(collision.gameObject);                          // Destroy the bullet
        
    }

    private IEnumerator _EnemyFireTimed()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRatio);
            int n = Random.Range(1, 100);
            if (n < fireProbability)
            {
                CreateEnemyBullet();
            }
        }
    }

    private void CreateEnemyBullet()
    {
        int n = Random.Range(0, bulletPrefab.Length - 1);
        GameObject obj = Instantiate(bulletPrefab[n], this.transform.position, Quaternion.identity);
        //obj.transform.parent = this.transform;
    }

}
