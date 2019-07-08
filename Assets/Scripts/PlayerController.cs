using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PlayerState(string state = null)
	{
		if (state != null) {
			_animator.Play(state);
		}


	} 
}
