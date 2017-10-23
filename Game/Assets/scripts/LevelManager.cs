using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	[SerializeField]
	private GameObject quitBtn, cantHitDialog;
    
	void Start () {        
		quitBtn.SetActive (false);
		cantHitDialog.SetActive (false);
		InstantiatePlayers ();
	}

	void InstantiatePlayers(){
		GameObject player = GameManager.Instance.GetChosenPlayerPrefab ();
		Debug.Log ("init player: "+player.name);
		Instantiate (player).name=player.name;
		GameObject enemy = GameManager.Instance.GetRandomEnemyPrefab ();
		Debug.Log ("init enemy: "+enemy.name);
		Instantiate (enemy,new Vector3(5,0,0),Quaternion.identity).name=enemy.name;
	}

	public void UpdateCharacterHealth(string characterName,byte characterHealth){
		GameObject character=GameObject.Find(characterName);
		if (character != null)
			character.GetComponent<Health> ().UpdateHealth (characterHealth);
	}

	public void CantHit(){
		//показать окно
		cantHitDialog.SetActive (true);
		GetComponent<TouchDetection> ().enabled = false;
	}

	public void ShowQuitBtn(){
		cantHitDialog.SetActive (false);
		quitBtn.SetActive (true);
		GetComponent<TouchDetection> ().enabled = true;
	}

	public void FinishGame(){
		//выяснить кто победил и обновить статы
		GameObject[] characters=GameObject.FindGameObjectsWithTag("Character");
		Health[] healths = new Health[characters.Length];
		for (int i = 0; i < characters.Length; i++) {
			healths [i] = characters [i].GetComponent<Health> ();
		}
		if (healths [0].CurrentHealth > healths [1].CurrentHealth) {
			Save (characters[0].name,characters[1].name);
		}

		if (healths [0].CurrentHealth < healths [1].CurrentHealth) {
			Save (characters[1].name,characters[0].name);
		}
		//если по ровну,то не считаем

		//выйти в лобби
		GameManager.Instance.LoadScene("lobby");
	}

	void Save(string winner,string loser){
		Debug.Log ("save win="+winner+" lose="+loser);
		if (PlayerPrefs.HasKey (winner)) {
			string characterStatsData = PlayerPrefs.GetString (winner);
			CharacterStats characterStats=JsonUtility.FromJson<CharacterStats> (characterStatsData);
			characterStats.winsCount++;
			PlayerPrefs.SetString (winner,JsonUtility.ToJson (characterStats));
		} else {
			CharacterStats characterStats = new CharacterStats ();
			characterStats.winsCount = 1;
			PlayerPrefs.SetString (winner,JsonUtility.ToJson (characterStats));
		}

		if (PlayerPrefs.HasKey (loser)) {
			string characterStatsData = PlayerPrefs.GetString (loser);
			CharacterStats characterStats=JsonUtility.FromJson<CharacterStats> (characterStatsData);
			characterStats.loseCount++;
			PlayerPrefs.SetString (loser,JsonUtility.ToJson(characterStats));
		} else {
			CharacterStats characterStats = new CharacterStats ();
			characterStats.loseCount = 1;
			PlayerPrefs.SetString (loser,JsonUtility.ToJson(characterStats));
		}

		PlayerPrefs.Save ();

	}
	
}
