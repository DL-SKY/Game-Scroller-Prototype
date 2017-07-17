/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт Главного меню =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class SceneMainMenu0 : MonoBehaviour
{
    int iCountActivate = 0;                         //Костыль для устранения ошибки NGUI

    [Header("GUI")]
    public GameObject goMenuExitFrame;              //Ссылка на окно Выхода
    public GameObject goConfigFrame;                //Ссылка на окно Настроек
    public GameObject goUpgradeMenu;                //Ссылка на окно Прокачки

    [Header("Config")]
    public UISlider sliderMusic;                    //Ссылка на слайдер
    public UILabel labelPercentMusic;               //Ссылка на UILabel
    public UISlider sliderSound;                    //Ссылка на слайдер
    public UILabel labelPercentSound;               //Ссылка на UILabel
    public UICheckbox checkVibro;                   //Ссылка на чекбокс

    //------------------------------------------------
    void Awake()
    {
        //Актуализируем состояние элементов окна настроек
        //ConfigManager.instance.ApplyConfiguration();
        sliderMusic.sliderValue = ConfigManager.instance.fMusicVolume;
        sliderSound.sliderValue = ConfigManager.instance.fEfxVolume;
        checkVibro.isChecked = ConfigManager.instance.bVibrateEnable;
    }
    //------------------------------------------------
    // Use this for initialization
    void Start ()
    {
        //Прячем меню
        goMenuExitFrame.transform.localScale = Vector3.zero;
        goConfigFrame.transform.localScale = Vector3.zero;
		goUpgradeMenu.transform.localScale = Vector3.zero;
    }
    //------------------------------------------------
    // Update is called once per frame
    void Update ()
    {
        //Выход - вызов диалога выхода
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();


			//Если открыто меню прокачки
			if (goUpgradeMenu.transform.localScale.x == 1)
			{
				//закрываем
				goUpgradeMenu.transform.localScale = Vector3.zero;
			}
            //Открытие меню Выхода
            else if (goMenuExitFrame.transform.localScale.x == 0)
            {
                //Time.timeScale = 0.0f;
                if (goConfigFrame.transform.localScale.x == 0)
                {
                    goMenuExitFrame.transform.localScale = Vector3.one;
                    goConfigFrame.transform.localScale = Vector3.zero;
                }
                else
                {
                    goMenuExitFrame.transform.localScale = Vector3.zero;
                    goConfigFrame.transform.localScale = Vector3.zero;
                }               
            }
            //Закрытие меню
            else
            {
                //Time.timeScale = 1.0f;
                goMenuExitFrame.transform.localScale = Vector3.zero;
                goConfigFrame.transform.localScale = Vector3.zero;
            }
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
    //Действие на нажатие чекбокса вибрации
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
            StartCoroutine( UtilityBase.VibrateCoroutine(1, ConfigManager.instance.bVibrateEnable) );
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
	}
	//------------------------------------------------
}
