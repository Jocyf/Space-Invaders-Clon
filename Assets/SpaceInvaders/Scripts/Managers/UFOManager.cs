using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOManager : MonoBehaviour {

    public GameObject omniPrefab;
    public float omniRatio = 5.0f;
    [Range(1, 100)] public int omniProbability = 10;
    public float initialOmniTime = 15.0f;

    public Transform UFOInitialPoint;
    private GameObject UFOObject;

    public void Reset()
    {
        Destroy(UFOObject);
        StopCoroutine("_UFOFireTimed");
    }

    public void StartPlaying()
    {
        StartCoroutine("_StartPlayingTimed");
    }

    IEnumerator _StartPlayingTimed()
    {
        float waitTime = Random.Range(initialOmniTime * 0.1f, initialOmniTime);
        yield return new WaitForSeconds(waitTime);

        StartCoroutine("_UFOFireTimed");
    }

    private void Start()
    {
        //StartPlaying();
    }

    private IEnumerator _UFOFireTimed()
    {
        while (true)
        {
            yield return new WaitForSeconds(omniRatio);
            int n = Random.Range(1, 100);
            if (n < omniProbability)
            {
                CreateUFO();
            }
        }
    }

    private void CreateUFO()
    {
        UFOObject = Instantiate(omniPrefab, UFOInitialPoint.position, Quaternion.identity);
        //obj.transform.parent = this.transform;
    }
}
