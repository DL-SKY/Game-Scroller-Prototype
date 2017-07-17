/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт снаряжения =
*/

using UnityEngine;
using System.Collections;


public class Equipment : MonoBehaviour 
{
	public enum EquipmentType { Shield = 0, RocketRed = 1, SlowMo = 2, Health = 3, RocketBlue = 4, RocketGreen = 5,
                                Harpoon = 6 };
    [Header("Main")]
	public bool bEnabled = true;                //Флаг доступности в игре (может сгенерироваться в респауне)
	public EquipmentType type = 0;				//Тип снаряжения
    public float fReloading = 5;                //Время перезарядки
	public float fCooldown = 0;					//Время кулдауна
	public string strSprite = "";				//Имя спрайта

	[Header("Number of charges")]
	public int iCharges = 1;					//количество зарядов

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		fCooldown = 0.0f;
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
		//перезарядка
		if (fCooldown > 0)
		{
			fCooldown -= Time.deltaTime;
		}
	}
	//------------------------------------------------
	//Обработка столкновений с триггерами
	private void OnTriggerEnter2D(Collider2D _other)
	{
		//Если столкновение с Игроком - игнорируем (т.к. обработка у самого Игрока)
		if (_other.tag == "Player")
			return;

		//Если Блок
		if (_other.tag == "Block")
		{
			Destroy(_other.gameObject);
			Destroy(this.gameObject);
		}

		//Если Бокс, то игнорим
		if (_other.tag == "Box")
		{
			//Destroy(_other.gameObject);
			//Destroy(this.gameObject);
		}

		//Если такой же ящик снаряжения, то игнорим
		if (_other.tag == "Equipment")
		{
			//Destroy(_other.gameObject);
			//Destroy(this.gameObject);
		}

		//Если Метеор
		if (_other.tag == "Meteor")
		{
			Destroy(this.gameObject);
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

		//Эффект поломки
		if (transform.localScale.x > 0)
			Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effEquipment, transform.position, Quaternion.identity);
	}
    //------------------------------------------------
    //Активация снаряжения
    public void OnActivate()
    {
        //Проверка на кулдаун
		if (fCooldown <= 0.0f)
		{
			//Ставим на кулдаун
			fCooldown = fReloading;
			//Отнимаем заряды
			iCharges--;

			//Какое действие выполняем...
			//Здоровье
			if (type == EquipmentType.Health)
			{ 
				PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
				playerShip.iHPcur = playerShip.iHPmax;
			}
            //Щиты
            if (type == EquipmentType.Shield)
            {
                PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
                playerShip.iShields = playerShip.iShieldsMax;
            }
            //Время
            else if (type == EquipmentType.SlowMo)
            {
                SceneGameScene0 gameScene = GameObject.FindObjectOfType<SceneGameScene0>();
                gameScene.StartCoroutine(gameScene.TimerSlowMo());
            }
            //Ракеты Red
            else if (type == EquipmentType.RocketRed)
            {
                PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
                Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().goRocketRed, playerShip.transform.position, Quaternion.identity);
            }
            //Ракеты Blue
            else if (type == EquipmentType.RocketBlue)
            {
                PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
                Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().goRocketBlue, playerShip.transform.position, Quaternion.identity);
            }
            //Ракеты Green
            else if (type == EquipmentType.RocketGreen)
            {
                PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
                SceneGameScene0 scene = GameObject.FindObjectOfType<SceneGameScene0>();

                //public Vector3 v3DeltaPos = new Vector3();      //Дельта смещения при перемещении
                Instantiate(scene.goRocketGreen,
                            new Vector3(-scene.v3DeltaPos.x, playerShip.transform.position.y, playerShip.transform.position.z),
                            Quaternion.identity);
                Instantiate(scene.goRocketGreen,
                            new Vector3(0.0f, playerShip.transform.position.y, playerShip.transform.position.z),
                            Quaternion.identity);
                Instantiate(scene.goRocketGreen,
                            new Vector3(scene.v3DeltaPos.x, playerShip.transform.position.y, playerShip.transform.position.z),
                            Quaternion.identity);
            }
            //Гарпун
            else if (type == EquipmentType.Harpoon)
            {
                PlayerShip playerShip = GameObject.FindObjectOfType<PlayerShip>();
                Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().goHarpoon, playerShip.transform.position, Quaternion.identity);
            }
            //...

            //Если заряды кончились - удаляем объект
            if (iCharges <=0)
			Destroy(gameObject);
		}
    }
    //------------------------------------------------
   	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
