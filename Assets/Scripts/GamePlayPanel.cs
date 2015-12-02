using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour {

	public int thisPlayersScore = 0;

	public TimerScript timerScript;

	public GameObject[] playerScores;
	public GameObject[] playerNames;

	public Text[] playerScoreTexts;
	public Text[] playerNameTexts;
	public Text winnerText;

	public PhotonView photonView;


	public void UpdatePlayerScore(int playerIndex, int scoreIncrement){
		thisPlayersScore = thisPlayersScore + scoreIncrement;

		photonView.RPC ("UpdatePlayerScore_RPC", PhotonTargets.AllBuffered, playerIndex, thisPlayersScore);

		ExitGames.Client.Photon.Hashtable playerScore = new ExitGames.Client.Photon.Hashtable ();
		playerScore.Add (RoomPropertyType.PlayerScores [playerIndex], thisPlayersScore);
		PhotonNetwork.room.SetCustomProperties (playerScore);

	}

	[PunRPC]
	public void UpdatePlayerScore_RPC(int playerIndex, int score){
		playerScoreTexts [playerIndex].text = score.ToString ();
	
	}

	public void UpdatePlayerName(int playerIndex, string name){
		photonView.RPC ("UpdatePlayerName_RPC", PhotonTargets.AllBuffered, playerIndex, name);
	}

	[PunRPC]
	public void UpdatePlayerName_RPC(int playerIndex, string name){
		playerNameTexts [playerIndex].text = name;
	}

	public void ResetGamePanel(){
		ResetPlayerNameTexts ();
		ResetPlayerScoreTexts ();
		ResetWinnerText ();
		timerScript.ResetTimerText ();
	}

	public void ResetPlayerNameTexts(){
		int nameIndex = 1;
		foreach (Text currentName in playerNameTexts) {
			currentName.text = "Player " + nameIndex;
			nameIndex++;
		}
	
	}

	public void ResetPlayerScoreTexts(){
		foreach (Text currentScore in playerScoreTexts) {
			currentScore.text = "0";
		}
		thisPlayersScore = 0;
	}

	public void SetWinnerText(string winnerTextString){
		winnerText.text = winnerTextString;
	}

	public void ResetWinnerText(){
		winnerText.text = "";
	}

}
