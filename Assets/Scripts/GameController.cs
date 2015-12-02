using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {



	public LobbyPanel lobbyPanelScript;
	public GamePlayPanel gamePlayPanelScript;
	public NetworkManager networkManager;
	public CandyController candyController;

	public GameObject lobbyPanel;
	public GameObject gamePlayPanel;
	GameObject player;

	public Transform[] spawnPoints;

	public Camera lobbyCamera;
	private Camera gameCamera;


	//This script should be the central script for starting, ending, and restarting a game
	//Will need functions to:
		//Reset the scores
		//Start count down
		//End game at count down == 0
		//Calculate and display winner
		//Leave room and return to lobby
		//Set all scripts, values, Gui, etc. upon entering lobby
		//Start game function

	public void setUpLobby(){
		turnOnLobbyPanel ();
		lobbyPanelScript.ResetLobby ();
		turnOffGamePlayPanel();
		candyController.ResetAllCandies ();
	}

	public void setUpRoom(int playerPosition){
		candyController.ResetAllCandies ();
		StartCoroutine (spawnPlayer (playerPosition));
	}

	public void StartGame(){
		turnOffLobbyPanel ();
		turnOnGamePlayPanel ();
		gamePlayPanelScript.ResetPlayerScoreTexts ();
		candyController.StartCandySpawn ();
	}

	IEnumerator spawnPlayer (int playerPosition)	{

		player = PhotonNetwork.Instantiate ("Player_1", spawnPoints [playerPosition-1].position, spawnPoints [playerPosition-1].rotation, 0);
		
		player.transform.Find ("crappy_player").gameObject.tag = "Player" + (playerPosition).ToString();
		
		player.GetComponentInChildren<Destroy_Candy> ().enabled = true;

		gameCamera = player.GetComponentInChildren<Camera> ();
		yield return null;
	}

	void turnOnLobbyPanel()
	{
		lobbyPanel.SetActive (true);
		lobbyCamera.enabled = true;
	}

	void turnOffLobbyPanel(){
		lobbyPanel.SetActive (false);
		lobbyCamera.enabled = false;
	}

	void turnOnGamePlayPanel(){
		gamePlayPanel.SetActive (true);
		gameCamera.enabled = true;
	}

	void turnOffGamePlayPanel(){
		gamePlayPanel.SetActive (false);

		if (gameCamera) {
			gameCamera.enabled = false;
		}
	}


	public void EndGame(){
		player.GetComponentInChildren<Destroy_Candy> ().enabled = false;
		ShowWinner ();
		networkManager.LeaveRoom();
	}

	public void ShowWinner(){

		int[] playerScores = GetScoresAsIntegers();
		int playerIndex = FindWinner (playerScores);
		string winnerTextString = CreateWinnerText (playerIndex);
		gamePlayPanelScript.SetWinnerText (winnerTextString);
	}

	public int[] GetScoresAsIntegers(){
		int[] playerScores = new int[RoomPropertyType.PlayerScores.Length];
		int scoreIndex = 0;
		foreach (string currentScore in RoomPropertyType.PlayerScores) {
			Debug.LogError("Score as string: " + currentScore);
			int.TryParse (PhotonNetwork.room.customProperties[currentScore].ToString(), out playerScores [scoreIndex]);
			Debug.LogError(playerScores[scoreIndex]);
			scoreIndex++;
		}
		return playerScores;
	}

	public int FindWinner(int[] playerScores){
		int maxScore = Mathf.Max (playerScores);
		Debug.LogError ("Max Score: " + maxScore);
		int playerIndex = 0;
		foreach (int currentScore in playerScores) {
			if(currentScore == maxScore){
				break;
			}
			playerIndex++;
		}
		return playerIndex;
	}

	public string CreateWinnerText(int playerIndex){
		string winnerTextString;
		winnerTextString = "Player " + (playerIndex + 1) + "\n" + "WON!";
		return winnerTextString;
	}
}
