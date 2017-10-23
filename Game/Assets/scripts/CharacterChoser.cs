using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoser : MonoBehaviour {
	[SerializeField]
	private Image characterImage;
	[SerializeField]
	private Text winsCountLabel;
	[SerializeField]
	private Text loseCountLabel;

	private CharacterData[] characters;

	private int currentCharacterNumber = 0;

	void Start(){
		characters = GameManager.Instance.Characters;
		setCharacter (characters[0]);
	}

	public void nextCharacter(){
		if (currentCharacterNumber + 1 == characters.Length)
			currentCharacterNumber = 0;
		else
			currentCharacterNumber++;
		setCharacter (characters[currentCharacterNumber]);
	}

	public void previoustCharacter(){
		if (currentCharacterNumber - 1 <0)
			currentCharacterNumber = characters.Length-1;
		else
			currentCharacterNumber--;
		setCharacter (characters[currentCharacterNumber]);
	}

	void setCharacter(CharacterData characterData){
		if (characterData == null)
			return;
		characterImage.sprite = characterData.Pic;
		characterImage.preserveAspect = true;
		updateStats (characterData.CharacterName);
		GameManager.Instance.chosenPlayer = currentCharacterNumber;
	}

	void updateStats(string characterName){
		if (PlayerPrefs.HasKey (characterName)) {
			string characterStatsData = PlayerPrefs.GetString (characterName);
			CharacterStats characterStats=JsonUtility.FromJson<CharacterStats> (characterStatsData);
			winsCountLabel.text = characterStats.winsCount.ToString();
			loseCountLabel.text = characterStats.loseCount.ToString();
		} else {
			winsCountLabel.text = "0";
			loseCountLabel.text = "0";
		}
	}

	public void UpdateStats(){
		updateStats(characters[currentCharacterNumber].CharacterName);
	}
}
