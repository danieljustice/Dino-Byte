using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {
	
	Vector3 position;
	Quaternion rotation;
	Vector3 scale;
	float smoothing = 10f;
	public Camera playerCam;
	bool initialLoad = true;
	//GameObject playerBody;

	
	void Start () {
		//playerBody = GameObject.FindGameObjectWithTag("Player");
		//Debug.Log ("Awake: "+ photonView.viewID);
		if (photonView.isMine) {

			GetComponent<Player_Nom> ().enabled = true;
			GetComponent<Animator>().enabled = true;
			playerCam.GetComponent<Camera> ().enabled = true;
			playerCam.GetComponent<AudioListener> ().enabled = true;


		} 

		else 
		{
			StartCoroutine("UpdateData");
		}
	}
	
	IEnumerator UpdateData()
	{
		while (true) 
		{
			if(initialLoad)
			{
				transform.position = position;
				transform.rotation = rotation;
				transform.localScale = scale;
				initialLoad = false;
			}

			transform.position = Vector3.Lerp (transform.position, position, Time.deltaTime*smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * smoothing);
			yield return null;
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting == true) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (transform.localScale);
		//	Debug.Log("isWriting: "+ photonView.viewID);
		} 
		else 
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
			scale = (Vector3)stream.ReceiveNext();
			//Debug.Log("notWriting: "+ photonView.viewID);

		}
	}
	
}
