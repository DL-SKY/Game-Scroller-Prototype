/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт снаряда РАКЕТА =
*/

using UnityEngine;
using System.Collections;


public class Rocket : MonoBehaviour
{
    public enum RocketType { RocketRed = 0, RocketBlue = 1, RocketGreen = 2, Harpoon = 3 };
    [Header("Main")]
    public RocketType type = 0;

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
    //Обработка столкновений с триггерами
    private void OnTriggerEnter2D(Collider2D _other)
    {
        //Если столкновение с Игроком - игнорируем (т.к. обработка у самого Игрока)
        if (_other.tag == "Player")
            return;
        //Все остальные объекты
        else
        {
            //Начисляем очки СЧЕТА
            //GameObject.FindObjectOfType<PlayerShip>().iScore += 5;

            //Уничтожаем объект
            if (type == RocketType.Harpoon)
            {
                _other.GetComponent<MoveScript>().v3Speed += new Vector3(0.0f, 7.0f, 0.0f);
            }
            else 
                Destroy(_other.gameObject);

            //уничтожаем ракету
            if ( (type == RocketType.RocketRed) || (type == RocketType.RocketGreen) 
                || (type == RocketType.Harpoon) )
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
        //Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effAmmo, transform.position, Quaternion.identity);
    }
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
}
