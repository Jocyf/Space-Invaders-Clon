// Create 2D collider based on non transparent pixels
// https://forum.unity.com/threads/create-2d-collider-based-on-non-transparent-pixels.390051/

// 2D ColliderGen. Asset Store: https://forum.unity.com/threads/released-2d-collidergen-generate-precise-polygon-colliders-for-your-2d-sprites.174072/
// https://forum.unity.com/threads/released-2d-collidergen-generate-precise-polygon-colliders-for-your-2d-sprites.174072/



// Here is my code below.. It's probably quite messy but it works for getting a list of distinct Vector2s that surround the edge of the 
// non transparent texture pixels as shown in a couple of posts above.
// I first go from bottom to top to check for non transparent edges, and then from left to right so the points aren't going to be in the correct order.

// Using an IComparer I found online somewhere It organizes the Vector2's in a clockwise rotation. 
// As you can see (Apart from it being upside down and back to front) it does represent the graphic below pretty well. 
// But it still seems a little wrong, does anybody know the correct IComparer method I could use to generate the outline correctly? This is the code so far below:

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MHSTexture2DColliderMaker : MonoBehaviour
{
    public void GenerateColliderFromTexture(Texture2D texture)
    {
        bool FoundEdge = false;
        int LastX = -1;
        int LastY = -1;

        List<Vector2> vectors = new List<Vector2>();

        Color32[] colors = texture.GetPixels32();

        // Find edges on Y
        for (int X = 0; X < texture.width; X++)
        {
            FoundEdge = false;
            for (int Y = 0; Y < texture.height; Y++)
            {
                if (colors[(Y * texture.width) + X].a != 205)
                {
                    if ((colors[(Y * texture.width) + X].a > 0 && !FoundEdge) || (colors[(Y * texture.width) + X].a == 0 && FoundEdge) || Y == 0 || Y == texture.height - 1)
                    {
                        vectors.Add(new Vector2(X, Y));
                        FoundEdge = !FoundEdge;
                    }
                }
                else if (colors[(Y * texture.width) + X].a == 205 && FoundEdge)
                {
                    vectors.Add(new Vector2(LastX, LastY));
                    FoundEdge = false;
                }
                LastX = X;
                LastY = Y;
            }
        }
        // Find edges on X
        for (int Y = 0; Y < texture.height; Y++)
        {
            FoundEdge = false;
            for (int X = 0; X < texture.width; X++)
            {
                if (colors[(Y * texture.width) + X].a != 205)
                {
                    if ((colors[(Y * texture.width) + X].a > 0 && !FoundEdge) || (colors[(Y * texture.width) + X].a == 0 && FoundEdge))
                    {
                        vectors.Add(new Vector2(X, Y));
                        FoundEdge = !FoundEdge;
                    }
                }
                else if (colors[(Y * texture.width) + X].a == 205 && FoundEdge)
                {
                    vectors.Add(new Vector2(LastX, LastY));
                    FoundEdge = false;
                }
                LastX = X;
                LastY = Y;
            }
        }

        vectors = vectors.Distinct<Vector2>().ToList();

        vectors.Sort(new ClockwiseComparer(Vector2.zero));

        // For testing (Draws white border around texture)
        foreach (Vector2 vector in vectors)
            texture.SetPixel((int)vector.x, (int)vector.y, new Color(1, 1, 1, 1));

        texture.Apply();

        // Create collider
        Vector2[] vectorsArray = new Vector2[vectors.Count() + 1];
        for (int I = 0; I < vectors.Count(); I++)
            vectorsArray[I] = vectors[I];

        vectorsArray[vectors.Count()] = vectors[0];

        this.gameObject.AddComponent<PolygonCollider2D>().SetPath(0, vectorsArray);
    }
}

/// <summary>
///     ClockwiseComparer provides functionality for sorting a collection of Vector2s such
///     that they are ordered clockwise about a given origin.
/// </summary>
public class ClockwiseComparer : IComparer<Vector2>
{
    private Vector2 m_Origin;

    /// <summary>
    ///     Gets or sets the origin.
    /// </summary>
    /// <value>The origin.</value>
    public Vector2 origin { get { return m_Origin; } set { m_Origin = value; } }

    /// <summary>
    ///     Initializes a new instance of the ClockwiseComparer class.
    /// </summary>
    /// <param name="origin">Origin.</param>
    public ClockwiseComparer(Vector2 origin)
    {
        m_Origin = origin;
    }

    /// <summary>
    ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    public int Compare(Vector2 first, Vector2 second)
    {
        return IsClockwise(first, second, m_Origin);
    }

    /// <summary>
    ///     Returns 1 if first comes before second in clockwise order.
    ///     Returns -1 if second comes before first.
    ///     Returns 0 if the points are identical.
    /// </summary>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    /// <param name="origin">Origin.</param>
    public static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
    {
        if (first == second)
            return 0;

        Vector2 firstOffset = first - origin;
        Vector2 secondOffset = second - origin;

        float angle1 = Mathf.Atan2(firstOffset.x, firstOffset.y);
        float angle2 = Mathf.Atan2(secondOffset.x, secondOffset.y);

        if (angle1 < angle2)
            return -1;

        if (angle1 > angle2)
            return 1;

        // Check to see which point is closest
        return (firstOffset.sqrMagnitude < secondOffset.sqrMagnitude) ? -1 : 1;
    }
}