using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonServer : MonoBehaviour, IPhotonPeerListener {

	enum EvCode : byte
	{
		UpdateStats
	}

	enum OpCode : byte
	{
		SetCharacterHealth,
		RequestHit
	}

	enum OpParam : byte
	{
		CharacterName,
		CharacterHealth,
		CanHit
	}

	private const string CONNECTION = "127.0.0.1:5055";
    private const string APP_NAME = "SimplePhotonServer";

    private static PhotonServer instance;
    public static PhotonServer Instance {
        get {
            return instance;
        }
    }

    public PhotonPeer PhotonPeer { get; set; }

    #region  MonoBehaviour methods

    void Awake() {
		if (Instance != null)
			DestroyObject (gameObject);
		else {
			DontDestroyOnLoad (gameObject);
			Application.runInBackground = true;
			instance = this;
		}
    }

    // Use this for initialization
    void Start()
    {
        PhotonPeer = new PhotonPeer(this,ConnectionProtocol.Udp);
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonPeer != null)
            PhotonPeer.Service();
    }

    void OnApplicationQuit() {
        Disconnect();
    }

    #endregion

    void Connect() {
        if (PhotonPeer!=null)
            PhotonPeer.Connect(CONNECTION,APP_NAME);
    }

    void Disconnect() {
        if (PhotonPeer != null)
            PhotonPeer.Disconnect();
    }

	public bool IsConnected(){
		if (PhotonPeer == null)
			return false;
		return PhotonPeer.PeerState.Equals (PeerStateValue.Connected);
	}


	#region receive messages

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn "+message);
    }

    public void OnEvent(EventData eventData)
    {
		//приходят с сервера без спроса
		// обнуление статов
        Debug.Log("OnEvent " + eventData);
		switch(eventData.Code){
		case (byte)EvCode.UpdateStats:
			GameManager.Instance.UpdateStats ();
			break;
		}
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log("OnOperationResponse " + operationResponse);
		// получение разрешения на удар (обновленное кол-во жизней у персонажа)
		switch(operationResponse.OperationCode){
		case (byte)OpCode.RequestHit:
			if (operationResponse.ReturnCode != 0)
				return;
			LevelManager levelManager = GameManager.Instance.GetLevelManager ();
			if (levelManager == null)
				return;
			if (operationResponse.Parameters.ContainsKey ((byte)OpParam.CanHit)) {
				//Если нельзя бить - показываем сообщение и заканчиваем игру
				if (!(bool)operationResponse [(byte)OpParam.CanHit]) {
					levelManager.CantHit ();
				} else {
					//если можно бить - обновляем хп
					string characterName = (string)operationResponse.Parameters [(byte)OpParam.CharacterName];
					byte newHealth = (byte)operationResponse.Parameters [(byte)OpParam.CharacterHealth];
					if (levelManager != null)
						levelManager.UpdateCharacterHealth (characterName, newHealth);
				}
			}			
				
			break;
		}

    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log("OnStatusChanged " + statusCode);
    }

	#endregion

	#region send requests

	public void SetCharacterHealth(string name,byte health){
		if (PhotonPeer == null)
			return;
		Debug.Log ("SetCharacterHealth "+name+" "+health);
		PhotonPeer.OpCustom ((byte)OpCode.SetCharacterHealth, 
			new Dictionary<byte, object>{ { (byte)OpParam.CharacterName,name } ,
				{(byte)OpParam.CharacterHealth,health} }, true);
	}

	public void RequestHit(string characterName){
		if (PhotonPeer == null) {
			return;
		}
		Debug.Log ("RequestHit "+characterName);
		PhotonPeer.OpCustom ((byte)OpCode.RequestHit,
			new Dictionary<byte, object>{ { (byte)OpParam.CharacterName,characterName } }, true);
	}
	#endregion

}
