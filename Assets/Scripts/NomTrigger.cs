using UnityEngine;
using System.Collections;

public class NomTrigger : MonoBehaviour {

	public GameObject player;
	private NPC_Nom npcNom;

	void Awake(){
		npcNom = player.GetComponent<NPC_Nom> ();
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Candy") {
			npcNom.startNom = true;
		}
	}

}
