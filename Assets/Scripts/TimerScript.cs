using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {
	public double timeAfterLastCandy;
	public Text timerText;
	public PhotonView photonView;

	public GameController gameController;


	public void SetGameEndTime(){
		if (PhotonNetwork.isMasterClient) {
			ExitGames.Client.Photon.Hashtable endTime = new ExitGames.Client.Photon.Hashtable ();
			endTime.Add (RoomPropertyType.EndTime, PhotonNetwork.time + timeAfterLastCandy);
			PhotonNetwork.room.SetCustomProperties (endTime);
		}
	}

	public void StartCountDown(){
		SetGameEndTime ();
		photonView.RPC ("StartCountDown_RPC", PhotonTargets.AllBuffered);
	}

	[PunRPC]
	void StartCountDown_RPC(){
		StartCoroutine (CountDown ());
	}

	IEnumerator CountDown(){
		double endTime = (double)PhotonNetwork.room.customProperties[RoomPropertyType.EndTime];
		while(PhotonNetwork.time < endTime){
			timerText.text = ((int)(endTime - PhotonNetwork.time)).ToString();
			yield return null;
		}
		timerText.text = "0";
		gameController.EndGame ();
	}

	public void ResetTimerText(){
		timerText.text = "";
	}

	public void SetTimerText(string TimerText){
		timerText.text = TimerText;
	}

}
