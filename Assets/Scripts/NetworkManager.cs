using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] GameObject candies;
	[SerializeField] GameObject lobbyPanel;
	[SerializeField] GameObject gamePlayPanel;
	[SerializeField] InputField playerName;
	[SerializeField] Text[] playerNameOnScoreBoard;
	PlayerIndicatorUpdater playerIndicatorUpdater;

	public GameController gameController;
	public GamePlayPanel gamePlayPanelScript;

	int playerPosition;
	public bool[] playersInRoom = new bool[4];
	RoomInfo[] roomsList;
	GameObject player;
	

	// Use this for initialization
	void Start () {
		
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.1");
		Debug.Log ("Is master client: " + PhotonNetwork.isMasterClient);
		playerIndicatorUpdater = lobbyPanel.GetComponentInChildren<PlayerIndicatorUpdater>();

		
	}
	
	// Update is called once per frame
	void Update () {
		connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}
	
	void OnJoinedLobby()
	{
		gameController.setUpLobby();
	}

	public void JoinRoom()
	{
		//called from GUI pushbutton
		PhotonNetwork.player.name = playerName.text;
		PhotonNetwork.JoinRandomRoom ();

	}

	void OnPhotonRandomJoinFailed(){
		CreateRoom ();
	}

	void CreateRoom(){
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.maxPlayers = 4;

		roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable ();
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player1ID,0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player2ID,0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player3ID,0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player4ID,0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player1Score, 0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player2Score, 0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player3Score, 0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.Player4Score, 0);
		roomOptions.customRoomProperties.Add (RoomPropertyType.EndTime, 0);

		PhotonNetwork.CreateRoom (null, roomOptions, TypedLobby.Default);
	}

	void OnJoinedRoom()
	{
		playerPosition = DeterminePlayerPosition ();
		gameController.setUpRoom (playerPosition);
	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}
	
	public void putPlayerNameOnScoreBoard(string name, int playerIndex){
		PhotonView photonView = PhotonView.Get(this);
		photonView.RPC ("PutPlayerNameOnScoreBoard_RPC", PhotonTargets.AllBufferedViaServer, PhotonNetwork.player.name, playerIndex);
	}

	[PunRPC]
	void PutPlayerNameOnScoreBoard_RPC(string name, int playerIndex){
		if (name != null) {
			playerNameOnScoreBoard [playerIndex].text = name;
		}
	}

	public int DeterminePlayerPosition(){
		int playerIndex = 0;

		while ((int)PhotonNetwork.room.customProperties[RoomPropertyType.PlayerIDs[playerIndex]] != 0) {
			playerIndex++;
		}

		ExitGames.Client.Photon.Hashtable roomProp = new ExitGames.Client.Photon.Hashtable ();
		roomProp.Add (RoomPropertyType.PlayerIDs [playerIndex], PhotonNetwork.player.ID);
		PhotonNetwork.room.SetCustomProperties (roomProp);

		ExitGames.Client.Photon.Hashtable playerIndexHash = new ExitGames.Client.Photon.Hashtable ();
		playerIndexHash.Add (PlayerProperties.PlayerIndex, playerIndex);
		PhotonNetwork.player.SetCustomProperties(playerIndexHash);

		updateNumPlayersInRoom (playerIndex, true);
		return playerIndex + 1;
	}

	public void OnLeftRoom(){
		Debug.LogError ("This Player " + PhotonNetwork.player.customProperties [PlayerProperties.PlayerIndex] + " left the room");
		//ExitGames.Client.Photon.Hashtable resetProperty = new ExitGames.Client.Photon.Hashtable ();
		//resetProperty.Add (RoomPropertyType.PlayerIDs [(int)PhotonNetwork.player.customProperties [PlayerProperties.PlayerIndex]], 0);
		//PhotonNetwork.room.SetCustomProperties (resetProperty);
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer){
		Debug.LogError ("Player " + otherPlayer.customProperties [PlayerProperties.PlayerIndex] + " left the room");
		if (PhotonNetwork.isMasterClient) {
			ExitGames.Client.Photon.Hashtable resetProperty = new ExitGames.Client.Photon.Hashtable ();
			resetProperty.Add (RoomPropertyType.PlayerIDs [(int)otherPlayer.customProperties[PlayerProperties.PlayerIndex]], 0);
			PhotonNetwork.room.SetCustomProperties (resetProperty);
		}
	}

	public void updateNumPlayersInRoom(int playerIndex, bool isInRoom){
		PhotonView photonView = PhotonView.Get(this);
		photonView.RPC ("UpdateNumPlayersInRoom_RPC", PhotonTargets.AllBufferedViaServer, playerIndex, isInRoom);
	}

	[PunRPC]
	void UpdateNumPlayersInRoom_RPC(int playerIndex, bool isInRoom)
	{
		playersInRoom[playerIndex] = isInRoom;
	}

	public void LeaveRoom(){
		StartCoroutine (CoLeaveRoom ());
	}

	IEnumerator CoLeaveRoom(){
		yield return new WaitForSeconds (3f);
		gamePlayPanelScript.ResetGamePanel ();
		PhotonNetwork.LeaveRoom ();

	}

}



