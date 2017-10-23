using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour {

	[SerializeField]
	private CharacterData[] characters = new CharacterData[3];

	public CharacterData[] Characters{ get{ return characters;}}

    public int chosenPlayer { get; set; }

	private static GameManager instance;
	public static GameManager Instance {
		get {
			return instance;
		}
	}

	void Awake() {
		if (Instance != null)
			DestroyObject (gameObject);
		else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
	}

	public GameObject GetChosenPlayerPrefab(){
		GameObject player = Characters [chosenPlayer].CharacterPref;
		player.name = Characters [chosenPlayer].CharacterName;
		return player;
	}

	public GameObject GetRandomEnemyPrefab(){
		int randomNum = Random.Range (0, Characters.Length);
		while(randomNum==chosenPlayer)
			randomNum = Random.Range (0, Characters.Length);
		GameObject enemy = Characters [randomNum].CharacterPref;
		enemy.name = Characters [randomNum].CharacterName;
		return enemy;
	}

	public LevelManager GetLevelManager(){
		GameObject lmObject = GameObject.Find ("LevelManager");
		if (lmObject == null)
			return null;
		else
			return lmObject.GetComponent<LevelManager> ();
	}

	public void UpdateStats(){
		PlayerPrefs.DeleteAll ();
		CharacterChoser cc = FindObjectOfType<CharacterChoser> ();
		if (cc != null)
			cc.UpdateStats ();
	}
    
	public void LoadScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}
}
