using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	//[HideInInspector]public static float points;
	Text scoretext;
	// Use this for initialization
	void Start () {
		scoretext = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		scoretext.text = "Score : " + Singleton.Instance.currentPlayer.playerScore;
		
		//ENABLE BONUS PLAYER SELECTIONS
		if(Singleton.Instance.currentPlayer.playerScore >= 50)
			Singleton.Instance.playerData.ball2Enabled = true;
		if(Singleton.Instance.currentPlayer.playerScore >= 100)
			Singleton.Instance.playerData.ball3Enabled = true;
	}
}
