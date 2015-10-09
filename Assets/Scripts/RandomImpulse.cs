using UnityEngine;
using System.Collections;

public class RandomImpulse : MonoBehaviour {
	
	public Rigidbody rb;
	public float thrust;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Vector3 impulseDirection = new Vector3 (Random.Range (-10f, 10f), 0f, Random.Range (-10f, 10f));
		rb.AddForce (impulseDirection * thrust, ForceMode.Impulse);
	}
	
}