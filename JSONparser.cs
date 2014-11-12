using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class JSONparser : MonoBehaviour {

	string respons = "{respons: 1}"; 		// пример ответа от сервера 
	string pattern1 = "respons";					// шаблон который будем искать в ответе
	string pattern2 = "error";					// шаблон который будем искать в ответе
	MatchCollection matches1;
	MatchCollection matches2;

	void Start () 
	{
		Regex newReg1 = new Regex(pattern1); 
		Regex newReg2 = new Regex(pattern2);

		matches1 = newReg1.Matches(respons);
		matches2 = newReg2.Matches(respons);
	}

	void OnGUI ()
	{
	
		foreach(Match mat in matches1)
		{
			string test = mat.Value;
			if (test == "respons")
			{
				GUI.TextArea(new Rect (10,10,100,50), "Выполненно успешно");
			}

		}
		foreach(Match mat in matches2)
		{
			string test = mat.Value;
			if (test == "error")
			{
				GUI.TextArea(new Rect (10,10,100,50), "Ошибка");
			}	
		}
	}
}
