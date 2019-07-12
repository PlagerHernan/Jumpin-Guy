using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Animator _animator;
	public bool isActive = false; //variable para controlar si el jugador (y el juego) estan activos o no
	public bool isDead = false;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool userAction = Input.GetKey (KeyCode.Space) || Input.GetMouseButton(0);

		if (isActive == true && userAction) {
			PlayerState("PlayerJump");
		}
	}

	void PlayerState(string state = null)
	{
		if (state != null) {
			_animator.Play(state);
		}
	} 

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy") {
			//Debug.Log ("me muerooooo");
			isActive = false;
			PlayerState ("PlayerDie");
		}
	}

	void Die()
	{
		Debug.Log ("estoy muerto");
		isDead = true;
	}
}
