using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour {

    public EnemiesManager enemiesManager;

    private float _ratio = 1f;
    private AudioSource myAS;

	void Start ()
    {
        myAS = GetComponent<AudioSource>();
        _ratio = enemiesManager.ratio;
        InvokeRepeating("ChangeRatio", 1f, 0.1f);
    }

    /*private void Update()
    {
        ChangeRatio();
    }*/

    void ChangeRatio()
    {
	    if(_ratio != enemiesManager.ratio)
        {
            _ratio = enemiesManager.ratio;
            myAS.pitch = 1 + (1 - _ratio);
        }
	}
}
