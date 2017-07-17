/*
© Alexander Danilovsky, 2017
//------------------------------------------------
= Скрипт кнопок диалога выхода =
*/

using UnityEngine;
using System.Collections;


public class ExitDialogButton : MonoBehaviour
{
    public enum ButtonType { Ok = 0, Cancel = 1 }
    [Header("Main")]
    public ButtonType type = 0;

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
    //Обработчик нажатия на кнопку
    void OnClick()
    {
        //Ок
        if (type == ButtonType.Ok)
        {
            Application.Quit();
        }
        //Отмена
        if (type == ButtonType.Cancel)
        {
            GameObject.Find("AnchorMenuDialogExit").transform.localScale = Vector3.zero;
        }
    }
    //------------------------------------------------
}
