using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Destroy_Candy : MonoBehaviour {

	public int score;
	public string playerTag;
	private GameObject canvas;
	public Transform scoreBoard;
	public Text tex;
	public PlayerIndicatorUpdater playerIndicatorUpdater;
	//[SerializeField] Canvas canvas;


	void Start()
	{
		score = 0;
		playerIndicatorUpdater = GameObject.Find ("PlayerIndicatorPanel").GetComponent<PlayerIndicatorUpdater> ();
		playerIndicatorUpdater.GetPlayerDestroyCandyComponent (gameObject);
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Candy")
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC ("TurnOffCandy_RPC", PhotonTargets.AllBuffered, other.gameObject.name);
			//PhotonNetwork.Destroy (other.gameObject);
			score +=1;
			tex.text = score.ToString();

		}
	}


	public void FindPlayerScoreBoard()
	{
		playerTag = transform.parent.tag;
		tex = GameObject.Find (playerTag + "Score").GetComponent<Text>();

	}


	[PunRPC]
	void TurnOffCandy_RPC(string Name)
	{
		if (GameObject.Find (Name) == null) 
		{
			Debug.LogError(Name);
		}
		else
		{
			GameObject.Find (Name).SetActive (false);
		}

	}


}
