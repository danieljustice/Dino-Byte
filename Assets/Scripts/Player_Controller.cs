using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour {
	public float verticalSpeed;
	public float horizontalSpeed;
	public float rotationSpeed;
	public float rotationDegree;
	public float gravity;
	private Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		moveDirection = new Vector3 (0.0f, 0.0f, 0.0f);
		moveDirection = transform.TransformDirection(moveDirection);

		if (Input.GetButton ("Jump")) {
			moveDirection.z=horizontalSpeed;
			moveDirection.y=verticalSpeed;
		
		}
	 	moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	
	}
}
