/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Менеджер конфигурации =
*/

using UnityEngine;
using System.Collections;

using System.IO;


public class ConfigManager : MonoBehaviour
{
    //Позволяет другие сценарии для вызова функций из ConfigManager.
    public static ConfigManager instance = null;     //Allows other scripts to call functions from ConfigManager.	

    //Перечисление типов сборки
    public enum TypeBuild
    {
        Test = 0, Demo = 1, preAlpha = 2, Alpha = 3,
        preBeta = 4, Beta = 5, Release = 6
    };
    [Header("General settings")]
    //Тип сборки и версия
    public TypeBuild build;                         //Тип сборки из Перечисления TypeBuild
	public string strVersion = "v. 0.0.1";
    //Громкость звука
    public float fEfxVolume = 0.35f;                //Громкость эффектов (+ интерфейса)
    public float fMusicVolume = 0.15f;              //Громкость музыки
    //Вибрация
    public bool bVibrateEnable = true;			    //Флаг использования вибрации
	//Индекс используемой локализации 
	public int iLocalization = 0;				    //(0 - Russian, 1 - English,)	

    [Header("Game")]
    public Vector3 v3Speed = new Vector3();         //Скорость движения (фона и окружения)
    public int iDifficulty = 0;                     //Сложность
    public float fTimerToGeneration = 1.0f;         //Таймер появления объекто
	public int iRecordScore = 0;					//Рекордный счет

    [Header("Player")]
    public int iEquipmentsNumber = 1;				//Кол-во допустимых слотов снаряжения
    public int iHPmax = 0;                          //Максимальное кол-во ХП
    public int iHPcur = 0;                          //Текущее значение ХП
    public int iMoney = 0;                          //Деньги
    public int iScore = 0;                          //Счет, результат
    public int iShieldsMax = 5;                     //макс. кол-во щитов
    public int iShields = 0;						//Щиты

	[Header("Equipments")]
	public bool[] bEquipmentsEnabled;				//Признак Enabled элементов массива префабов "Экипировки"
	public int[] iEquipmentsCharges;				//кол-во зарядов элементов массива префабов "Экипировки"


