using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour {

    public GameObject barrierPrefab;

    public int numBarriers = 4;
    public Vector3 startPoint = Vector3.zero;   // Distancia "buena"  (-5.25, -3.2, 0)
    public float distance = 3.5f;

    private GameObject[] Barries;

    #region Singleton
    public static BarrierManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void Reset()
    {
        DestroyBarriers();
    }

    public void StartPlaying()
    {
        GenerateBarriers();
    }

    void Start ()
    {
       //StartPlaying();
    }

    private void GenerateBarriers()
    {
        Barries = new GameObject[numBarriers];
        
        for (int i = 0; i < numBarriers; i++)
        {
            Vector3 displ = new Vector3(distance, 0, 0) * i;
            GameObject obj = Instantiate(barrierPrefab, startPoint + displ, Quaternion.identity);
            obj.transform.parent = this.transform;

            Barries[i] = obj;
        }
    }

    private void OnDisable()
    {
        DestroyBarriers();
    }

    private void DestroyBarriers()
    {
        if (Barries == null) return;

        for (int i = Barries.Length - 1; i >= 0; i--)
        {
            Destroy(Barries[i]);
        }
    }
}
