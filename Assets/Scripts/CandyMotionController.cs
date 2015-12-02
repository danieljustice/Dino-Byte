using UnityEngine;
using System.Collections;

public class CandyMotionController : MonoBehaviour {

	//The purpose of this script is to control the behavior of the candies when 
	//they are 'spawned' and then send/receive the transform information to/from the network
	
	public Rigidbody candyRB;
	public float thrust;
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	bool initialLoad = true;
	float timeOfNextImpulse;
	public float timeBetweenImpulses;

	public int candyValue = 1;

	public CandyController candyController;
	public GamePlayPanel gamePlayPanel;


	public void startCandyMovement () {
		if (PhotonNetwork.isMasterClient) {
			candyRB.isKinematic = false;
			StartCoroutine(TimedImpulses());
		}else{
			StopCoroutine("UpdateData");
			StartCoroutine ("UpdateData");
		}
	}

	public void StopCandyMovement(){
		if (PhotonNetwork.isMasterClient) {
			StopCoroutine(TimedImpulses());
		} else {
			StopCoroutine(UpdateData());
		}
	}

	//uses incoming data packets to lerp position and rotation.  Uses a smoothing variable.
	IEnumerator UpdateData()
	{
		while (true) 
		{
			if(initialLoad)
			{
				transform.position = position;
				transform.rotation = rotation;
				initialLoad = false;
			}
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime*smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting == true) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} 
		else 
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
		}
	}

	void applyRandomImpulse()
	{
		//Adds random impulse to candy on spawn
		Vector3 impulseDirection = new Vector3 (Random.Range (-10f, 10f), 0f, Random.Range (-10f, 10f));
		candyRB.AddForce (impulseDirection * thrust, ForceMode.Impulse);
	}


	IEnumerator TimedImpulses(){
		while (true) {
			if (timeOfNextImpulse <= Time.time) {
				applyRandomImpulse ();
				timeOfNextImpulse = Time.time + timeBetweenImpulses;
			}
			yield return null;
		}
	}

	public void EatCandy(){
		gamePlayPanel.UpdatePlayerScore((int)PhotonNetwork.player.customProperties[PlayerProperties.PlayerIndex], candyValue);

		PhotonView photonView = PhotonView.Get (this);
		photonView.RPC ("TurnOffCandy_RPC", PhotonTargets.AllBuffered);
	}

	[PunRPC]
	void TurnOffCandy_RPC()
	{
		if (this.gameObject == null) 
		{
			Debug.LogError(this.gameObject.name);
		}
		else
		{
			candyController.ResetCandy(this.gameObject);
		}
		
	}
}
