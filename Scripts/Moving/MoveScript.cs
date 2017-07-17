/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт движущегося объекта =
*/

using UnityEngine;
using System.Collections;


public class MoveScript : MonoBehaviour 
{
	//Rigidbody2D rb2D;									//Ссылка на объект

    [Header("Configuration")]
    public float fLifeTime = 20;                        //Время жизни
	public bool bIsDeleting = true;						//Флаг удаления после ухода из поля зрения камеры
	public bool bIsStop = false;						//Флаг паузы в перемещении, стоянка

	[Header("Speed and Direction")]
	public Vector3 v3Speed = new Vector3();				//Скорость движения
	public Vector3 v3Direction = new Vector3();			//Направление движения	

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		//rb2D = GetComponent<Rigidbody2D>();

        //Удаляем. Таймер удаления
        if (bIsDeleting)
        {
            GameObject goTMP = gameObject as GameObject;
            Destroy(goTMP, fLifeTime);
        }
    }
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{

		//перемещение
		ToMove();
	}
	//------------------------------------------------
	//FixedUpdate: вызывается каждый раз через определеннок число кадров. Вы можете вызывать этот метод вместо Update() когда имеете дело с физикой ("RigidBody" и др.)
	void FixedUpdate()
	{
		//перемещение
		//ToMove();
	}
	//------------------------------------------------
	//Перемещение
	void ToMove()
	{
        //Проверка на стоянку
        if (bIsStop)
            return;

        //Вектор смещения
        Vector3 v3ToMove = new Vector3(v3Speed.x * v3Direction.x,
                                        v3Speed.y * v3Direction.y,
                                        v3Speed.z * v3Direction.z);
        v3ToMove *= Time.deltaTime;
        
		//Перемещаемся
        transform.Translate(v3ToMove);     
		//rb2D.velocity (v3ToMove);
		//MovePosition(v3ToMove);
    }
	//------------------------------------------------
	//------------------------------------------------
}
