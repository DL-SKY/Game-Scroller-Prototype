/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт игровых кнопок сцены =
*/

using UnityEngine;
using System.Collections;


public class GameButtons : MonoBehaviour 
{
	float fCooldown = 0.0f;
	UISprite sprite;
	//Перечисление типа кнопки
	public enum ButtonType { ToLeft = 0, ToRight = 1, ToAction1 = 2, ToAction2 = 3, ToAction3 = 4,
                             ToAction4 = 5, ToAction5 = 6, ToMenu = 7, ToResetEquipment = 8 };
	[Header("Main")]
	public Color32 clrGUI;							//Цвет GUI
	public ButtonType buttonType = 0;				//Тип кнопки
	public int iItem = 0;							//Индекс кнопки ToAction

	[Header("Links")]
	public PlayerShip playerShip;					//Ссылка на корабль Игрока
	public UILabel labelCooldown;					//Ссылка на метку кулдауна
	public UILabel labelCharges;					//Ссылка на метку зарядов

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		if (buttonType == ButtonType.ToAction1)
			iItem = 0;
		else if (buttonType == ButtonType.ToAction2)
			iItem = 1;
		else if (buttonType == ButtonType.ToAction3)
			iItem = 2;

		sprite = GetComponentInChildren<UISprite>();
		clrGUI = GameObject.FindObjectOfType<SceneGameScene0>().clrGUI;
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
		//для кнопок ToAction
		if (	(buttonType != ButtonType.ToAction1) &&
				(buttonType != ButtonType.ToAction2) &&
				(buttonType != ButtonType.ToAction3)	)
			return;

		if (iItem >= playerShip.equipments.Length)
		{
			transform.localScale = Vector3.zero;
			return;
		}

		//Отображаем или не отображаем кнопку
		if (playerShip.equipments[iItem] == null)
		{
			transform.localScale = Vector3.zero;
		}
		else //if (playerShip.equipments[iItem] != null)
		{
			transform.localScale = Vector3.one;

			//показываем правильный спрайт
			sprite.spriteName = playerShip.equipments[iItem].strSprite;

			//Отображаем кулдаун и заряды
			fCooldown = playerShip.equipments[iItem].fCooldown;
			fCooldown = (float)System.Math.Round(fCooldown, 1);

			if (fCooldown > 0.0f)
			{
				labelCooldown.text = fCooldown.ToString();
				sprite.color = new Color32(128, 128, 128, 128);	//Серый
			}
			else
			{
				labelCooldown.text = "";
				//sprite.color = new Color32(0, 255, 1, 128);
				sprite.color = new Color32(clrGUI.r, clrGUI.g, clrGUI.b, 128);
			}
			labelCharges.text = playerShip.equipments[iItem].iCharges.ToString();
		}

	}
	//------------------------------------------------
	//Обработчик нажатия на кнопку
	void OnClick()
	{
		if (iItem >= playerShip.equipments.Length)
			return;

		//Звук
		SoundManager.instance.PlayOnClick();

		//Налево
		if (buttonType == ButtonType.ToLeft)
		{
			playerShip.OnMoved(true);
		}
		//Направо
		if (buttonType == ButtonType.ToRight)
		{
			playerShip.OnMoved(false);
		}
		//Действие слота 1
		if (buttonType == ButtonType.ToAction1)
		{
			playerShip.OnAction(0);
		}
		//Действие слота 2
		if (buttonType == ButtonType.ToAction2)
		{
			playerShip.OnAction(1);
		}
		//Действие слота 3
		if (buttonType == ButtonType.ToAction3)
		{
			playerShip.OnAction(2);
		}
		//Меню
		if (buttonType == ButtonType.ToMenu)
		{
            GameObject goMenuFrame = GameObject.FindObjectOfType<SceneGameScene0>().goMenuFrame;
            //Остановка времени, открытие меню
            //if (Time.timeScale == 1)
            if (goMenuFrame.transform.localScale.x == 0)
			{
				Time.timeScale = 0.0f;
                goMenuFrame.transform.localScale = Vector3.one;
            }
			//Нормализация времени, закрытие меню
			else
			{
				Time.timeScale = 1.0f;
                goMenuFrame.transform.localScale = Vector3.zero;
            }
		}
		//Сброс снаряжения
		if (buttonType == ButtonType.ToResetEquipment)
		{
			for (int i = 0; i < playerShip.equipments.Length; i++)
			{
				if (playerShip.equipments[i] != null)
					Destroy(playerShip.equipments[i].gameObject);
			}
		}
		//...
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
