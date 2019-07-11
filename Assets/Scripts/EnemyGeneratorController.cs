using UnityEngine;
using System.Collections;

public class EnemyGeneratorController : MonoBehaviour {

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateEnemy ()
	{
		Instantiate (enemyPrefab, gameObject.transform.position, gameObject.transform.rotation);
	}

	void GeneratorOn ()
	{
		InvokeRepeating ("CreateEnemy", 1f, 2.5f);
	}

	void GeneratorOff ()
	{
		CancelInvoke ("CreateEnemy");
	}
}
