using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerIndicatorUpdater : MonoBehaviour
{

	//int numOfPlayers;
	int numOfReadyPlayers;
	int currentNumOfPlayers;
	[SerializeField] Toggle[] inRoomIndicators;
	[SerializeField] Toggle[] readyToPlayIndicators;
	[SerializeField] Button readyButton;
	[SerializeField] GameObject lobbyPanel;
	[SerializeField]
	GameObject gamePlayPanel;

	bool allPlayersInRoom = false;
	bool playerReady = false;
	bool thisPlayerReady;
	public GameObject candies;
	public GameController gameController;


	void ResetKeyValues ()
	{
		numOfReadyPlayers = 0;
		currentNumOfPlayers = 0;
		thisPlayerReady = false;
		setReadyButtonState (thisPlayerReady);
		turnOffAllIndicators ();

	}

	void OnJoinedRoom ()
	{
		ResetKeyValues ();
		CheckNumOfPlayersInRoom ();
		UpdatePlayerIndicator ();
	}

	void OnLeftRoom ()
	{
		ResetKeyValues ();
		UpdatePlayerIndicator ();
	}

	void OnPhotonPlayerConnected ()
	{
		CheckNumOfPlayersInRoom ();
		UpdatePlayerIndicator ();
	}

	void OnPhotonPlayerDisconnected ()
	{
		CheckNumOfPlayersInRoom ();
		UpdatePlayerIndicator ();
	}

	void CheckNumOfPlayersInRoom ()
	{
		if (currentNumOfPlayers != PhotonNetwork.playerList.Length) {
			if (PhotonNetwork.playerList.Length == RoomPropertyType.MaxPlayers) {
				EnableReadyButton ();
			} else if (readyButton.interactable) {
				DisableReadyButton ();
			}
			currentNumOfPlayers = PhotonNetwork.playerList.Length;
		}
	}

	void UpdatePlayerIndicator ()
	{

		for (int currentPIndicator = 0; currentPIndicator < currentNumOfPlayers; currentPIndicator++) {
			inRoomIndicators [currentPIndicator].isOn = true;
		}

		for (int currentPIndicator = currentNumOfPlayers; currentPIndicator < RoomPropertyType.MaxPlayers; currentPIndicator++) {
			inRoomIndicators [currentPIndicator].isOn = false;
		}

	}

	void EnableReadyButton ()
	{
		readyButton.interactable = true;
	}

	void DisableReadyButton ()
	{
		readyButton.interactable = false;
		setReadyButtonState (false);
		SetAllReadyIndicatorsToFalse ();
	}


	//Called from ready button on lobby panel
	public void UpdatePlayerReadyIndicators ()
	{
		thisPlayerReady = !thisPlayerReady;

		PhotonView photonView = PhotonView.Get (this);
		if (thisPlayerReady) {
			photonView.RPC ("IncreaseNumOfReadyPlayers_RPC", PhotonTargets.AllBuffered);
		} else {
			//numOfReadyPlayers--;
			photonView.RPC ("DecreaseNumOfReadyPlayers_RPC", PhotonTargets.AllBuffered);
		}

		setReadyButtonState (thisPlayerReady);
		Debug.LogError (numOfReadyPlayers);
		//Start game when everyone clicks ready
		if (PhotonNetwork.isMasterClient && (numOfReadyPlayers == RoomPropertyType.MaxPlayers)) {
			photonView.RPC ("InitiateGame_RPC", PhotonTargets.AllBufferedViaServer);
		}
	}

	[PunRPC]
	void IncreaseNumOfReadyPlayers_RPC ()
	{
		numOfReadyPlayers++;
		readyToPlayIndicators [numOfReadyPlayers - 1].isOn = true;
	}

	[PunRPC]
	void DecreaseNumOfReadyPlayers_RPC ()
	{
		numOfReadyPlayers--;
		readyToPlayIndicators [numOfReadyPlayers].isOn = false;
	}
	
	public void setReadyButtonState (bool playerReady)
	{	
		//set the global variable this.playerReady to playerReady so that if 
		//an outside script calls this function everything lines up
		this.playerReady = playerReady;
		if (!playerReady) {
			readyButton.GetComponentInChildren<Text> ().text = "Ready!";
		} else {
			readyButton.GetComponentInChildren<Text> ().text = "Not Ready!";
		}
	}

	public void turnOffAllIndicators ()
	{
		//Normally called from NetworkManager.cs when the player joins the lobby
		//Only does this locally
		foreach (Toggle currentIndicator in inRoomIndicators) {
			currentIndicator.isOn = false;
		}
		foreach (Toggle currentIndicator in readyToPlayIndicators) {
			currentIndicator.isOn = false;
		}
	}

	void SetAllReadyIndicatorsToFalse ()
	{
		foreach (Toggle currentTog in readyToPlayIndicators) {
			currentTog.isOn = false;
		}
	}

	[PunRPC]
	void InitiateGame_RPC ()
	{
		gameController.StartGame ();
		/*
		lobbyPanel.SetActive (false);
		gamePlayPanel.SetActive (true);
		destroy_candy.FindPlayerScoreBoard ();
		candies.GetComponent<RandomSpawner> ().StartSpawningCandies ();
		*/
	}


}