    //------------------------------------------------
    //Awake: вызывается один раз, когда объект создается. По сути аналог обычной функции-конструктора
    protected void Awake()
	{
		//Проверяем статичную переменную instance
		CheckInstance();
	}
	//------------------------------------------------
	// Use this for initialization
	protected void Start () 
	{
		//Инициализация Флотов
		//fleetPlayer = new Fleet();
		//fleetEnemy = new Fleet();


		//Загружаем настройки
		LoadConfig();
	}
	//------------------------------------------------
	// Update is called once per frame
	protected void Update () 
	{
	
	}
	//------------------------------------------------
	//FixedUpdate: вызывается каждый раз через определеннок число кадров. Вы можете вызывать этот метод вместо Update() когда имеете дело с физикой ("RigidBody" и др.)
	void FixedUpdate()
	{
		//Проверка локализации
		if (Localization.instance.currentLanguage == "Russian")
			iLocalization = 0;
		else if (Localization.instance.currentLanguage == "English")
			iLocalization = 1;
	}
    //------------------------------------------------
    //Функция проверки инстанцирования в статичную переменную
    public virtual void CheckInstance()
    {
        //Проверяем на существование экземпляра ConfigManager
        if (instance == null)
            //если нет, установим ЭТОТ экземпляр
            instance = this;
        //Если экземпляр уже существует и это не ЭТОТ:
        else if (instance != this)
            //То уничтожаем его. Это обеспечивает создание только одного экземпляра класса ConfigManager
            Destroy(gameObject);

        //Указываем, что ConfigManagerBase не должно удаляться при повторной загрузке уровня/сцены
        DontDestroyOnLoad(gameObject);
    }
    //------------------------------------------------
	//Сохранение игрового счета
	public void SaveScore()
	{
		if (iRecordScore < iScore)
			iRecordScore = iScore;
	}
	//------------------------------------------------
    //Сохранение настроек
    public void SaveConfig()
	{ 
        //Если компилируется НЕ для ВЭБ-плеера
#if !UNITY_WEBPLAYER

		//-------------------------------------
        //Конфигурация
        /* Config.con
        0 - Локализация
        1 - Громкость музыки
        2 - Громкость эффектов/звуков
        3 - Вибрация
		4 - Версия
		5 - рекорд счет
        */
        string[] strSaveConfig = new string[6];
		strSaveConfig[0] = iLocalization.ToString();
        strSaveConfig[1] = fMusicVolume.ToString();
        strSaveConfig[2] = fEfxVolume.ToString();
        strSaveConfig[3] = bVibrateEnable.ToString();
		strSaveConfig[4] = strVersion;
		strSaveConfig[5] = iRecordScore.ToString();

        File.WriteAllLines(Application.persistentDataPath + "/Config.con", strSaveConfig);
		//-------------------------------------
		//Сохранение
		/* GameSave.sav
		//Сцена
		0-2 - v3Speed = ConfigManager.instance.v3Speed;									//Скорость
		3 - iDifficulty = ConfigManager.instance.iDifficulty;							//Сложность
		4 - fTimerToGeneration = ConfigManager.instance.fTimerToGeneration;				//Таймер появления объектов
		 * 
		//Корабль игрока
		5 - playerShip.iEquipmentsNumber = ConfigManager.instance.iEquipmentsNumber;	//Кол-во допустимых слотов снаряжения
		6 - playerShip.iHPmax = ConfigManager.instance.iHPmax;							//Максимальное кол-во ХП
		7 - playerShip.iHPcur = ConfigManager.instance.iHPcur;							//Текущее значение ХП
		8 - playerShip.iMoney = ConfigManager.instance.iMoney;							//Деньги
		9 - playerShip.iScore = ConfigManager.instance.iScore;							//Счет, результат
		10 - playerShip.iShieldsMax = ConfigManager.instance.iShieldsMax;               //макс. кол-во щитов
		11 - playerShip.iShields = ConfigManager.instance.iShields;						//Щиты
		 * 
		//Экипировка/оснащение
		for (int i = 0; i < goEquipments.Length; i++)								//Оснащение
		{
			goEquipments[i].GetComponent<Equipment>().bEnabled = ConfigManager.instance.bEquipmentsEnabled[i];
			goEquipments[i].GetComponent<Equipment>().iCharges = ConfigManager.instance.iEquipmentsCharges[i];
		}
        */
		string[] strSaveGame = new string[12 + bEquipmentsEnabled.Length*2];
		strSaveGame[0] = v3Speed.x.ToString();
		strSaveGame[1] = v3Speed.y.ToString();
		strSaveGame[2] = v3Speed.z.ToString();
		strSaveGame[3] = iDifficulty.ToString();
		strSaveGame[4] = fTimerToGeneration.ToString();
		strSaveGame[5] = iEquipmentsNumber.ToString();
		strSaveGame[6] = iHPmax.ToString();
		strSaveGame[7] = iHPcur.ToString();
		strSaveGame[8] = iMoney.ToString();
		strSaveGame[9] = iScore.ToString();
		strSaveGame[10] = iShieldsMax.ToString();
		strSaveGame[11] = iShields.ToString();
		for (int i = 0; i < bEquipmentsEnabled.Length; i++)
		{
			strSaveGame[2 * i + 12] = bEquipmentsEnabled[i].ToString();
			//strSaveGame[2 * i + 14] = iEquipmentsCharges[i].ToString();
			strSaveGame[2 * i + 13] = iEquipmentsCharges[i].ToString();
		}

		File.WriteAllLines(Application.persistentDataPath + "/GameSave.sav", strSaveGame);
    
#endif
	}
	//------------------------------------------------
	//Загрузка настроен
	public void LoadConfig()
	{
		//Если компилируется НЕ для ВЭБ-плеера
#if !UNITY_WEBPLAYER

		//-------------------------------------
        //Конфигурация
        if (File.Exists(Application.persistentDataPath + "/Config.con"))
        {
            string[] strLoadConfig = new string[6];                     
			int iFileLength = File.ReadAllLines(Application.persistentDataPath + "/Config.con").Length;
			
			//Проверка загружаемых данных
			if (strLoadConfig.Length == iFileLength)
			{
				strLoadConfig = File.ReadAllLines(Application.persistentDataPath + "/Config.con");

				iLocalization = int.Parse(strLoadConfig[0]);
				fMusicVolume = float.Parse(strLoadConfig[1]);
				fEfxVolume = float.Parse(strLoadConfig[2]);
				bVibrateEnable = bool.Parse(strLoadConfig[3]);
				strVersion = strLoadConfig[4];
				iRecordScore = int.Parse(strLoadConfig[5]);
			}
        }
		//"По умолчанию"
		else
		{
			iLocalization = 0;
			fMusicVolume = 0.15f;
			fEfxVolume = 0.35f;
			bVibrateEnable = true;
			strVersion = "v. 0.0.1";
			iRecordScore = 0;
        }
        //-------------------------------------
        //Сохранение
        if (File.Exists(Application.persistentDataPath + "/GameSave.sav"))
		{
			string[] strLoadGame = new string[12 + bEquipmentsEnabled.Length*2];
			int iFileLength = File.ReadAllLines(Application.persistentDataPath + "/GameSave.sav").Length;
			//Проверка загружаемых данных
			if (strLoadGame.Length == iFileLength)
			{ 
				strLoadGame = File.ReadAllLines(Application.persistentDataPath + "/GameSave.sav");		

				v3Speed = new Vector3(	float.Parse(strLoadGame[0]),
										float.Parse(strLoadGame[1]),
										float.Parse(strLoadGame[2])  );
				iDifficulty = int.Parse(strLoadGame[3]);
				fTimerToGeneration = float.Parse(strLoadGame[4]);
				iEquipmentsNumber = int.Parse(strLoadGame[5]);
				iHPmax = int.Parse(strLoadGame[6]);
				iHPcur = int.Parse(strLoadGame[7]);
				iMoney = int.Parse(strLoadGame[8]);
				iScore = int.Parse(strLoadGame[9]);
				iShieldsMax = int.Parse(strLoadGame[10]);
				iShields = int.Parse(strLoadGame[11]);
				for (int i = 0; i < bEquipmentsEnabled.Length; i++)
				{
					 bEquipmentsEnabled[i] = bool.Parse(strLoadGame[2 * i + 12]);
					 //iEquipmentsCharges[i] = int.Parse(strLoadGame[2 * i + 14]);
					iEquipmentsCharges[i] = int.Parse(strLoadGame[2 * i + 13]);
				}
			}
		}
		//"По умолчанию"
		else
		{
			v3Speed = new Vector3(0.0f, 1.5f, 0.0f );
			iDifficulty = 0;
			fTimerToGeneration = 1.5f;
			iEquipmentsNumber = 1;
			iHPmax = 2;
			iHPcur = 2;
			iMoney = 0;
			iScore = 0;
			iShieldsMax = 1;
			iShields = 0;
		/*
			bEquipmentsEnabled[0] = true;
			iEquipmentsCharges[0] = 1;

			bEquipmentsEnabled[1] = true;
			iEquipmentsCharges[1] = 1;

			bEquipmentsEnabled[2] = true;
			iEquipmentsCharges[2] = 1;

			bEquipmentsEnabled[3] = true;
			iEquipmentsCharges[3] = 1;

			bEquipmentsEnabled[4] = true;
			iEquipmentsCharges[4] = 1;

			bEquipmentsEnabled[5] = true;
			iEquipmentsCharges[5] = 1;

			bEquipmentsEnabled[6] = true;
			iEquipmentsCharges[6] = 1;
		*/
		}
      
#endif

		//Применяем настройки
		ApplyConfiguration();	
	}
	//------------------------------------------------
	//Применение настроек конфигурации
	public void ApplyConfiguration()
	{ 
		//"General settings"
		//Локализация
		ApplyLocalization();
		//Настраиваем громкость
		if (SoundManager.instance.audioEfxSource != null)
			SoundManager.instance.audioEfxSource.volume = fEfxVolume;
		if (SoundManager.instance.audioGUISource != null)
			SoundManager.instance.audioGUISource.volume = fEfxVolume;
		if (SoundManager.instance.audioMusicSource != null)
			SoundManager.instance.audioMusicSource.volume = fMusicVolume;

		//сохраняем
		//SaveConfig();
	}
	//------------------------------------------------
	//Применение настроек локализации
	void ApplyLocalization()
	{
		if (GameObject.FindObjectOfType<Localization>() == null)
			return;

		if (Localization.instance != null)
		{
			//Если индекс локализации больше, чем кол-во файлов локализаций в префабе Локализация,
			//то используем первый файл (Русский)
			if (iLocalization >= Localization.instance.languages.Length)
				iLocalization = 0;

			Localization.instance.currentLanguage = Localization.instance.languages[iLocalization].name;
		}
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//Данная функция вызывается при выходе из игры
	void OnApplicationQuit()
	{
		SaveConfig();
		//Debug.Log("...Exit game...");
	}
}
