using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	
	public GameObject PauseCanv;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Time.timeScale == 1)
			{
				PauseCanv.GetComponent<Animator>().SetBool("show", true);
				Time.timeScale = 0;
				AudioListener.pause = true;
			}
			else
			{
				PauseCanv.GetComponent<Animator>().SetBool("show", false);
				Time.timeScale = 1;
				AudioListener.pause = false;
			}
		}
	}

	public void Continue()
	{
		PauseCanv.GetComponent<Animator>().SetBool("show", false);
		Time.timeScale = 1;
	}
	public void ExittoMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadSceneAsync(0);
	}
}
