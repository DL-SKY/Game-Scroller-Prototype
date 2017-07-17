/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт игрового корабля =
*/

using UnityEngine;
using System.Collections;


public class PlayerShip : MonoBehaviour
{
    Rigidbody2D rb2D;                               //Ссылка на объект
    Animator animator;                              //Ссылка на "аниматор"
    bool bMoveNow = false;                          //Флаг перемещения в настоящий момент

	[Header("Main")]
	public int iEquipmentsNumber = 1;				//Кол-во допустимых слотов снаряжения
    public int iHPmax = 0;                          //Максимальное кол-во ХП
    public int iHPcur = 0;                          //Текущее значение ХП
    public int iMoney = 0;                          //Деньги
	public int iScore = 0;							//Счет, результат

	[Header("Equipment")]
    public int iShieldsMax = 5;                     //макс. кол-во щитов
    public int iShields = 0;						//Щиты
    public Equipment[] equipments;                  //Снаряжение

    [Header("Navigation")]
    public float fMoveTime = 1.0f;                  //Время на перемещение
    public Vector3 v3DeltaPos = new Vector3();      //Дельта смещения при перемещении
    public Vector3 v3Position = new Vector3();      //Текущее положение

    [Header("Effects")]
    public float fSpeedFlame = 0.1f;                //Скорость частиц огня двигателя
    public ParticleSystem[] particleSystems;        //Ссылки на системы частиц огня двигателя     

