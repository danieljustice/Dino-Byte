using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

	public InputField playerName;
	public Toggle[] inRoomToggles;
	public Toggle[] playerReadyToggle;
	public Button joinRoomButton;
	public Button readyButton;
	public Text connectionText;
	public PlayerIndicatorUpdater playerIndicatorUpdater;




	public void ResetLobby(){
		playerIndicatorUpdater.turnOffAllIndicators ();
		playerIndicatorUpdater.setReadyButtonState (false);

	}
}
