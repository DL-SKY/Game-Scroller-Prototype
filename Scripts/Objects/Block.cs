﻿/*
Spaces & Ships
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт космического блока =
*/

using UnityEngine;
using System.Collections;


public class Block : MonoBehaviour
{

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

        //Если такой же Блок
        if (_other.tag == "Block")
        {
            Destroy(_other.gameObject);
            Destroy(this.gameObject);
        }

        //Если Бокс
        if (_other.tag == "Block")
        {
            Destroy(_other.gameObject);
            Destroy(this.gameObject);
        }

		//Если ящик снаряжения
		if (_other.tag == "Equipment")
		{
			Destroy(_other.gameObject);
			Destroy(this.gameObject);
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

        //Эффект взрыва
        Instantiate(GameObject.FindObjectOfType<SceneGameScene0>().effExplosion, transform.position, Quaternion.identity);
    }
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
    //------------------------------------------------
}