    //------------------------------------------------
    // Use this for initialization
    void Start ()
    {
        //Инициализация параметров
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        iHPcur = iHPmax;
        v3Position = transform.position;
		iEquipmentsNumber = ConfigManager.instance.iEquipmentsNumber;
        equipments = new Equipment[iEquipmentsNumber];
		/*for (int i = 0; i < equipments.Length; i++)
		{
			equipments[i] = new Equipment();
		}*/
		//Актуализируем скорость частиц огня двигателя
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].gravityModifier = fSpeedFlame / 10.0f;
		}
    }
    //------------------------------------------------
    // Update is called once per frame
    void Update ()
    {
        //Актуализируем скорость частиц огня двигателя
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].gravityModifier = fSpeedFlame / 10.0f;
        }

        //Перемещение...
        //Влево
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnMoved(true);
        }
        //Вправо
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnMoved(false);
        }
		//Использовать умение/действие 1
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnAction(0);
		}
        //Использовать умение/действие 2
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnAction(1);
        }
        //Использовать умение/действие 3
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnAction(2);
        }
    }
    //------------------------------------------------
    //Перемещение
    public void OnMoved(bool _bLeft)
    {
        //Проверка на текущее перемещение
        if (bMoveNow)
            return;

        //Влево
        if (_bLeft)
        {
            //проверка
            if (transform.position.x >= 0.0f)
            {
                //Включаем анимацию и запускаем корутин
                animator.SetBool("ToLeft", true);
                StartCoroutine( SmoothMovement(transform.position - v3DeltaPos) );
            }
        }
        //Вправо
        if (!_bLeft)
        {
            //проверка
            if (transform.position.x <= 0.0f)
            {
                //Включаем анимацию и запускаем корутин
                animator.SetBool("ToRight", true);
                StartCoroutine( SmoothMovement(transform.position + v3DeltaPos) );
            }
        }
    }
    //------------------------------------------------
    //Подпрограмма плавного перемещения
    protected IEnumerator SmoothMovement(Vector3 _v3EndPos)
    {
        bMoveNow = true;
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - _v3EndPos).sqrMagnitude;
        float fTMP = 2.0f / fMoveTime;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, _v3EndPos, fTMP * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - _v3EndPos).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        //Снимаем флаг перемещения и отключаем анимацию
        bMoveNow = false;
        animator.SetBool("ToLeft", false);
        animator.SetBool("ToRight", false);
    }
    //------------------------------------------------
    //Обработка столкновений с триггерами
    private void OnTriggerEnter2D(Collider2D _other)
    {
        //Блок
        if (_other.tag == "Block")
        {
			OnDamage();
            Destroy(_other.gameObject);
        }
        //Ящик
        if (_other.tag == "Box")
        {
            //Определяем, что за ящик...
            //Здоровье
            if (_other.GetComponent<Box>().boxType == Box.BoxType.HP)
            {
                iHPcur++;
                if (iHPcur > iHPmax)
                    iHPcur = iHPmax;
            }
            //Деньги
            if (_other.GetComponent<Box>().boxType == Box.BoxType.Money)
            {
                iMoney += 5;
            }
            //Щиты
            if (_other.GetComponent<Box>().boxType == Box.BoxType.Shield)
            {
                iShields++;
                if (iShields > iShieldsMax)
                    iShields = iShieldsMax;
            }
            //Сброс снаряжения
            if (_other.GetComponent<Box>().boxType == Box.BoxType.Drop)
            {
                for (int i = 0; i < equipments.Length; i++)
                {
                    if (equipments[i] != null)
                        Destroy(equipments[i].gameObject);
                }
            }
            //Дополнительные заряды
            if (_other.GetComponent<Box>().boxType == Box.BoxType.Ammo)
            {
                for (int i = 0; i < equipments.Length; i++)
                {
                    if (equipments[i] != null)
                        equipments[i].iCharges++;
                }
            }

			//Начисляем очки СЧЕТА
			iScore += 5 + 1 * ConfigManager.instance.iDifficulty;

            //Уничтожаем ящик
            Destroy(_other.gameObject);
        }
		//Снаряжение
		if (_other.tag == "Equipment")
		{
			//Определяем возможность установки снаряжения
			for (int i = 0; i < equipments.Length; i++)
			{
				if (equipments[i] == null)
				{
					GameObject goToInstantiate;
					goToInstantiate = _other.gameObject as GameObject;
					//Инстанцируем объект
					GameObject goInstance = Instantiate(goToInstantiate, Vector3.zero, Quaternion.identity) as GameObject;
					//Устанавливаем как дочерний
					goInstance.transform.SetParent(transform);

					goInstance.transform.localPosition = Vector3.zero;
					Destroy(goInstance.GetComponent<BoxCollider2D>());
					Destroy(goInstance.GetComponent<Rigidbody2D>());
					Destroy(goInstance.GetComponent<MoveScript>());
					goInstance.transform.localScale = Vector3.zero;

					equipments[i] = goInstance.GetComponent<Equipment>();
					
					break;
				}
			}

			//Начисляем очки СЧЕТА
			iScore += 5 + 1 * ConfigManager.instance.iDifficulty;

			//Уничтожаем объект
			Destroy(_other.gameObject);
		}
        //Метеор
        if (_other.tag == "Meteor")
		{
			OnDamage(2);
			Destroy(_other.gameObject);
		}
    }
	//------------------------------------------------
	//Ф-ция получения урона
	void OnDamage(int _dmg = 1)
	{
		if (iShields > 0)
			iShields -= _dmg;
		else
			iHPcur -= _dmg;

		//Проверка на неотрицательное число
		if (iShields < 0)
			iShields = 0;

        //Вибрация
		StartCoroutine(UtilityBase.VibrateCoroutine(1, ConfigManager.instance.bVibrateEnable));

		//Проверка на "окончание игры"
		CheckOnGameOver();
	}
    //------------------------------------------------
	//Действие - использовать активное умение
	public void OnAction(int _index = 0)
	{
		if (equipments[_index] == null)
			return;

		equipments[_index].OnActivate();
	}
    //------------------------------------------------
	//Проверка на "окончание игры"
	void CheckOnGameOver()
	{ 
		//Игра не окончена
		if (iHPcur > 0)
			return;
		//Игра окончена
		else
		{
			//Останавливаем время
			Time.timeScale = 0.1f;

			//Штраф за уничтожение собственного корабля
			iMoney -= (iMoney / 10);

			//Сохраняем даные об Игровом СЧЕТЕ (для рейтинга)
			ConfigManager.instance.iScore = iScore;
			ConfigManager.instance.SaveScore();
			//...

			//Обнуляем игровой СЧЕТ
			iScore = 0;

			//Сохраняем данные о корабле, счете  и пр.
			iHPcur = iHPmax;
			GameObject.FindObjectOfType<SceneGameScene0>().SaveConfiguration();
			//...

			//Перезагружаем уровень
			GameObject.FindObjectOfType<SceneGameScene0>().RestartScene();
			//...
		}
	}
    //------------------------------------------------
	//Ф-ция обновления кол-ва слотов
	public void SlotsUpdate()
	{ 
		//Вспомогательная переменная
		Equipment[] eqptTMP = new Equipment[equipments.Length];
		eqptTMP = equipments;

		//Пересоздаем массив слотов с актуальным кол-вом элементов
		equipments = new Equipment[iEquipmentsNumber];
		for (int i = 0; i < eqptTMP.Length; i++)
		{
			equipments[i] = eqptTMP[i];
		}
	}
	//------------------------------------------------
}
