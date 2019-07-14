using UnityEngine;
using UnityEngine.UI; //para acceder a objetos de interfaz 
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//variables publicas que asigno desde el editor
	[Range (0.02f, 0.3f)]
	public float parallaxSpeed = 0.02f;
	public RawImage background;
	public RawImage platform;
	public GameObject uiIdle;
	public GameObject player;
	public GameObject enemyGenerator;

	private AudioSource gameMusic;

	private enum EstadoDelJuego {Parado, Jugando, Finalizado};
	private EstadoDelJuego estadoDelJuego = EstadoDelJuego.Parado;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		bool userAction = Input.GetKey (KeyCode.Return);

		//empieza el juego 
		if (estadoDelJuego == EstadoDelJuego.Parado && userAction) {
			estadoDelJuego = EstadoDelJuego.Jugando;
			uiIdle.SetActive (false); //desactivo titulo e info
			player.SendMessage ("PlayerState", "PlayerRun"); //envio mensaje a player para q empiece a correr
			player.GetComponentInChildren<ParticleSystem>().Play (); //activo el polvo al correr
			player.GetComponent<PlayerController> ().isActive = true; //activo al jugador. isActive: variable creada en PlayerController
			enemyGenerator.SendMessage ("GeneratorOn"); //envio mensaje a enemyGenerator para q empiece a generar

			gameMusic = gameObject.GetComponent<AudioSource>();
			gameMusic.Play ();

			InvokeRepeating ("AccelerateTime", 6f, 6f); //acelero tiempo de juego, desde los 6'', cada 6''
		}

		//juego en marcha
		else if (estadoDelJuego == EstadoDelJuego.Jugando) {
			Parallax ();

			//si player no esta activo (muriendo)
			if (player.GetComponent<PlayerController> ().isActive == false) {
				estadoDelJuego = EstadoDelJuego.Finalizado;
				gameMusic.Stop();
				CancelInvoke ("AccelerateTime");
				Time.timeScale = 1f;
				enemyGenerator.SendMessage ("GeneratorOff");
			}
		}

		//juego finalizado
		else if (estadoDelJuego == EstadoDelJuego.Finalizado) 
		{
			//si player ha muerto 
			//isDead: variable creada en PlayerController, modificada en evento de PlayerDie.anim
			if (player.GetComponent<PlayerController> ().isDead && userAction) 
			{
				RestartGame ();
			}
		}
	}

	//movimiento pantalla
	void Parallax(){
		float finalSpeed = parallaxSpeed * Time.deltaTime; 
		background.uvRect = new Rect (background.uvRect.x + finalSpeed, 0f, 1f, 1f); //Se mueve en x. El resto queda igual
		platform.uvRect = new Rect (platform.uvRect.x + finalSpeed*4, 0f, 1f, 1f); //Se mueve 4 veces mas rapido que el background
	}

	void AccelerateTime(){
		Time.timeScale += 0.25f;
		Debug.Log (Time.timeScale);
	} 

	void RestartGame()
	{
		//desde unity 5.3 en adelante:  reemplazar por SceneManager.LoadScene()
		Application.LoadLevel ("MainScene");
	}
}
