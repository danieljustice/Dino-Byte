using UnityEngine;
using System.Collections;

public class Player_Nom : MonoBehaviour {
	//public bool Play(string animation, PlayMode mode = PlayMode.StopSameLayer);
	// Use this for initialization
	Animator anim;
	int nomHash = Animator.StringToHash("Nom");
	void Awake(){
		anim = GetComponent < Animator> ();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			anim.SetTrigger(nomHash);
		}
	
	}


}
