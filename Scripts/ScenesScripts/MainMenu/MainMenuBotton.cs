/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт кнопок Главного Меню =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class MainMenuBotton : MonoBehaviour 
{
	public enum ButtonType {Ok = 0, Cancel = 1, Exit = 2, Config = 3, Upgrade = 4,
							Manual = 5, Play = 6, Credits = 7};

	[Header("Main")]
	public ButtonType btnType = 0;					//Тип кнопки

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
	
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


		//Ok
		if (btnType == ButtonType.Ok)
		{

		}
		//Cancel
		if (btnType == ButtonType.Cancel)
		{ 
		
		}
		//Exit
		if (btnType == ButtonType.Exit)
		{
			SceneMainMenu0 sceneSceneMainMenu = GameObject.FindObjectOfType<SceneMainMenu0>();
			sceneSceneMainMenu.goMenuExitFrame.transform.localScale = Vector3.one;
		}
		//Config
		if (btnType == ButtonType.Config)
		{
			SceneMainMenu0 sceneSceneMainMenu = GameObject.FindObjectOfType<SceneMainMenu0>();
			sceneSceneMainMenu.goConfigFrame.transform.localScale = Vector3.one;
		}
		//Upgrade
		if (btnType == ButtonType.Upgrade)
		{
            SceneMainMenu0 sceneSceneMainMenu = GameObject.FindObjectOfType<SceneMainMenu0>();
            sceneSceneMainMenu.goUpgradeMenu.transform.localScale = Vector3.one;
        }
		//Manual
		if (btnType == ButtonType.Manual)
		{

		}
		//Play
		if (btnType == ButtonType.Play)
		{
			SceneManager.LoadScene("GameScene0");
		}
		//Credits
		if (btnType == ButtonType.Credits)
		{
			SceneManager.LoadScene("CreditsScene0");
		}



		//...
	}
	//------------------------------------------------
	//------------------------------------------------
}
