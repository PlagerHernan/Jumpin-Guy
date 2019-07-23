using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //para usar IPointerDownHandler y IPointerUpHandler

public class InteractiveElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  //IPointerClickHandler
{
	public bool click = false;

	public void OnPointerDown  (PointerEventData evenData)
	{
		//Debug.Log ("entró a OnPointerDown!");
		click = true;
	}

	public void OnPointerUp  (PointerEventData evenData)
	{
		//Debug.Log ("entró a OnPointerUp!");
		click = false;
	}

//	public void OnPointerClick  (PointerEventData evenData)
//	{
//		Debug.Log ("entró a OnPointerClick!");
//		click = true;
//	}

}
