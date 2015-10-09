using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerIndicatorUpdater : MonoBehaviour {

	int numOfPlayers;
	int numOfReadyPlayers;
	[SerializeField] Toggle[] inRoomIndicators;
	[SerializeField] Toggle[] readyToPlayIndicators;
	[SerializeField] Button readyButton;
	[SerializeField] GameObject lobbyPanel;
	[SerializeField] GameObject gamePlayPanel;
	Destroy_Candy destroy_candy;
	bool allPlayersInRoom = false;
	bool playerReady = false;
	public GameObject candies;
	//public Destroy_Candy destroy_Candy;

	// Use this for initialization
	void Start () {
	
	}

	//Destray_Candy script on the client's character sends it's own gameobject
	//as a package.  This will be used later to initialize the FindPlayerScoreBoard function
	public void GetPlayerDestroyCandyComponent(GameObject destroyCandyGameObject)
	{
		destroy_candy = destroyCandyGameObject.GetComponent<Destroy_Candy> ();
	}

	void OnJoinedRoom()
	{
		StartCoroutine (UpdatePlayerIndicator ());
	}

	IEnumerator UpdatePlayerIndicator()
	{
		PhotonView photonView = PhotonView.Get (this);
		while (true) 
		{
			numOfPlayers = PhotonNetwork.playerList.Length;
			int i = 0;
			if(numOfPlayers == 4)
			{
				StartCoroutine(EnableReadyButton());
				allPlayersInRoom = true;
			}

			while(i < inRoomIndicators.Length)
			{
				if(i < numOfPlayers)
				{
					inRoomIndicators[i].isOn = true;
				}
				else
				{
					inRoomIndicators[i].isOn = false;
					//This if statement takes care of the indicators once 
					//everyone joins and then one client drops out
					if(allPlayersInRoom)
					{
						allPlayersInRoom = false;
						StartCoroutine(DisableReadyButton());
						playerReady = false;
						SetReadyButtonState();
						photonView.RPC ("SetAllReadyIndicatorsToFalse_RPC", PhotonTargets.AllBufferedViaServer);
					}
				}

				i++;
				yield return null;
			}
			yield return null;
		}
	}



	IEnumerator EnableReadyButton()
	{
		readyButton.interactable = true;
		yield return null;
	}

	IEnumerator DisableReadyButton()
	{
		readyButton.interactable = false;
		yield return null;
	}



	public void UpdatePlayerReadyIndicators()
	{
		StartCoroutine (UpdatePlayerReadyIndicators_IEnumerator ());
	}



	IEnumerator UpdatePlayerReadyIndicators_IEnumerator()
	{
		int x = 0;
		while (x < readyToPlayIndicators.Length && readyToPlayIndicators[x].isOn == true) 
		{
			x++;
			yield return null;
		}
		PhotonView photonView = PhotonView.Get (this);
		if (!playerReady) 
		{
			playerReady = true;
			photonView.RPC ("UpdatePlayerReadyIndicators_RPC", PhotonTargets.AllBufferedViaServer, x, playerReady);
			SetReadyButtonState ();
		} 
		else 
		{
			playerReady = false;
			photonView.RPC ("UpdatePlayerReadyIndicators_RPC", PhotonTargets.AllBufferedViaServer, x-1, playerReady);
			SetReadyButtonState ();
		}

		//what to do once all clients are joined and clicked "Ready!"
		if (playerReady && x >= readyToPlayIndicators.Length - 1) 
		{
			photonView.RPC ("InitiateGame_RPC", PhotonTargets.AllBufferedViaServer);

		}

	}

	public void SetReadyButtonState()
	{
		if (!playerReady) 
		{
			readyButton.GetComponentInChildren<Text> ().text = "Ready!";
		} 
		else 
		{
			readyButton.GetComponentInChildren<Text> ().text = "Not Ready!";
		}
	}

	public void TurnOffAllIndicators()
	{
		//Normally called from NetworkManager.cs when the player joins the lobby
		//Only does this locally
		foreach (Toggle currentIndicator in inRoomIndicators) 
		{
			currentIndicator.isOn = false;
		}
		foreach (Toggle currentIndicator in readyToPlayIndicators) 
		{
			currentIndicator.isOn = false;
		}
	}


	[PunRPC]
	void UpdatePlayerReadyIndicators_RPC(int x, bool readiness)
	{
		readyToPlayIndicators [x].isOn = readiness;
	}

	[PunRPC]
	void SetAllReadyIndicatorsToFalse_RPC()
	{
		foreach(Toggle x in readyToPlayIndicators)
		{
			x.isOn= false;
		}
	}


	[PunRPC]
	void InitiateGame_RPC()
	{
		lobbyPanel.SetActive(false);
		gamePlayPanel.SetActive(true);
		destroy_candy.FindPlayerScoreBoard();
		candies.GetComponent<RandomSpawner> ().StartSpawningCandies();
	}


}



