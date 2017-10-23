using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CharacterData",menuName="Characters/Data")]
public class CharacterData : ScriptableObject {
	[SerializeField]
	[Tooltip("Имя для персонажа")]private string characterName;
	public string CharacterName{get{ return characterName;}}
	[SerializeField]
	[Tooltip("Картинка для меню выбора персонажа")]private Sprite pic;
	public Sprite Pic{get{ return pic;}}
	[SerializeField]
	[Tooltip("Префаб для персонажа в игре")]private GameObject pref;
	public GameObject CharacterPref{get{ return pref;}}

}
