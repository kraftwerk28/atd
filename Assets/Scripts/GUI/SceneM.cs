using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour {

	public Text p;

	private AsyncOperation _as = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Go(int i)
	{
		StartCoroutine(LoadS(i));
	}

	private IEnumerator LoadS(int _index)
	{
		_as = SceneManager.LoadSceneAsync(_index);
		yield return _as;
	}

	private void OnGUI()
	{
		if (_as != null)
			p.text = (_as.progress * 100).ToString() + "%";
	}

	public void AppExit()
	{
		Application.Quit();
	}
}
