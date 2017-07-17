/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт кнопок меню Игровой сцены =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class MenuButton : MonoBehaviour 
{
	public enum ButtonType { Ok = 0, Cancel = 1, ExitToMainMenu = 2, Manual = 3,
							 Config = 4, Upgrade = 5};

	[Header("Main")]
	public ButtonType btnType = 0;					//Тип кнопки
    public bool bMainMenu = false;                  //Флаг использования меню в сцене Гланого Меню


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
			//Если сцена - Главное меню
            if (bMainMenu)
            {
                SceneMainMenu0 sceneMainMenuScene = GameObject.FindObjectOfType<SceneMainMenu0>();
                sceneMainMenuScene.goConfigFrame.transform.localScale = Vector3.zero;
                return;
            }

			//Если сцена - Игра
			//Нормализация времени, закрытие меню
			Time.timeScale = 1.0f;
			SceneGameScene0 sceneGameScene = GameObject.FindObjectOfType<SceneGameScene0>();
			sceneGameScene.goMenuFrame.transform.localScale = Vector3.zero;
			sceneGameScene.goConfigFrame.transform.localScale = Vector3.zero;
		}
		//Exit to MainMenu
		if (btnType == ButtonType.ExitToMainMenu)
		{
			//Нормализация времени
			Time.timeScale = 1.0f;
			//Сохраняем данные
			SceneGameScene0 sceneGameScene = GameObject.FindObjectOfType<SceneGameScene0>();
			sceneGameScene.SaveConfiguration();
			//Переход в главное меню
			SceneManager.LoadScene("MainMenu0");
		}
		//Manual
		if (btnType == ButtonType.Manual)
		{ 
		
		}
		//Config
		if (btnType == ButtonType.Config)
		{
			SceneGameScene0 sceneGameScene = GameObject.FindObjectOfType<SceneGameScene0>();
			//Прячем текущее меню и отображаем меню настроек
			sceneGameScene.goMenuFrame.transform.localScale = Vector3.zero;
			sceneGameScene.goConfigFrame.transform.localScale = Vector3.one;
		}
		//Upgrade
		if (btnType == ButtonType.Upgrade)
		{
            SceneGameScene0 sceneGameScene = GameObject.FindObjectOfType<SceneGameScene0>();
            //Прячем текущее меню и отображаем меню настроек
            sceneGameScene.goMenuFrame.transform.localScale = Vector3.zero;
            sceneGameScene.goUpgradeMenu.transform.localScale = Vector3.one;
        }
		//...
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
