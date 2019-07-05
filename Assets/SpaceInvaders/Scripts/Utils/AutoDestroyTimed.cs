using UnityEngine;
using System.Collections;

public class AutoDestroyTimed : MonoBehaviour
{
	public float destroyTime = 5f;

	void OnEnable()
	{
		StartCoroutine("_CheckIfAlive");
	}
	
	IEnumerator _CheckIfAlive ()
	{
		yield return new WaitForSeconds(destroyTime);
		Destroy(this.gameObject);
	}
}
