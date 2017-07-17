/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт сцены Благодарности =
*/

using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class SceneCredits : MonoBehaviour 
{
	[Header("Main")]
	public float fTimerSkip = 2.5f;					//Таймер появления возможности закрытия окна
	public bool bSkip = false;						//Флаг возможности пропустить/вернуться

	[Header("GUI")]
	public GameObject goButtonSkip;					//Ссылка на Анкор с фоном и кнопкой "Пропустить"

	//------------------------------------------------
	// Use this for initialization
	void Start () 
	{
		bSkip = false;

		goButtonSkip.transform.localScale = Vector3.zero;
		StartCoroutine(TimerToSkip());
	}
	//------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
	
		//Выход
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!bSkip)
				return;

			SceneManager.LoadScene("MainMenu0");
		}
	}
	//------------------------------------------------
	IEnumerator TimerToSkip()
	{
		yield return new WaitForSeconds(fTimerSkip);
		bSkip = true;
		goButtonSkip.transform.localScale = Vector3.one;
	}
	//------------------------------------------------
	//------------------------------------------------
	//------------------------------------------------
}
