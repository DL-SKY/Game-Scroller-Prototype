/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт Игровой сцены =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using Random = UnityEngine.Random; 		            //Tells Random to use the Unity Engine random number generator.


public class SceneGameScene0 : MonoBehaviour 
{
    int iCountActivate = 0;                         //Костыль для устранения ошибки NGUI

    [Header("Main")]
    public Vector3 v3DeltaPos = new Vector3();      //Дельта смещения при перемещении
    public Vector3 v3Speed = new Vector3();         //Скорость движения (фона и окружения)    

	[Header("Generation")]
	public int iDifficulty = 0;						//Сложность (0-1-2)
	public float fTimerToGeneration = 1.0f;         //Таймер появления объекто
	public int iPercentToGenOneObj = 50;			//Процент появления 1го объекта
	public int iPercentToGenTwoObjs = 40;			//Процент появления 2ух объектов
	public int iPercentToBlock = 40;				//Процент появления Блока
    public int iPercentToBox = 15;                  //Процент появления Ящика
	public int iPercentToEquipment = 15;            //Процент появления Снаряжения
    public int iPercentToMeteor = 10;				//Процент появления Метеора

    [Header("Links")]
    public GameObject goPlayer;                     //Ссылка на корабль игрока    
	PlayerShip playerShip;							//Ссылка на скрипт корабля Игрока
    public Transform[] transRespawn;                //Ссылки на точки появления объектов

    [Header("BackGrounds")]
    public GameObject goBackGround0;                //Ссылка на "фон0 - объекты"
    public GameObject goBackGround10;               //Ссылка на "фон10"

	[Header("GUI")]
	public Color32 clrGUI;							//Цвет интерфейса
	public UILabel labelMoney;						//Ссылка на ярлык кол-ва денег
	public UILabel labelScore;						//Ссылка на ярлык счета/результата
	public UISprite[] spritesHP;					//Ссылка на спрайты кол-ва здоровья
    public UISprite[] spritesSP;					//Ссылка на спрайты кол-ва щитов
    public GameObject goMenuFrame;                  //Ссылка на окно Меню
	public GameObject goConfigFrame;                //Ссылка на окно Настроек
	public GameObject goGameOver;					//Ссылка на окно окончания игры (проигрыша)
    public GameObject goUpgradeMenu;                //Ссылка на окно апгрейда

    [Header("Config")]
    public UISlider sliderMusic;                    //Ссылка на слайдер
    public UILabel labelPercentMusic;               //Ссылка на UILabel
    public UISlider sliderSound;                    //Ссылка на слайдер
    public UILabel labelPercentSound;               //Ссылка на UILabel
    public UICheckbox checkVibro;                   //Ссылка на чекбокс

    [Header("Prefabs")]
    public GameObject[] goBlocks;                   //Префабы блоков
    public GameObject[] goBoxes;                    //Префабы ящиков
	public GameObject[] goEquipments;               //Префабы снаряжения
    public GameObject[] goMeteors;					//Префабы метеоров

    [Header("Charges")]
    public GameObject goRocketRed;                  //Префаб ракеты (красная, летит со средней скоростью)
    public GameObject goRocketBlue;                 //Префаб ракеты
    public GameObject goRocketGreen;                //Префаб ракеты
    public GameObject goHarpoon;                    //Префаб гарпуна

    [Header("Effects")]
    public GameObject effExplosion;                 //Эффект взрыва
    public GameObject effMeteor;                    //Эффект взрыва метеора
    public GameObject effHP;                        //Эффект разлома ящика с HP
    public GameObject effMoney;                     //Эффект разлома ящика с деньгами
    public GameObject effShield;                    //Эффект разлома ящика с щитами
    public GameObject effDrop;                      //Эффект разлома ящика с дропом
    public GameObject effAmmo;                      //Эффект разлома ящика с боеприпасами
    public GameObject effEquipment;					//Эффект разлома ящика со снаряжением

