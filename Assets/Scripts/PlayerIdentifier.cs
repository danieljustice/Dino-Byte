using UnityEngine;
using System.Collections;

public class PlayerIdentifier : MonoBehaviour {


	public int playerNo;

	void Start(){
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			
			other.gameObject.tag = "Player" + playerNo;
		}
	}

}
