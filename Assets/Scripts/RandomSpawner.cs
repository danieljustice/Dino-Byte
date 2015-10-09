using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomSpawner : MonoBehaviour
{



	public GameObject candy;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	[SerializeField] GameObject[] candies;
	Text timerText;
	float timeEnd;
	public float countDownTime;
	float timeLeft;
	public float countDownTimeRemaining;
	bool flag = true;
	Vector3 candySpawnPosition = new Vector3(0f, 0f, 0f);


	

	public void StartSpawningCandies()
	{
		//PlayerIndicatorUpdater.cs calls this function
		StartCoroutine (SpawnCandies ());
	}

	IEnumerator SpawnCandies ()
	{

		int candyCount = candies.Length;
		timerText = GameObject.Find ("Time").GetComponent<Text> ();
		int i = 0;
		yield return new WaitForSeconds (startWait);
	
		if(PhotonNetwork.isMasterClient)
		{
			while (i < candyCount) {


				candies[i].GetComponent<Rigidbody>().isKinematic = false;
				PhotonView photonView = PhotonView.Get(this);
				photonView.RPC ("turnCandiesOn_RPC", PhotonTargets.AllBufferedViaServer, i);
				i++;
				yield return new WaitForSeconds (spawnWait);
			}
			StartCoroutine(StartCountDown());
		}
	}


	IEnumerator StartCountDown()
	{
		//only runs on master client
		timeEnd = countDownTime + Time.time;
		timeLeft = countDownTime;
		PhotonView photonView = PhotonView.Get (this);
			while (timeLeft > 0) {
				countDownTimeRemaining = timeEnd - Time.time;
				if (timeLeft != (int)countDownTimeRemaining) {
					timeLeft = (int)countDownTimeRemaining;
					if (timeLeft < 0) {
						timeLeft = 0;
					}
					photonView.RPC ("updateTimer_RPC", PhotonTargets.AllBufferedViaServer, (int)timeLeft);
				}
				yield return null;
			}
		photonView.RPC ("startEndGameScript_RPC", PhotonTargets.AllViaServer);
	}


	[PunRPC]
	void turnCandiesOn_RPC (int y)
	{
		candies [y].SetActive (true);
		//candies [y].GetComponent<MeshRenderer> ().enabled = true;
		//candies [y].GetComponent<SphereCollider> ().enabled = true;
		candies [y].GetComponent<CandyMotionController> ().startCandyMovement ();
		
	}

	[PunRPC]
	void updateTimer_RPC (int timeRemaining)
	{
		timerText.text = timeRemaining.ToString ();
	}

	[PunRPC]
	void startEndGameScript_RPC ()
	{
		timerText.GetComponent<EndGame> ().enabled = true;
	}

	
}
