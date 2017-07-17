/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт меню прокачки =
*/

using UnityEngine;
using System.Collections;


public class ItemToUpgrade : MonoBehaviour 
{
	public enum UpgradeType
	{
		HPmax = 0, SPmax = 1, SlowMo = 2, HP = 3, SP = 4, 
		RocketRed = 5, RocketBlue = 6, RocketGreen = 7, Harpoon = 8, Slots = 9
	};
	[Header("Main")]
	public UpgradeType upType = UpgradeType.HPmax;				//Тип кнопки
	public Transform transHint;									//Ссылка на окно подсказки
	
	[Header("Level")]
	public int iLevel = 0;										//Уровень этого параметра/оснащения
	public int iLevelMax = 5;									//Макс. уровень этого параметра/оснащения

	[Header("Cost")]
	public int[] iCosts = {100, 250, 500, 1000, 2500};			//Массив цен
	public int iCost = 0;										//Цена улучшения
	public Color clrActive;										//Активный цвет
	public Color clrDisable;									//Неактивный цвет

	[Header("Transforms Lvl")]
	public Transform[] transLevel;								//Ссылка на трансформы точек - индикаторов уровня
	public UILabel labelCost;									//Ссылка на лейбл стоимости


	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		transHint.localScale = Vector3.zero;

		//Проверка прпавильного уровня над остальными объектами
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -5);
		transHint.localPosition = new Vector3(transHint.localPosition.x, transHint.localPosition.y, -5);

		Show();
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
		//Проверка
		if (iLevel < 0)
			iLevel = 0;
		if (iLevel > iLevelMax)
			iLevel = iLevelMax;

		//Отображаем зеленые или серые кубики
		for (int i = 0; i < transLevel.Length; i++)
		{
			if (i < iLevel)
				transLevel[i].localScale = Vector3.one;
			else
				transLevel[i].localScale = Vector3.zero;
		}

		//Стоимость
		int iMoney = ConfigManager.instance.iMoney;
		if (iLevel < iCosts.Length)
		{
			iCost = iCosts[iLevel];
			labelCost.text = iCost.ToString() + " (" + iMoney.ToString() + ")";
			if (iMoney/10000 >= 1)
				labelCost.text = iCost.ToString() + " (" + (iMoney/1000).ToString() + "K)";

			//цвет ярлыка
			if (iCost <= iMoney)
				labelCost.color = clrActive;
			else
				labelCost.color = clrDisable;
		}
		else
		{
			iCost = 0;
			labelCost.text = "";
		}
		
	}
	//------------------------------------------------
	//Обработчик нажатия на кнопку
	void OnClick()
	{
		//Показываем окно подсказки/описания
		transHint.localScale = Vector3.one;
	}
	//------------------------------------------------
	//Применить изменения
	public void ToApply()
	{
		if (upType == UpgradeType.HPmax)
		{
			ConfigManager.instance.iHPmax = iLevel;
		}
		if (upType == UpgradeType.SPmax)
		{
			ConfigManager.instance.iShieldsMax = iLevel;
		}
		if (upType == UpgradeType.SlowMo)
		{
			ConfigManager.instance.iEquipmentsCharges[0] = iLevel;
		}
		if (upType == UpgradeType.HP)
		{
			ConfigManager.instance.iEquipmentsCharges[1] = iLevel;
		}
		if (upType == UpgradeType.SP)
		{
			ConfigManager.instance.iEquipmentsCharges[2] = iLevel;
		}
		if (upType == UpgradeType.RocketRed)
		{
			ConfigManager.instance.iEquipmentsCharges[3] = iLevel;
		}
		if (upType == UpgradeType.RocketBlue)
		{
			ConfigManager.instance.iEquipmentsCharges[4] = iLevel;
		}
		if (upType == UpgradeType.RocketGreen)
		{
			ConfigManager.instance.iEquipmentsCharges[5] = iLevel;
		}
		if (upType == UpgradeType.Harpoon)
		{
			ConfigManager.instance.iEquipmentsCharges[6] = iLevel;
		}
		if (upType == UpgradeType.Slots)
		{
			ConfigManager.instance.iEquipmentsNumber = iLevel;
		}

	}
	//------------------------------------------------
	//Прорисовываем элементы
	void Show()
	{
		if (upType == UpgradeType.HPmax)
		{
			iLevel = ConfigManager.instance.iHPmax;
		}
		if (upType == UpgradeType.SPmax)
		{
			iLevel = ConfigManager.instance.iShieldsMax;
		}
		if (upType == UpgradeType.SlowMo)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[0];
		}
		if (upType == UpgradeType.HP)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[1];
		}
		if (upType == UpgradeType.SP)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[2];
		}
		if (upType == UpgradeType.RocketRed)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[3];
		}
		if (upType == UpgradeType.RocketBlue)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[4];
		}
		if (upType == UpgradeType.RocketGreen)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[5];
		}
		if (upType == UpgradeType.Harpoon)
		{
			iLevel = ConfigManager.instance.iEquipmentsCharges[6];
		}
		if (upType == UpgradeType.Slots)
		{
			iLevel = ConfigManager.instance.iEquipmentsNumber;
		}
	}
	//------------------------------------------------
	//Попытка улучшения
	public void ToUpgrade()
	{
		//подготовка: сохранение
		SceneGameScene0 sceneGame = GameObject.FindObjectOfType<SceneGameScene0>();
		if (sceneGame)
			sceneGame.SaveConfiguration();

		//Если не хватает денег - ничего не делаем
		if (iCost > ConfigManager.instance.iMoney)
			return;

		//Списываем деньги и улучшаем параметр
		ConfigManager.instance.iMoney -= iCost;
		iLevel++;
		ToApply();

		if (sceneGame)
			sceneGame.ApplyConfiguration();
	}
	//------------------------------------------------
	//------------------------------------------------
}
