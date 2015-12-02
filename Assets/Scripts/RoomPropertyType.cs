using UnityEngine;
using System.Collections;

//This class acts as an enumerator for the different types of room properties available

public class RoomPropertyType : MonoBehaviour{

	public static readonly string Player1ID = "P1";
	public static readonly string Player2ID = "P2";
	public static readonly string Player3ID = "P3";
	public static readonly string Player4ID = "P4";

	public static readonly string[] PlayerIDs = {Player1ID, Player2ID, Player3ID, Player4ID};

	public const string Player1Score = "S1";
	public const string Player2Score = "S2";
	public const string Player3Score = "S3";
	public const string Player4Score = "S4";

	public static readonly string[] PlayerScores = {Player1Score, Player2Score, Player3Score, Player4Score};

	public const string EndTime = "ET";

	public const int MaxPlayers = 4;
}
