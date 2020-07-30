using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject uiIdle;
	public GameObject uiScore;
	public GameObject buttonJump;
	public Text pointsText;
	public Text recordText;
	public GameObject player;
	public GameObject enemyGenerator;
	public InteractiveElement interactiveScreen;

	[Range (0.02f, 0.3f)]
	public float parallaxSpeed = 0.02f;
	public RawImage background;
	public RawImage platform;

	private AudioSource[] audios;
	private AudioSource backgroundMusic;
	private AudioSource recordAudio;
	public AudioClip gameMusic;

	private enum EstadoDelJuego {Parado, Jugando, Finalizado};
	private EstadoDelJuego estadoDelJuego = EstadoDelJuego.Parado;

	private int pointsCount = 0;

	// Use this for initialization
	void Start () 
	{
		//PlayerPrefs.DeleteKey ("Record"); //borrar record

		recordText.text = "Record: " + GetRecord().ToString (); 

		audios = GetComponents<AudioSource> ();
		backgroundMusic = audios [0];
		recordAudio = audios [1];
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool userAction =  Input.GetKey (KeyCode.Return); //KeyCode.Return: Enter

		//empieza el juego 
		if (estadoDelJuego == EstadoDelJuego.Parado && (userAction || interactiveScreen.click)) 
		{
			estadoDelJuego = EstadoDelJuego.Jugando;
			uiIdle.SetActive (false); //desactivo titulo e info

			uiScore.SetActive (true); //activo puntaje
			buttonJump.SetActive(true); //activo botón virtual de salto
			InvokeRepeating ("AccelerateTime", 6f, 6f); //acelero tiempo de juego, desde los 6'', cada 6''

			player.SendMessage ("PlayerState", "PlayerRun"); //envio mensaje a player para q empiece a correr
			player.GetComponentInChildren<ParticleSystem>().Play (); //activo el polvo al correr
			player.GetComponent<PlayerController> ().isActive = true; //activo al jugador. isActive: variable creada en PlayerController
			enemyGenerator.SendMessage ("GeneratorOn"); //envio mensaje a enemyGenerator para q empiece a generar

			backgroundMusic.clip = gameMusic; 
			backgroundMusic.Play ();
		}

		//juego en marcha
		else if (estadoDelJuego == EstadoDelJuego.Jugando) {
			Parallax ();

			//si player no esta activo (muriendo)
			if (player.GetComponent<PlayerController> ().isActive == false) {
				estadoDelJuego = EstadoDelJuego.Finalizado;
				backgroundMusic.Stop();
				CancelInvoke ("AccelerateTime");
				Time.timeScale = 1f;
				enemyGenerator.SendMessage ("GeneratorOff");
				if (pointsCount >= GetRecord ()) {
					SetRecord(pointsCount);
				}
				buttonJump.SetActive(false); //desactivo botón virtual de salto
			}
		}

		//juego finalizado
		else if (estadoDelJuego == EstadoDelJuego.Finalizado) 
		{
			//si player ha muerto 
			//isDead: variable creada en PlayerController, modificada en evento de PlayerDie.anim
			if (player.GetComponent<PlayerController> ().isDead) 
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

	void IncreasePoints() //llamado desde PlayerController, OnTriggerEnter2D() 
	{
		pointsCount ++;
		//Debug.Log ("puntos: " + pointsCount);
		pointsText.text = pointsCount.ToString ();

		if (pointsCount > GetRecord ())
		{ 
			if (pointsCount == GetRecord () + 1 ) 
			{
				Debug.Log ("nuevo record");
				recordText.GetComponent<Animator>().Play ("RecordBlink");
				recordAudio.Play ();
			}

			recordText.text = "Record: " + pointsCount.ToString ();
		}
	}

	void RestartGame()
	{
		//desde unity 5.3 en adelante:  reemplazar por SceneManager.LoadScene()
		Application.LoadLevel ("MainScene");
	}

	int GetRecord()
	{
		return PlayerPrefs.GetInt ("Record", 0);
	}

	void SetRecord(int currentPoints)
	{
		PlayerPrefs.SetInt ("Record", currentPoints);
	}
	
}
