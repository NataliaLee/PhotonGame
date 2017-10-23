using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [SerializeField]
	private byte maxHealth = 5;
	[SerializeField]
	private float txtOffset = 1f;
	[SerializeField]
	private Text healthTxtPrefab;
	private Text healthTxt;
	private byte currentHealth;
	private Canvas canvas;

	public byte CurrentHealth{get{return currentHealth;}}

	void Start () {
		canvas = FindObjectOfType<Canvas> ();
		currentHealth=maxHealth;
		PhotonServer.Instance.SetCharacterHealth (gameObject.name,maxHealth);
		setHealthPanel ();
	}

	public void TryHit() {
		Debug.Log ("try hit "+gameObject.name);
		PhotonServer.Instance.RequestHit (gameObject.name);
    }

	public void UpdateHealth(byte newHealth){
		Debug.Log (gameObject.name+" update health "+newHealth);
		currentHealth = newHealth;
		healthTxt.text = ""+CurrentHealth;
	}

	private void setHealthPanel(){
		healthTxt = Instantiate (healthTxtPrefab ,new Vector3(),transform.rotation);
		healthTxt.transform.SetParent(canvas.transform, false);
		healthTxt.transform.SetAsFirstSibling ();
		healthTxt.text = ""+CurrentHealth;
		Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + txtOffset, transform.position.z);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		healthTxt.transform.SetPositionAndRotation(screenPos, transform.rotation);
	}
}
