using UnityEngine;
using System.Collections;

public class SetUpGame : MonoBehaviour {

	public GameObject[] candies;
	Vector3 originPosition = new Vector3(0f, 0f, 0f);
	Rigidbody tempCandyRB;


	public void setUpGameOnJoinedRoom()
	{
		initializeCandies ();
	}

	private void initializeCandies()
	{
		foreach (GameObject currentCandy in candies) 
		{
			currentCandy.transform.localPosition = originPosition;
			tempCandyRB = currentCandy.GetComponent<Rigidbody>();
			tempCandyRB.velocity = Vector3.zero;
			tempCandyRB.angularVelocity = Vector3.zero;
			currentCandy.SetActive(false);
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
