using UnityEngine;
using System.Collections;

public class CandyMotionController : MonoBehaviour {

	//The purpose of this script is to control the behavior of the candies when 
	//they are 'spawned' and then send/receive the transform information to/from the network

	public float tumble;
	public Rigidbody rb;
	public float thrust;
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	bool initialLoad = true;
	float timeOfLastImpulse;
	public float timeBetweenImpulses;


	public void startCandyMovement () {
		if (PhotonNetwork.isMasterClient) 
		{
			//Gives candy a random rotation on spawn
			GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble; 
			applyRandomImpulse();
		}

		else
		{
			StopCoroutine("UpdateData");
			StartCoroutine ("UpdateData");
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
		rb = GetComponent<Rigidbody> ();
		Vector3 impulseDirection = new Vector3 (Random.Range (-10f, 10f), 0f, Random.Range (-10f, 10f));
		rb.AddForce (impulseDirection * thrust, ForceMode.Impulse);
	}


	void Update () {

		if (timeOfLastImpulse <= Time.time) 
		{
			applyRandomImpulse();
			timeOfLastImpulse = Time.time + timeBetweenImpulses;
		}
	
	}
}
