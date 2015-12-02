using UnityEngine;
using System.Collections;

public class CandyController : MonoBehaviour {

	public GameObject[] candies;

	public float spawnWait;
	public float startWait;

	public PhotonView photonView;

	public TimerScript timerScript;



	public void ResetAllCandies(){
		foreach (GameObject currentCandy in candies) 
		{
			ResetCandy(currentCandy);
		}
	}

	public void ResetCandy(GameObject candy){
		ResetCandyPosition(candy);
		ResetCandyVelocities(candy);
		TurnOffCandyMesh(candy);
		TurnOffCandyCollider(candy);
		CandyIsKinematic(candy);
		candy.GetComponent<CandyMotionController> ().StopCandyMovement ();
	}
	
	public void ResetCandyPosition(GameObject candy){
		candy.transform.position = candy.transform.parent.transform.position;
		candy.transform.rotation = Quaternion.identity;
	}
	
	public void ResetCandyVelocities(GameObject candy){
		Rigidbody candyRB = candy.GetComponent<Rigidbody> ();
		candyRB.velocity = Vector3.zero;
		candyRB.angularVelocity = Vector3.zero;
	}

	public void TurnOnCandyMesh(GameObject candy){
		MeshRenderer candyMR = candy.GetComponent<MeshRenderer> ();
		candyMR.enabled = true;
	}

	public void TurnOffCandyMesh(GameObject candy){
		MeshRenderer candyMR = candy.GetComponent<MeshRenderer> ();
		candyMR.enabled = false;
	}

	public void TurnOnCandyCollider(GameObject candy){
		Collider candyCollider = candy.GetComponent<Collider> ();
		candyCollider.enabled = true;
	}

	public void TurnOffCandyCollider(GameObject candy){
		Collider candyCollider = candy.GetComponent<Collider> ();
		candyCollider.enabled = false;
	}

	public void CandyIsKinematic(GameObject candy){
		Rigidbody candyRB = candy.GetComponent<Rigidbody> ();
		candyRB.isKinematic = true;
	}

	public void CandyIsNotKinematic(GameObject candy){
		Rigidbody candyRB = candy.GetComponent<Rigidbody> ();
		candyRB.isKinematic = false;
	}
	
	public void StartCandySpawn(){
		if (PhotonNetwork.isMasterClient) {
			StartCoroutine (SpawnCandy ());
		}
	}
	
	IEnumerator SpawnCandy(){
		yield return new WaitForSeconds (startWait);

		int candyIndex = 0;
		while (candyIndex < candies.Length) {
			photonView.RPC("TurnOnCandyMesh_RPC", PhotonTargets.AllBufferedViaServer, candyIndex);
			photonView.RPC("StartCandyMovement_RPC", PhotonTargets.AllBufferedViaServer, candyIndex);
			photonView.RPC("TurnOnCandyCollider_RPC", PhotonTargets.AllBufferedViaServer, candyIndex);
			//candy is NOT kinematic only for master client
			CandyIsNotKinematic(candies[candyIndex]);
			candyIndex++;
			yield return new WaitForSeconds(spawnWait);
		}
		timerScript.StartCountDown ();
	}

	[PunRPC]
	void TurnOnCandyMesh_RPC(int candyIndex){
		TurnOnCandyMesh (candies[candyIndex]);
	}
	[PunRPC]
	void TurnOffCandyMesh_RPC(int candyIndex){
		TurnOffCandyMesh (candies[candyIndex]);
	}
	[PunRPC]
	void TurnOnCandyCollider_RPC(int candyIndex){
		TurnOnCandyCollider (candies [candyIndex]);
	}
	[PunRPC]
	void TurnOffCandyCollider_RPC(int candyIndex){
		TurnOffCandyCollider (candies [candyIndex]);
	}
	[PunRPC]
	void StartCandyMovement_RPC(int candyIndex){
		candies[candyIndex].GetComponent<CandyMotionController> ().startCandyMovement ();
	}

}
