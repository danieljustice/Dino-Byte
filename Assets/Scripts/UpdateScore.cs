using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour {

	string oldScore;
	string currentScore;
	Text txt;
	bool flag;

	// Use this for initialization
	void Start () {
		//txt = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentScore = GetComponent<Text> ().text;
		if (oldScore != currentScore) 
		{ 
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC ("UpdateScore_RPC", PhotonTargets.AllBufferedViaServer, currentScore);

		}

	}


	[PunRPC]
	void UpdateScore_RPC(string score)
	{
		GetComponent<Text> ().text = score.ToString();

		oldScore = score;
	}

}