    //------------------------------------------------
    void Awake()
    {
        //Актуализируем состояние элементов окна настроек
        //ConfigManager.instance.ApplyConfiguration();
        //sliderMusic.sliderValue = ConfigManager.instance.fMusicVolume;
        //sliderSound.sliderValue = ConfigManager.instance.fEfxVolume;
        //checkVibro.isChecked = ConfigManager.instance.bVibrateEnable;
    }
    //------------------------------------------------
    // Use this for initialization
    void Start () 
	{
        //Актуализируем данные о скорости и смещении в др. глассах
        goPlayer.GetComponent<PlayerShip>().v3DeltaPos = v3DeltaPos;
        goPlayer.GetComponent<PlayerShip>().fSpeedFlame = v3Speed.y;
		playerShip = goPlayer.GetComponent<PlayerShip>();
        goBackGround10.GetComponent<Scrolling>().v3Speed = v3Speed;

        //Прячем Меню
        goMenuFrame.transform.localScale = Vector3.zero;
		goConfigFrame.transform.localScale = Vector3.zero;
		goGameOver.transform.localScale = Vector3.zero;
        goUpgradeMenu.transform.localScale = Vector3.zero;

		//Применение настроек
		ApplyConfiguration();

        //Запуск подпрограммы-таймера
        StartCoroutine(TimerGeneration());
	}
	//------------------------------------------------
	// Update is called once per frame
	public void Update () 
	{
        //Актуализируем данные о скорости в др. классах
        goPlayer.GetComponent<PlayerShip>().fSpeedFlame = v3Speed.y;
        goBackGround10.GetComponent<Scrolling>().v3Speed = v3Speed;

		//Интерфейс...
		//Деньги
		labelMoney.text = playerShip.iMoney.ToString();
		ConfigManager.instance.iMoney = playerShip.iMoney;
		//Счет
		labelScore.text = playerShip.iScore.ToString();
		//Здоровье максимальное
		for (int i = 0; i < spritesHP.Length; i++)
		{
			if (i < playerShip.iHPmax)
				spritesHP[i].enabled = true;
			else
				spritesHP[i].enabled = false;
		}
		//Здоровье текущее
		for (int i = 0; i < spritesHP.Length; i++)
		{
			if (i < playerShip.iHPcur)
			{
				spritesHP[i].color = new Color32(clrGUI.r, clrGUI.g, clrGUI.b, 200);
			}
			else
				spritesHP[i].color = new Color32(128, 128, 128, 150);		//Серый
		}
        //Щиты
        for (int i = 0; i < spritesSP.Length; i++)
        {
            if (i < playerShip.iShields)
                spritesSP[i].enabled = true;
            else
                spritesSP[i].enabled = false;
        }


        //Нажатие клавиш
        //Меню
        if (Input.GetKeyDown(KeyCode.Escape))
        {

			//Если открыто меню прокачки
			if (goUpgradeMenu.transform.localScale.x == 1)
			{
				//закрываем
				goUpgradeMenu.transform.localScale = Vector3.zero;
				Time.timeScale = 1.0f;
			}
			//Остановка времени, открытие меню
			else if (goMenuFrame.transform.localScale.x == 0)
			{
				Time.timeScale = 0.0f;
				//сохранение
				SaveConfiguration();
				goMenuFrame.transform.localScale = Vector3.one;
				goConfigFrame.transform.localScale = Vector3.zero;
			}
			//Нормализация времени, закрытие меню
			else
			{
				Time.timeScale = 1.0f;
				goMenuFrame.transform.localScale = Vector3.zero;
				goConfigFrame.transform.localScale = Vector3.zero;
			}
        }


    }
    //------------------------------------------------
    //Ф-ция генерации объектов
    void OnGenerationSpawn()
    {
        //Вспомогательные переменные
        GameObject goToInstantiate;
		goToInstantiate = goBlocks[Random.Range(0, goBlocks.Length)];
        int iPercent = Random.Range(1, 100);
        int iCount = 0;
        int iPos = Random.Range(0, 3);

        //кол-во объектов
		if (iPercent <= iPercentToGenOneObj)
            iCount = 1;
		else if (iPercent <= iPercentToGenOneObj + iPercentToGenTwoObjs)
            iCount = 2;
        else
            iCount = 3;

		//проверяем кол-во объектов с уровнем сложности
		switch (iDifficulty)
		{ 
			case 0:
				if (iCount >= 3)	iCount = 1;
				break;
			case 1:
				if (iCount >= 3)	iCount = 2;
				break;
			case 2:
				if (iCount <= 1) iCount = 2;
				break;
		}

        for (int i = 0; i < iCount; i++)
        {
            //Проверяем переменную позиционирования
			//(инкремент ниже - чтобы исключить повторения точки появления за одну генерацию)
            if (iPos > 2)
                iPos = 0;

            //Определяем, какой именно объект генерируем. И настраиваем...
			iPercent = Random.Range(1, 100);
			//...Блок
			if (iPercent <= iPercentToBlock)
			{
				goToInstantiate = goBlocks[Random.Range(0, goBlocks.Length)];
				goToInstantiate.GetComponent<MoveScript>().v3Speed = Random.Range(0.5f, 2.5f + 1.0f * iDifficulty/*5.0f*/) * v3Speed;
				goToInstantiate.GetComponent<MoveScript>().v3Direction = new Vector3(0.0f, -1.0f, 0.0f);
			}
            //...Ящик
            else if (iPercent <= iPercentToBlock + iPercentToBox)
            {
                goToInstantiate = goBoxes[Random.Range(0, goBoxes.Length)];
				goToInstantiate.GetComponent<MoveScript>().v3Speed = Random.Range(0.5f, 2.5f + 1.0f * iDifficulty/*5.0f*/) * v3Speed;
                goToInstantiate.GetComponent<MoveScript>().v3Direction = new Vector3(0.0f, -1.0f, 0.0f);
            }
			//...Снаряжение
			else if (iPercent <= iPercentToBlock + iPercentToBox + iPercentToEquipment)
			{
				//Проверка на доступность рандомного снаряжения
				int iIndex = Random.Range(0, goEquipments.Length);
				for (int i2 = 0; i2 < goEquipments.Length; i2++)
				{
					if (goEquipments[iIndex + i2].GetComponent<Equipment>().bEnabled)
					{
						iIndex += i2;
						break;
					}						

					if ((iIndex + i2) >= goEquipments.Length)
						iIndex *= -1;
				}

				goToInstantiate = goEquipments[iIndex];
				goToInstantiate.GetComponent<MoveScript>().v3Speed = Random.Range(0.5f, 2.5f + 1.0f * iDifficulty/*5.0f*/) * v3Speed;
				goToInstantiate.GetComponent<MoveScript>().v3Direction = new Vector3(0.0f, -1.0f, 0.0f);
				//...код ниже нужно переместить в ф-цию примененеи настроек при загрузке уровня
				//Указываем кол-во зарядов (зависит от "прокачки")
				//goToInstantiate.GetComponent<Equipment>().iCharges = ConfigManager.instance.iEquipmentsCharges[iIndex];
			}
            //...Метеор
			else if (iPercent <= iPercentToBlock + iPercentToBox + iPercentToEquipment + iPercentToMeteor)
			{
				goToInstantiate = goMeteors[Random.Range(0, goMeteors.Length)];
				goToInstantiate.GetComponent<MoveScript>().v3Speed = Random.Range(1.5f, 4.0f + 1.5f * iDifficulty/*5.0f*/) * v3Speed;
				goToInstantiate.GetComponent<MoveScript>().v3Direction = new Vector3(0.0f, -1.0f, 0.0f);
			}

				
				

            //Создаем объект
            OnSpawn(iPos, goToInstantiate);

            //Переменная позиции (инкремент - чтобы исключить повторения за одну генерацию)
            iPos++;

			//Начисляем очки СЧЕТА
			playerShip.iScore++;
            playerShip.iScore += 1 * ConfigManager.instance.iDifficulty;
			//playerShip.iScore += 9999;
        }        
    }
    //------------------------------------------------
    //Создание объектов
    void OnSpawn(int _spawnPoint, GameObject _toInstantiate)
    {
        //Вспомогатедльные переменные
        Vector3 v3Position = new Vector3();

        //Выбираем точку появления
        switch (_spawnPoint)
        {
            case 0: //левая      
                v3Position = transRespawn[0].position;          
                break;
            case 1: //центральная
                v3Position = transRespawn[1].position;
                break;
            case 2: //правая
                v3Position = transRespawn[2].position;
                break;
        }

        //Инстанцируем объект
        GameObject goInstance = Instantiate( _toInstantiate, v3Position, Quaternion.identity ) as GameObject;
        //Устанавливаем как дочерний
        goInstance.transform.SetParent(goBackGround0.transform);
    }
    //------------------------------------------------
    //Подпрограмма таймера генерации объектов
    protected IEnumerator TimerGeneration()
    {
        //...в бесконечном цикле
        while (true)
        {
			UpdateSpeedAndDifficulty();
            OnGenerationSpawn();
            yield return new WaitForSeconds(fTimerToGeneration);
        }
    }
	//------------------------------------------------
	//Ф-ция обновления скорости и сложности
	public void UpdateSpeedAndDifficulty()
	{ 
		/* 
		 * по умолчанию:
		 * iDifficulty = 0;
		 * v3Speed = new Vector3(0.0f, 1.5f, 0.0f);
		*/
		v3Speed = new Vector3(0.0f, 1.5f + (float)playerShip.iScore/1000.0f, 0.0f);
		int iDif = playerShip.iScore % 300;
		//iDifficulty = 
	}
    //------------------------------------------------
	//Подпрограмма слоу-мо
	public IEnumerator TimerSlowMo()
	{
		Time.timeScale = 0.1f;
		yield return new WaitForSeconds(0.1f);
		Time.timeScale = 1.0f;
	}
    //------------------------------------------------
	//Функция перезапуска сцены
	public void RestartScene()
	{
		StartCoroutine(TimerRestart());
	}
    //------------------------------------------------
	//Подпрограмма перезапуска сценны
	IEnumerator TimerRestart()
	{
		//показываем окно окончания игры
		goGameOver.transform.localScale = Vector3.one;

		//Пауза
		yield return new WaitForSeconds(0.5f);
		Time.timeScale = 1.0f;
		//Перегружаем сцену
		SceneManager.LoadScene("GameScene0");
	}
    //------------------------------------------------
	//Ф-ция применения настроек
	public void ApplyConfiguration()
	{
		//Сцена
		v3Speed = ConfigManager.instance.v3Speed;  
		iDifficulty = ConfigManager.instance.iDifficulty;
		fTimerToGeneration = ConfigManager.instance.fTimerToGeneration;

		//Экипировка/оснащение
		for (int i = 0; i < goEquipments.Length; i++)
		{
			goEquipments[i].GetComponent<Equipment>().bEnabled = ConfigManager.instance.bEquipmentsEnabled[i];
			goEquipments[i].GetComponent<Equipment>().iCharges = ConfigManager.instance.iEquipmentsCharges[i];
			
			if (!goEquipments[i].GetComponent<Equipment>().bEnabled)
				goEquipments[i].GetComponent<Equipment>().iCharges = 0;
		}

		//Корабль игрока
		playerShip.iEquipmentsNumber = ConfigManager.instance.iEquipmentsNumber;	//Кол-во допустимых слотов снаряжения
		playerShip.iHPmax = ConfigManager.instance.iHPmax;							//Максимальное кол-во ХП
		playerShip.iHPcur = ConfigManager.instance.iHPcur;							//Текущее значение ХП
		playerShip.iMoney = ConfigManager.instance.iMoney;							//Деньги
		playerShip.iScore = ConfigManager.instance.iScore;							//Счет, результат
		playerShip.iShieldsMax = ConfigManager.instance.iShieldsMax;                //макс. кол-во щитов
		playerShip.iShields = ConfigManager.instance.iShields;						//Щиты

		playerShip.SlotsUpdate();
	}
    //------------------------------------------------
	//Ф-ция сохранения настроек
	public void SaveConfiguration()
	{
		//Сцена
		ConfigManager.instance.v3Speed = v3Speed;
		ConfigManager.instance.iDifficulty = iDifficulty;
		ConfigManager.instance.fTimerToGeneration = fTimerToGeneration;

		//Экипировка/оснащение
		for (int i = 0; i < goEquipments.Length; i++)
		{
			ConfigManager.instance.bEquipmentsEnabled[i] = goEquipments[i].GetComponent<Equipment>().bEnabled;
			ConfigManager.instance.iEquipmentsCharges[i] = goEquipments[i].GetComponent<Equipment>().iCharges;
		}

		//Корабль игрока
		ConfigManager.instance.iEquipmentsNumber = playerShip.iEquipmentsNumber;	//Кол-во допустимых слотов снаряжения
		ConfigManager.instance.iHPmax = playerShip.iHPmax;							//Максимальное кол-во ХП
		ConfigManager.instance.iHPcur = playerShip.iHPcur;							//Текущее значение ХП
		ConfigManager.instance.iMoney = playerShip.iMoney;							//Деньги
		ConfigManager.instance.iScore = playerShip.iScore;							//Счет, результат
		ConfigManager.instance.iShieldsMax = playerShip.iShieldsMax;                //макс. кол-во щитов
		ConfigManager.instance.iShields = playerShip.iShields;						//Щиты
	}
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------










