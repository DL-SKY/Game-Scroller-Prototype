/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Кнопка меню прокачки =
*/

using UnityEngine;
using System.Collections;


public class ButtonToUpgrade : MonoBehaviour 
{
	ItemToUpgrade scriptItem2Up;

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		scriptItem2Up = GetComponentInParent<ItemToUpgrade>();
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
	
	}
	//------------------------------------------------
	//Обработчик щелчка по кнопке
	void OnClick()
	{
		//Звук
		SoundManager.instance.PlayOnClick();

		scriptItem2Up.ToUpgrade();
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
