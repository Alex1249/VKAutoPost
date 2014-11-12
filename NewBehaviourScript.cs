using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	
	string token = "Введите пожалуйста полученный access_token (необходимо обновлять раз в сутки)";
	string addresseeGroup = "Введите пожалуйста id группы";
	string[] addresseeGroupLoop = new string[1000];
	string message = "Введите пожалуйста текст сообщения";
	string[] loopMessage = new string[1000];
	string[] CloopMessage = new string[2000];
	string SloopMessage; 
	string respons = "";
	string resp = "";
	char delchar = '~';
	string media = "Введите пожалуйста список объектов, приложенных к записи и разделённых запятой или сотрите это поле и оставьте пустым";
	string[] mediaLoop = new string[1000];
	string[] CmediaLoop = new string[2000];
	string SmediaLoop;
	bool formGroup, formUser = true;
	int fGroup;
	int limitMessage;
	static string timer = "Введите пожалуйста интервал времени между сообщениями в секундах";
	float time;
	float TimerInt = 0;
	float timing;
	float aGroup;
	string STiming;
	string[] log = {"До следующей публикации осталось ","Публикация окончена "};
	int l = 0;
	bool start = false;
	bool stop = false;
	int i=0;
	bool SendOnePost = false;
	bool StartLoopPost = false;
	
	
	void Start () {
		for (int x = 0;x<loopMessage.Length;x++)
		{
			if (loopMessage[x] == null)
			{
				loopMessage[x] = message;
				mediaLoop[x] = media;
			}
			if (mediaLoop[x] == null)
			{
				mediaLoop[x] = media;
			}
			
		}
		Loade ();

		//PlayerPrefs.DeleteAll();
		//int z = 0;
		/*while (z<loopMessage.Length)
		{
			if (loopMessage == null)
			loopMessage[z] = message;
		}*/
		
	}
	void GetAccess () 
	{
		Application.OpenURL("https://oauth.vk.com/authorize?client_id=3994724&scope=offline,wall&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token");
	}
	IEnumerator PostMessage () 
	{
		string escMessege = WWW.EscapeURL(loopMessage[i]);
		string escMedia = WWW.EscapeURL(mediaLoop[i]);
		string SendMessage = "https://api.vk.com/method/wall.post?owner_id="+addresseeGroup+"&from_group="+fGroup+"&access_token="+token+"&message="+escMessege+"&attachments="+escMedia;
		
		WWW www = new WWW(SendMessage);
        yield return www;
		respons = www.text;
		char[] CharsRespons = respons.ToCharArray();
		if (CharsRespons[14] == 'p')
		{
			resp = "Публикация записи прошла успешно";
		}
		else
		{ 
			if (CharsRespons[14] == 'o')
			{
				resp = "Срок действия access_token истёк или он введён не верно. Необходимо получит новый.";
			}
			
		}
		start = true;
		Debug.Log("Post");
	}
	IEnumerator PostMessageOne () 
	{
		Debug.Log("Post");
		string escMessege = WWW.EscapeURL(message);
		string escMedia = WWW.EscapeURL(media);
		string SendMessage = "https://api.vk.com/method/wall.post?owner_id="+addresseeGroup+"&from_group="+fGroup+"&access_token="+token+"&message="+escMessege+"&attachments="+escMedia;
		WWW www = new WWW(SendMessage);
        yield return www;
		respons = www.text;
		char[] stringChars = respons.ToCharArray();
		if (stringChars[14] == 'p')
		{
			resp = "Публикация записи прошла успешно.";
		}
		else
		{ 
			if (stringChars[14] == 'o')
			{
				resp = "Срок действия access_token истёк или он введён не верно. Необходимо получит новый.";
			}
			
		}
		start = true;
	}
	void Save ()
	{
		PlayerPrefs.SetString("token", token);
		PlayerPrefs.SetString("addresseeGroup", addresseeGroup);
		
		int m = 0;
		int sm=0;
		
		while (m<loopMessage.Length)
		{
			//if ((m % 2)==0)
			CmediaLoop[m] = mediaLoop[sm];
			CloopMessage[m] = loopMessage[sm];
			m++;
			sm++;
			CmediaLoop[m] = "~";
			CloopMessage[m] = "~";
			m++;
		}
		
		SloopMessage = string.Concat(CloopMessage);
		PlayerPrefs.SetString("loopMessage", SloopMessage);
		SmediaLoop = string.Concat(CmediaLoop);
		PlayerPrefs.SetString("mediaLoop", SmediaLoop);
	}
	void Loade ()
	{
		if (PlayerPrefs.HasKey("token"))
		{
			token = PlayerPrefs.GetString("token");
			addresseeGroup = PlayerPrefs.GetString("addresseeGroup");
			SloopMessage = PlayerPrefs.GetString("loopMessage");
			loopMessage = SloopMessage.Split(delchar);
			SmediaLoop = PlayerPrefs.GetString("mediaLoop");
			mediaLoop = SmediaLoop.Split(delchar);
		}
		
	}
	void OnGUI ()
	{
		if (!SendOnePost && !StartLoopPost)
		{
			if (GUI.Button(new Rect(Screen.width/3-25, 100, 250, 40), "Отправить одно сообщение"))
			{
				StartLoopPost = false;
		    	SendOnePost =true;
			} 
			if (GUI.Button(new Rect(Screen.width/4, 140, 300, 40), "Запустить отправку отложенных записей"))
			{
		    	SendOnePost =false;
				StartLoopPost = true;
			}
		}
		if (SendOnePost || StartLoopPost)
		{
			if (GUI.Button(new Rect(10, 10, 100, 40), "Назад"))
			{
			    SendOnePost = false;
				StartLoopPost = false;
			}
		}
		if (SendOnePost)
		{
			token = GUI.TextArea(new Rect(10, 90, 500, 40), token, 200);
			addresseeGroup = GUI.TextArea(new Rect(10, 130, 500, 40), addresseeGroup, 200);
			message = GUI.TextArea(new Rect(10, 170, 500, 40), message, 200);
			media = GUI.TextArea(new Rect(10, 210, 500, 40), media, 200);
			resp = GUI.TextArea(new Rect(160, 290, 350, 50), resp, 200);
			if (GUI.Button(new Rect(10, 290, 150, 40), "Старт"))
			{
				stop = true;
		    	StartCoroutine(PostMessageOne ());
				
				
			} 
			if (GUI.Button(new Rect(10, 50, 250, 40), "Получить access_token"))
			{
		    	GetAccess ();
			}	
		}
		if (StartLoopPost)
		{
			if (!start)
			{
				if (GUI.Button(new Rect(200, 390, 150, 40), "Следующая запись"))
				{
						i++;
				}
				if ((GUI.Button(new Rect(10, 390, 150, 40), "предыдущая запись")) && (i > 0))
				{
					i--;
					Save ();
				}
				if ((GUI.Button(new Rect(10, 430, 150, 40), "Сохранить изменения")))
				{
					i--;
					Save ();
				}
			}
			GUI.TextArea(new Rect(300, 50, 150, 40), "Запись №"+(i+1), 200);
			if (stop)
			{
			GUI.TextArea(new Rect(10, 90, 500, 40), "Запись №"+(i+1)+" опубликована. "+log[l]+timing+STiming, 200);
			}
			token = GUI.TextArea(new Rect(10, 130, 500, 40), token, 200);
			addresseeGroup = GUI.TextArea(new Rect(10, 170, 500, 40), addresseeGroup, 200);
			loopMessage[i] = GUI.TextArea(new Rect(10, 210, 500, 40), loopMessage[i], 2000);	
			mediaLoop[i] = GUI.TextArea(new Rect(10, 250, 500, 40), mediaLoop[i], 200);
			resp = GUI.TextArea(new Rect(200, 430, 500, 40), resp, 200);
			timer = GUI.TextArea(new Rect(10, 290, 500, 40), timer, 200);
			if (aGroup<0)
			{
				if (GUI.Toggle(new Rect(10, 330, 200, 30), formUser, "От имени пользователя"))
				{
					formGroup = false;
					formUser = true;
				}
				if(GUI.Toggle(new Rect(250, 330, 200, 30), formGroup, "От имени группы"))
				{
					formUser = false;
					formGroup = true;
				}

				if (formUser == false)
				{
					fGroup= 1;
				}
				else
				{
					fGroup= 0;
				}
			}
			if (GUI.Button(new Rect(10, 350, 150, 40), "Старт"))
			{
				float TimerInt = System.Single.Parse(timer);
				time = TimerInt;
				stop = true;
				start = true;
		    	StartCoroutine(PostMessage ());
				Save ();
				
			} 
			if (GUI.Button(new Rect(10, 50, 250, 40), "Получить access_token"))
			{
		    	GetAccess ();
			} 
			if (start)
			{
				if (GUI.Button(new Rect(200, 350, 150, 40), "Остановить"))
				{
			    	start = false;
				} 
			}
			else
			{	
				if (stop)
				{
					if (GUI.Button(new Rect(200, 350, 150, 40), "Продолжить"))
					{
				    	start = true;
					}
				}
			}
		}
		
	}
	void Update () 
	{
			//Разбить на символы
		/*string s = PlayerPrefs.GetString(key);
        bool[] b = new bool[s.Length];
        for (int f = 0; f < b.Length; f++)
        {
            if (s[f].ToString() == "1")
                b[f] = true;
            else 
                b[f] = false;
        }*/
		//mediaLoop = SmediaLoop.Split(); 
		//Debug.Log(stringChars[14]);
		
		
		//Debug.Log(CharsMasseg[7]);
		aGroup = System.Single.Parse(addresseeGroup);
		Debug.Log(fGroup);
		Debug.Log(limitMessage);
		Debug.Log(time);
		if (time>60)
		{
			timing = Mathf.RoundToInt(time/60);
			STiming = " минут...";
		}
		else
		{
			timing = Mathf.RoundToInt(time);
			STiming = " секунд...";
		}
		if (start && !SendOnePost)
		{
			time -= Time.deltaTime;
			if (timing == 0f && time<60)
			{
				i++;
				float TimerInt = System.Single.Parse(timer);
				StartCoroutine(PostMessage());
				time = TimerInt;
			}
		}
	}
}