    //------------------------------------------------
    //Действие на изменение громкости Музыки
    public void OnSliderMusicChange()
    {
        ConfigManager.instance.fMusicVolume = sliderMusic.sliderValue;
        labelPercentMusic.text = ((int)(ConfigManager.instance.fMusicVolume * 100.0f)).ToString() + "%";

        ConfigManager.instance.ApplyConfiguration();
    }
    //------------------------------------------------
    //Действие на изменение громкости Звуков
    public void OnSliderSoundChange()
    {
        ConfigManager.instance.fEfxVolume = sliderSound.sliderValue;
        labelPercentSound.text = ((int)(ConfigManager.instance.fEfxVolume * 100.0f)).ToString() + "%";

        ConfigManager.instance.ApplyConfiguration();
    }
    //------------------------------------------------
    //Действие на нажатие чекбокса Вибрации
    public void OnActivateVibroCheck()
    {
        iCountActivate++;   //Костыль для устранения ошибки NGUI

        if (iCountActivate > 1)
        {
            ConfigManager.instance.bVibrateEnable = checkVibro.isChecked;
            //Звук при нажатии клавиши
            SoundManager.instance.PlayOnClick();
            //Вибрация
            //ConfigManager.instance.Vibrate(1);
            StartCoroutine(UtilityBase.VibrateCoroutine(1, ConfigManager.instance.bVibrateEnable));
        }
    }
    //------------------------------------------------
	//При удаление объекта
	void OnDestroy()
	{
		//Проверка во избежание ошибки
		//Some objects were not cleaned up when closing the scene. (Did you spawn new GameObjects from OnDestroy?)
		if (enabled)
			return;

		//SaveConfiguration();
	}
	//------------------------------------------------
}
