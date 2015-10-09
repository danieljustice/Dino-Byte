using UnityEngine;
using System.Collections;

public class RandomrRotator : MonoBehaviour {

	public float tumble;
	
	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble; 
	}
}