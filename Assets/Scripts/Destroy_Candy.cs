using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Destroy_Candy : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Candy")
		{
			other.GetComponent<CandyMotionController>().EatCandy();
		}
	}

}
