using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float velocidad = 2f;
	private Rigidbody2D _rb2d;

	// Use this for initialization
	void Start () {
		//muevo el enemy hacia la izquierda
		_rb2d = gameObject.GetComponent<Rigidbody2D> ();
		_rb2d.velocity = Vector2.left * velocidad;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//si colisiona contra el trigger Destroyer, destruyo el enemy
		if (other.gameObject.tag == "Destroyer") {
			GameObject.Destroy (gameObject);
		}
	}
}
