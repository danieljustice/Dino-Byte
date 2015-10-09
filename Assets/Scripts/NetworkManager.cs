using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera sceneCamera;
	[SerializeField] GameObject candies;
	[SerializeField] GameObject lobbyPanel;
	[SerializeField] GameObject gamePlayPanel;
	[SerializeField] InputField playerName;
	[SerializeField] Text[] playerNameOnScoreBoard;
	PlayerIndicatorUpdater playerIndicatorUpdater;
	public SetUpGame setUpGame;

	public bool[] playersInRoom = new bool[4];
	int index = 0;
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
		TurnOnLobbyPanel ();
		playerIndicatorUpdater.TurnOffAllIndicators ();

	}

	public void JoinRoom()
	{
		PhotonNetwork.player.name = playerName.text;
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 4};
		PhotonNetwork.JoinOrCreateRoom ("Dan", ro, TypedLobby.Default);
	}
	
	void OnJoinedRoom()
	{
		StartSpawnProcess (0f);
		setUpGame.setUpGameOnJoinedRoom ();
	}
	
	void StartSpawnProcess (float respawnTime)
	{
		sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}
	
	IEnumerator SpawnPlayer (float respawnTime)
	{
		yield return new WaitForSeconds(respawnTime);

		index = 0;
		while (playersInRoom[index] == true && index < 4) 
		{
			index++;
		}
		PhotonView photonView = PhotonView.Get(this);
		photonView.RPC ("UpdatePlayers_RPC", PhotonTargets.AllBufferedViaServer, index, true);
		player = PhotonNetwork.Instantiate ("Player_1", spawnPoints [index].position, spawnPoints [index].rotation, 0);
		playersInRoom [index] = true;
		sceneCamera.enabled = false;

		player.transform.Find ("crappy_player").gameObject.tag = "Player" + (index + 1).ToString();
	
		player.GetComponentInChildren<Destroy_Candy> ().enabled = true;

		photonView.RPC ("PutPlayerNameOnScoreBoard_RPC", PhotonTargets.AllBufferedViaServer, PhotonNetwork.player.name, index);


	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}

	void TurnOnLobbyPanel()
	{
		//Lobby Panel and Game Play Panel should not be active at the same time
		lobbyPanel.SetActive (true);
		gamePlayPanel.SetActive (false);
	}

	[PunRPC]
	void PutPlayerNameOnScoreBoard_RPC(string name, int playerPosition)
	{
		playerNameOnScoreBoard [playerPosition].text = name;
	}

	[PunRPC]
	void UpdatePlayers_RPC(int i, bool flag)
	{
		playersInRoom [i] = flag;
	}

}



