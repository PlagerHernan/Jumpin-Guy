using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject game;

	public bool isActive = false; 
	public bool isDead = false;

	private Animator _animator;

	private AudioSource audioSource;
	public AudioClip dieAudio;
	public AudioClip jumpAudio;
	public AudioClip pointAudio;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();

		//bool blockedButtons = true;
	}
	
	// Update is called once per frame
	void Update () {

		bool userAction = Input.GetKey (KeyCode.Space) || Input.GetMouseButton(0);
		float positionY = GetComponent<Transform>().position.y; 

		//salto
		if (isActive && userAction) {

			PlayerState("PlayerJump");

			audioSource.clip = jumpAudio;
			//si el jugador se encuentra en el suelo, reproduzco el sonido de salto
			if (positionY == -3.5f) 
			{
				audioSource.Play ();
			}
		}
	}

	//animacion
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
			other.GetComponentInChildren<BoxCollider2D>().enabled = false; //desactivo collider de point, para q no pueda sumar punto al morir
			isActive = false;
			Time.timeScale = 0.2f;
			PlayerState ("PlayerDie");
			StopDust();

			audioSource.clip = dieAudio;
			audioSource.Play ();
		}

		else if (other.gameObject.tag == "Point") 
		{
			game.SendMessage ("IncreasePoints");
			GameObject.Destroy (other.gameObject); //destruyo el point para q no se pueda sumar puntos pasando dos veces

			audioSource.clip = pointAudio;
			audioSource.Play ();
		}
	}

	//metodo llamado desde evento de PlayerDie.anim
	void Die()
	{
		//Debug.Log ("estoy muerto");
		isDead = true;
	}

	void PlayDust()
	{
		GetComponentInChildren<ParticleSystem> ().Play ();
	}

	void StopDust()
	{
		GetComponentInChildren<ParticleSystem> ().Stop ();
	}
}
