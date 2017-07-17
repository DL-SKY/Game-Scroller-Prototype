/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт скроллинга/перемещения (н-р фона) =
 * крепится на пустой объект (папку), в которой ...
 * ... дочерние элементы - куски фона для перемещения по циклу.
*/

using UnityEngine;
using System.Collections;

using System.Collections.Generic;						//Для использования List


public class Scrolling : MonoBehaviour 
{
	[Header("Configuration")]
	public bool bIsLooping = true;						//Флаг циклического перемещения
	public Vector3 v3DeltaPos = new Vector3();			//Дельта/Изменение в позиции разных кусков фона

	[Header("List parts BG")]
	public List<Transform> listChild = new List<Transform>();//Список кусков фона

	[Header("Speed and Direction")]
	public Vector3 v3Speed = new Vector3();				//Скорость прокрутки/движения
	public Vector3 v3Direction = new Vector3();			//Направление прокрутки/движения	

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		//проверка на пустой список
		if (listChild.Count <= 0)
			return;

		//размещаем куски фона по порядку с дельтой позиции
		for (int i = 0; i < listChild.Count; i++)
		{
			listChild[i].position += i * v3DeltaPos;
		}
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
		//перемещаем куски фона
		ToMoveChildrenBG();
	}
	//------------------------------------------------
	//Перемещение кусков фона, дочерних объектов
	void ToMoveChildrenBG()
	{
		//проверка на пустой список
		if (listChild.Count <= 0)
			return;

		//Вектор смещения
		Vector3 v3ToMove = new Vector3( v3Speed.x * v3Direction.x,
										v3Speed.y * v3Direction.y,
										v3Speed.z * v3Direction.z );
		v3ToMove *= Time.deltaTime;		

		//перемещаем все куски фона
		for (int i = 0; i < listChild.Count; i++)
		{
			listChild[i].Translate(v3ToMove);
		}

		//Если флаг цикла включен...
		if (bIsLooping)
		{
			//...и если 1ый кусочек НЕ виден в главной камере...
			if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main),
				 listChild[0].GetComponentInChildren<Renderer>().bounds))
			{
				//...Переносим первый кусочек в конец списка и меняем расположение
				Transform transform = listChild[0];

				listChild.Remove(transform);
				listChild.Add(transform);

				listChild[listChild.Count - 1].position = listChild[listChild.Count - 2].position + v3DeltaPos;
			}
		}
		//Если флаг цикла отключен...
		else
		{
			//...и если 1ый кусочек НЕ виден в главной камере...
			if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main),
				 listChild[0].GetComponentInChildren<Renderer>().bounds))
			{
				//...удаляем
				GameObject goTMP = listChild[0].gameObject as GameObject;
				listChild.Remove(listChild[0]);
				Destroy(goTMP);
			}
		}
	}
	//------------------------------------------------
}
