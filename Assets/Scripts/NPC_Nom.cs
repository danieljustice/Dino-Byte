using UnityEngine;
using System.Collections;

public class NPC_Nom : MonoBehaviour {

	public bool startNom;
	Animator anim;
	int nomHash = Animator.StringToHash("Nom");
	void Awake(){
		anim = GetComponent < Animator> ();
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (startNom) {
			anim.SetTrigger(nomHash);
		}
		startNom = false;
	}
	
	
}
