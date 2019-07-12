using UnityEngine;
using UnityEngine.UI; //para acceder a objetos de interfaz 
using System.Collections;

public class GameController : MonoBehaviour {

	//variables publicas que asigno desde el editor
	[Range (0.02f, 0.3f)]
	public float parallaxSpeed = 0.02f;
	public RawImage background;
	public RawImage platform;
	public GameObject uiIdle;
	public GameObject player;
	public GameObject enemyGenerator;

	public enum EstadoDelJuego {Parado, Jugando, Finalizado};
	public EstadoDelJuego estadoDelJuego = EstadoDelJuego.Parado;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//empieza el juego 
		if (estadoDelJuego == EstadoDelJuego.Parado && Input.GetKey (KeyCode.Space)) {
			estadoDelJuego = EstadoDelJuego.Jugando;
			uiIdle.SetActive (false); //desactivo titulo e info
			player.GetComponent<PlayerController>().isActive = true; //activo al jugador. isActive: variable creada en PlayerController
			player.SendMessage ("PlayerState", "PlayerRun"); //envio mensaje a player para q empiece a correr
			enemyGenerator.SendMessage ("GeneratorOn"); //envio mensaje a enemyGenerator para q empiece a generar
		}

		//juego en marcha
		else if (estadoDelJuego == EstadoDelJuego.Jugando) {
			Parallax ();

			//si player ha muerto
			if (player.GetComponent<PlayerController>().isActive == false) {
				estadoDelJuego = EstadoDelJuego.Finalizado;
			}
		}

		else if (estadoDelJuego == EstadoDelJuego.Finalizado) {
			enemyGenerator.SendMessage ("GeneratorOff");
		}
	}

	//movimiento pantalla
	void Parallax(){
		float finalSpeed = parallaxSpeed * Time.deltaTime; 
		background.uvRect = new Rect (background.uvRect.x + finalSpeed, 0f, 1f, 1f); //Se mueve en x. El resto queda igual
		platform.uvRect = new Rect (platform.uvRect.x + finalSpeed*4, 0f, 1f, 1f); //Se mueve 4 veces mas rapido que el background
	}
}
