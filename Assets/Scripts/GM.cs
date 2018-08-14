using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

	public static GM instance = null;

	public float yMinLive = -9f;

	public Transform spawnPoint;
	public GameObject playerPrefab;

	public float maxTime = 120f;
	bool timerOn = true;
	float timeLeft;

	public UI ui;
	GameData data = new GameData();

	PlayerCtrl player;

	public float timeToRespawn = 2f;
	


	void Awake(){
		if(instance == null){
			instance = this;
		}
	}


	// Use this for initialization
	void Start () {
		if (player == null){
			RespawnPlayer();
		}
		timeLeft = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null){
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if (obj != null){
				player = obj.GetComponent<PlayerCtrl>();
			}
		}
		UpdateTimer();
		DisplayHudData();
	}
	
	void UpdateTimer(){
		if(timerOn){
			timeLeft = timeLeft - Time.deltaTime;
			if(timeLeft <= 0f){
			timeLeft = 0;
			ExpirePlayer();	
			}
		}
	}

	void DisplayHudData(){
		if (data.coinCount < 10){
		ui.hud.txtCoinCount.text = "x 0" + data.coinCount;
	}
		else {
		ui.hud.txtCoinCount.text = "x " + data.coinCount;
		}
		ui.hud.txtTimer.text = timeLeft.ToString("F0");
	}
	public void IncrementCoinCount(){
		data.coinCount++;
	}
	
	public void RespawnPlayer(){
	Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
}

	public void KillPlayer(){
		if (playerPrefab != null){
			Destroy(player.gameObject);
			Invoke("RespawnPlayer", timeToRespawn);
		}
	}

	public void ExpirePlayer(){
		if (playerPrefab != null){
			Destroy(player.gameObject);
		}
		GameOver();
	}

	void GameOver(){
		timerOn = false;
		if (data.coinCount < 10){
		ui.gameOver.txtCoinCount.text = "Coins: 0" + data.coinCount;
	}
		else {
		ui.gameOver.txtCoinCount.text = "Coins: " + data.coinCount;
		}
		ui.hud.txtTimer.text = "Timer: " + timeLeft.ToString("F0");
		ui.gameOver.gameOverPanel.SetActive(true);
	}
}

