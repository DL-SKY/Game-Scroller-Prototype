/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт космического ящика/бокса =
*/

using UnityEngine;
using System.Collections;


public class Box : MonoBehaviour
{
    public enum BoxType { HP = 0, Money = 1, Shield = 2, Drop = 3, Ammo = 4 };
    [Header("Type")]
    public BoxType boxType = 0;                 //Тип содержимого ящика

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

        //Если Блок
        if (_other.tag == "Block")
        {
            Destroy(_other.gameObject);
            Destroy(this.gameObject);
        }

        //Если такой же Бокс, то игнорим
        if (_other.tag == "Box")
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
        if (boxType == BoxType.HP)
            Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effHP, transform.position, Quaternion.identity);
        if (boxType == BoxType.Money)
            Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effMoney, transform.position, Quaternion.identity);
        if (boxType == BoxType.Shield)
            Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effShield, transform.position, Quaternion.identity);
        if (boxType == BoxType.Drop)
            Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effDrop, transform.position, Quaternion.identity);
        if (boxType == BoxType.Ammo)
            Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effAmmo, transform.position, Quaternion.identity);
    }
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
}
