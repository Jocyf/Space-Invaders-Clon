using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Ship Movement")]
    public float speed = 30;
    public bool useMobileInput = true;
    public float sensivity = 0.5f;

    public float rightBound = 7f;
    public float leftBound = -7f;

    [Header("Fire Configuration")]
    public GameObject bulletPrefab;
    public Transform firePos;
    public float fireRatio = 0.2f;
    public bool isMobile = true;

    [Header("Explosion Configuration")]
    public GameObject explosionPrefab;
    public float destroyWaitTime = 2f;
    [Space(5)]
    public AudioClip destroyClip;

    private Transform myTransform;
    private Rigidbody2D myRigidbody2D;
    private float internalRatio = 0;

    private SpriteRenderer mySpriteRenderer;
    private Collider2D myCollider2D;

    private Vector3 originalPositon;
    private bool isPlayerActive = true;
    private bool isInitialized = false;

    private float h = 0;        // Horizontal key


    // Disparo del Player. Esta funcion sera llamada por el boton de disparo en movil
    public void PlayerFire()
    {
        if (internalRatio <= 0)
        {
            CreateBullet();
            internalRatio = fireRatio;
        }
    }

    public void SetHorizontal(float _h)
    {
        h = _h;
    }

    private void Start()
    {
        myTransform = transform;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();

        originalPositon = myTransform.position;
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized) return;

        SetPlayerActive(true);
    }

    void Update()
    {
        if (!isPlayerActive) return;

        if (internalRatio > 0) internalRatio -= Time.deltaTime;
        if ( (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump")) && !isMobile)
        {
            PlayerFire();
        }
    }

    void FixedUpdate()
    {
        if(!isPlayerActive) return;

        
        
        /*if (useMobileInput)
        {
            h = Input.GetAxis("Mouse X") * sensivity;
            if (h > 1)
                h = 1;
            else if (h < -1)
                h = -1;

            //Debug.Log("h: " + h.ToString() + " - Mouse Y: " + Input.GetAxis("Mouse Y").ToString());
        }*/
        if (!useMobileInput)
        {
            h = Input.GetAxis("Horizontal");
        }

        if ((transform.position.x < rightBound && transform.position.x > leftBound) ||
            (transform.position.x == rightBound && h < 0) || 
            (transform.position.x == leftBound && h > 0))
        {
            myRigidbody2D.velocity = new Vector2(h, 0) * speed;
        }
        else
            myRigidbody2D.velocity = Vector2.zero;

        CheckBounds();
    }

    private void CreateBullet()
    {
        GameObject obj = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
        //obj.transform.parent = this.transform;
    }

    private void CheckBounds()
    {
        // Security reachung the top/botton of the screen.
        if (transform.position.x > rightBound)
        {
            myTransform.position = new Vector2(rightBound, transform.position.y);
            myRigidbody2D.velocity = Vector2.zero;

        }
        else if (transform.position.x < leftBound)
        {
            myTransform.position = new Vector2(leftBound, transform.position.y);
            myRigidbody2D.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Bullet")) return;  // Colision con la barrera superior de la escena.

        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity); // Create the explosion
        Destroy(collision.gameObject);    // Destroy the bullet

        AudioManager.Instance.Play(destroyClip, this.transform);
        SpaceInvadersManager.Instance.Die(); // Decrease live count. TODO
        SetPlayerActive(false);
        StartCoroutine("_ReactivatePlayer");
    }

    IEnumerator _ReactivatePlayer()
    {
        yield return new WaitForSeconds(2.1f);
        if (SpaceInvadersManager.Instance.numlives > 0)
        {
            SetPlayerActive(true);
        }
    }

    private void SetPlayerActive(bool _active)
    {
        isPlayerActive = _active;
        mySpriteRenderer.enabled = _active;
        myCollider2D.enabled = _active;

        if(_active)
        {
            myTransform.position = originalPositon;
        }
    }
}
