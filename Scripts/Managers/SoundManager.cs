/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Звуковой менеджер =
*/

using UnityEngine;
using System.Collections;


public class SoundManager : MonoBehaviour
{
    //Позволяет другие сценарии для вызова функций из SoundManagerBase.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.	

    [Header("Audio objects Base Class")]
    //Перетащите ссылку на источник звука, который будет воспроизводить звуковые эффекты.   
    public AudioSource audioEfxSource;                  //Drag a reference to the audio source which will play the sound effects.
    public AudioSource audioGUISource;                  //Drag a reference to the audio source which will play the sound effects.
                                                        // Перетащите ссылку на источник звука, который будет играть музыку.
    public AudioSource audioMusicSource;                //Drag a reference to the audio source which will play the music.

    //Массивы звуковых файлов
    [Header("Music Base Class")]
    public AudioClip[] aclipEfx;                        //Аудиоэффекты
    public AudioClip[] aclipGUI;                        //Аудиоэффекты интерфейса
    public AudioClip[] aclipMusic;                      //Аудио фон

    //------------------------------------------------
    //Awake: вызывается один раз, когда объект создается. По сути аналог обычной функции-конструктора
    protected void Awake()
	{
		//Проверяем статичную переменную instance
		CheckInstance();
	}
	//------------------------------------------------
	// Use this for initialization
	protected void Start() 
	{
        //Устанавливаем звук по умолчанию
        SetVolume();
    }
	//------------------------------------------------
	// Update is called once per frame
	protected void Update() 
	{
	
	}
    //------------------------------------------------
    //Функция проверки инстанцирования в статичную переменную
    public virtual void CheckInstance()
    {
        //Проверяем на существование экземпляра SoundManagerBase
        if (instance == null)
            //если нет, установим ЭТОТ экземпляр
            instance = this;
        //Если экземпляр уже существует и это не ЭТОТ:
        else if (instance != this)
            //То уничтожаем его. Это обеспечивает создание только одного экземпляра класса SoundManagerBase
            Destroy(gameObject);

        //Указываем, что SoundManagerBase не должно удаляться при повторной загрузке уровня/сцены
        DontDestroyOnLoad(gameObject);
    }
    //------------------------------------------------
    //Установка громкости по умолчанию
    public virtual void SetVolume()
    {
        //Настраиваем громкость по умолчанию

        //audioEfxSource.volume = ConfigManager.instance.fEfxVolume;
        //audioGUISource.volume = ConfigManager.instance.fEfxVolume;
        //audioMusicSource.volume = ConfigManager.instance.fMusicVolume;   

        audioEfxSource.volume = 0.8f;
        audioGUISource.volume = 0.6f;
        audioMusicSource.volume = 0.3f;
    }
    //------------------------------------------------
    //Используется для воспроизведения одиночных звуковых клипов.
    public virtual void PlaySingleEfx(AudioClip _clip)
    {
        //Установите клип нашего EFX исходного источника звука к клипу переданном в качестве параметра.
        audioEfxSource.clip = _clip;

        //Проиграть EFX звук
        audioEfxSource.Play();
    }
    //------------------------------------------------
    //Используется для воспроизведения одиночных звуковых клипов.
    public void PlaySingleGUI(AudioClip _clip)
    {
        //Установите клип нашего GUI исходного источника звука к клипу переданном в качестве параметра.
        audioGUISource.clip = _clip;

        //Проиграть GUI звук
        audioGUISource.Play();
    }
    //------------------------------------------------
    //Звук при нажатии на клавишу
    public void PlayOnClick()
	{
		if (aclipGUI.Length < 1)
			return;

		PlaySingleGUI(aclipGUI[0]);
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
