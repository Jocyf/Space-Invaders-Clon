using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierUnit : MonoBehaviour
{
    public GameObject explosionPrefab;

    public Sprite explosionSprite;
    public int diameter = 20;

    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;

    private Sprite originalSprite;
    private Texture2D explosionTex;

    float _width;
    float _height;


    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();
        originalSprite = mySpriteRenderer.sprite;

        _width = myCollider2D.bounds.size.x * originalSprite.pixelsPerUnit;
        _height = myCollider2D.bounds.size.y * originalSprite.pixelsPerUnit;

        explosionTex = explosionSprite.texture;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Contains("Bullet")) return;  // Colision con la barrera superior de la escena.

        Vector3 hitPoint = collision.transform.position;
        //Debug.Log("OnTriggerEnter2D -> hitPoint: " + hitPoint);

        if (CheckPoint(hitPoint, collision)) return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.tag.Contains("Bullet")) return;  // Colision con la barrera superior de la escena.

        /*if (collision.contacts.Length > 0)
        {
            //Debug.Log("PointsColliding: " + collision.contacts.Length + " - First point that collided, contacts[0]: " + collision.contacts[0].point);
            MakeAHoleInBarrier(collision.contacts[0].point);
        }

        // Usado en 2017.2.5 Pero NO funciona (problema de Unity).
        //ContactPoint2D[] myContacts = new ContactPoint2D[1];
        //int numContacts = collision.GetContacts(myContacts);
        //Debug.Log("GetContacts Size 1: " + myContacts[0].point + " Array Length: " + numContacts);*/


        Vector3 hitPoint = collision.transform.position;
        //Debug.Log("OnTriggerEnter2D -> hitPoint: " + hitPoint);

        if (CheckPoint(hitPoint, collision)) return;
    }

    private bool CheckPoint(Vector2 hitPoint, Collider2D other)
    {
        // See if the raycast hits, and if the hit point was a solid color which results in the bunker manager making a splat at that point
        RaycastHit2D hit = Physics2D.Raycast(hitPoint, Vector2.up, 5.0f);

        //Debug.Log("CheckPoint -> hitPoint: " + hitPoint+" - CheckPoint-> Raycast hit: " + hit.point);
        if (hit.point != Vector2.zero && IsOpaquePixel(hitPoint))
        {
            //MakeAHoleInBarrierv1(hitPoint);
            //Instantiate(explosionPrefab, hit.point, Quaternion.identity);
            Destroy(other.gameObject);
            return true;
	    }

	    return false;
    }

    private bool IsOpaquePixel(Vector2 hitPoint)
    {
        int radius = diameter / 2;

        Vector2 localPos = hitPoint - (Vector2)transform.position;
        localPos += (Vector2)mySpriteRenderer.sprite.bounds.extents;
        localPos *= mySpriteRenderer.sprite.pixelsPerUnit;

        Vector2 pixelUV = Vector2.zero;
        pixelUV.x = localPos.x;// * myCollider2D.bounds.size.x;  //textureWidth = 155
        pixelUV.y = localPos.y;// * myCollider2D.bounds.size.y; //textureHeight = 113

        // If the hit point is transparent, we didn't hit anything
        Texture2D tex = mySpriteRenderer.sprite.texture;
        _width = tex.width;
        _height = tex.height;

        int centerX = (int)pixelUV.x;
        int centerY = (int)pixelUV.y;
        //int a, b = 0;

        for (int x = centerX - radius; x < centerX + radius; x++)
        {
            for (int y = centerY - radius; y < centerY + radius; y++)
            {
                if (x < 0 || y < 0 || x >= _width || y >= _height) continue;

                if (tex.GetPixel(x, y).a != 0)
                {
                    //Debug.Log("Collided with opaque pixel: " + x.ToString() + " y: " + y.ToString());
                    MakeAHoleInBarrierv2(new Vector2(x, y));
                    return true;
                }

                //=======
                // Getting a circle..
                /*a = x - centerX;
                b = y - centerY;

                if ((a * a) + (b * b) <= (radius * radius))
                    MakeAHoleInBarrierv2(new Vector2(x, y));*/
            }
        }

        return false;
    }

    // Use an explosion sprite to make the hole
    private void MakeAHoleInBarrierv2(Vector2 contactPoint)
    {
        int radius = diameter / 2;
        Texture2D tex = mySpriteRenderer.sprite.texture;

        //Replace texture
        Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        newTex.SetPixels32(tex.GetPixels32());

        int _explosionWidth = explosionTex.width;
        int _explosionHeight = explosionTex.height;
        //Debug.Log("_explosionWidth: " + _explosionWidth.ToString() + " _explosionHeight: " + _explosionHeight.ToString());

        int y1 = 0;
        for (int y = (int)contactPoint.y - _explosionHeight/4; y < (int)contactPoint.y + _explosionHeight/2; y++)   /* NO es exacto,*/
        {
            int x1 = 0;
            for (int x = (int)contactPoint.x - _explosionWidth/2; x < (int)contactPoint.x + _explosionWidth/2; x++)
            {
                //Debug.Log("x: " + x.ToString() + " y: " + y.ToString());
                if (x < 0 || y < 0 || x >= _width || y >= _height) { x1++; continue; }
                if (explosionTex.GetPixel(x1, y1).a > 0.5f)
                {
                    newTex.SetPixel(x, y, Color.clear);
                }
                x1++;
            }
            y1++;
        }


        newTex.Apply();

        Rect myRect = mySpriteRenderer.sprite.rect;
        //Debug.Log("myRect: " + myRect);
        mySpriteRenderer.sprite = Sprite.Create(newTex, myRect, new Vector2(0.5f, 0.5f));
    }


    // This makes a circle in the barrier
    private void MakeAHoleInBarrierv1(Vector2 contactPoint)
    {
        int radius = diameter / 2;
        Texture2D tex = mySpriteRenderer.sprite.texture;

        //Replace texture
        Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        newTex.SetPixels32(tex.GetPixels32());

        int _explosionWidth = explosionTex.width;
        int _explosionHeight = explosionTex.height;

        int a, b = 0;
        for (int y = (int)contactPoint.y - radius; y < (int)contactPoint.y + radius; y++)
        {
            for (int x = (int)contactPoint.x - radius; x < (int)contactPoint.x + radius; x++)
            {
                if (x < 0 || y < 0 || x >= _width || y >= _height) continue;
                //newTex.SetPixel(x, y, Color.clear);

                //=======
                // Getting a circle..
                a = x - (int)contactPoint.x;
                b = y - (int)contactPoint.y;

                if ((a * a) + (b * b) <= (radius * radius))
                    newTex.SetPixel(x, y, Color.clear);
            }
        }

        newTex.Apply();

        Rect myRect = mySpriteRenderer.sprite.rect;
        //Debug.Log("myRect: " + myRect);
        mySpriteRenderer.sprite = Sprite.Create(newTex, myRect, new Vector2(0.5f, 0.5f));
    }

}
