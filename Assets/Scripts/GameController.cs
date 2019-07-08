using UnityEngine;
using UnityEngine.UI; //para acceder a objetos de interfaz 
using System.Collections;

public class GameController : MonoBehaviour {

	[Range (0.02f, 0.3f)]
	public float parallaxSpeed = 0.02f;
	public RawImage background;
	public RawImage platform;
	public GameObject uiIdle;
	public GameObject player;

	public enum EstadoDelJuego {Parado, Jugando};
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
			player.SendMessage ("PlayerState", "PlayerRun"); //envio mensaje a player
		}

		//juego en marcha
		else if (estadoDelJuego == EstadoDelJuego.Jugando) {
			Parallax ();
		}
	}

	//movimiento pantalla
	void Parallax(){
		float finalSpeed = parallaxSpeed * Time.deltaTime; 
		background.uvRect = new Rect (background.uvRect.x + finalSpeed, 0f, 1f, 1f); //Se mueve en x. El resto queda igual
		platform.uvRect = new Rect (platform.uvRect.x + finalSpeed*4, 0f, 1f, 1f); //Se mueve 4 veces mas rapido que el background
	}
}
