/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт кнопки сцены Благодарности =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class CreditsButton : MonoBehaviour 
{
	SceneCredits credits;							//Ссылка на скрипт сцены

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		credits = GameObject.FindObjectOfType<SceneCredits>();
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
	
	}
	//------------------------------------------------
	//Обработчик нажатия на кнопку
	void OnClick()
	{
		//Звук
		SoundManager.instance.PlayOnClick();

		SceneManager.LoadScene("MainMenu0");
	}
	//------------------------------------------------
	//------------------------------------------------
}
