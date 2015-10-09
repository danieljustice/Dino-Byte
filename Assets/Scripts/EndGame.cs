using UnityEngine;
using System.Collections;
using UnityEngine.UI;



//This script currently lives on the Time UI piece on the gamplay panel

public class EndGame : MonoBehaviour {
	GameObject[] players;
	public Text[] playerScores;
	int[] scores;
	string winningPlayersText;
	public Text winnerText;
	public float waitToLeaveRoom;
	public NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");

		foreach(GameObject currentPlayer in players)
		{
			currentPlayer.GetComponentInChildren<Player_Nom>().enabled = false;
			currentPlayer.GetComponentInChildren<Destroy_Candy>().enabled = false;
		}

		scores = new int[playerScores.Length];
		int i = 0;
		foreach ( Text currentScore in playerScores)  
		{
			string tempScore = currentScore.text;
			int.TryParse(tempScore, out scores[i]);

			i++;
		}
		i = 0;
		int highScore = Mathf.Max (scores);
		foreach (int currentScore in scores) 
		{
			if(currentScore == highScore)
			{
				winningPlayersText = winningPlayersText + "Player " + (i + 1) + "\n";
			}
			i++;
		}

		winnerText.text = winningPlayersText + "WON!";
		StartCoroutine (leaveRoom());


	}


	IEnumerator leaveRoom()
	{
		yield return new WaitForSeconds (waitToLeaveRoom);
		PhotonNetwork.LeaveRoom();
		int a = 0;
		while(a < networkManager.playersInRoom.Length)
		{
			networkManager.playersInRoom[a] = false;
			a++;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
