using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	
	public float timeToKill = 1f;


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

	public void Restartlevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitToMainMenu(){
		LoadScene("MainMenu");
	}

	public void CloseApp(){
		Application.Quit();
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
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
		if (data.lifeCount < 10){
		ui.hud.txtLifeCount.text = "x 0" + data.lifeCount;
	}
		else {
		ui.hud.txtLifeCount.text = "x " + data.lifeCount;
		}
	}
	public void IncrementCoinCount(){
		data.coinCount++;
	}
	
	public void RespawnPlayer(){
	Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
}

	public void DecrementLives(){
		data.lifeCount--;
	}
	public void KillPlayer(){
		if (playerPrefab != null){
			Destroy(player.gameObject);
			DecrementLives();
			if (data.lifeCount > 0){
				Invoke("RespawnPlayer", timeToRespawn);
			}
			else {
				GameOver();
			}
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
		ui.gameOver.txtTimer.text = "Timer: " + timeLeft.ToString("F0");
		if (data.coinCount < 10){
		ui.gameOver.txtCoinCount.text = "Coins: 0" + data.coinCount;
	}
		else {
		ui.gameOver.txtCoinCount.text = "Coins: " + data.coinCount;
		}
		ui.gameOver.gameOverPanel.SetActive(true);
	}

	public void LevelComplete(){
		Destroy(player.gameObject);
		timerOn = false;
		ui.levelComplete.txtTimer.text = "Timer: " + timeLeft.ToString("F0");
		if (data.coinCount < 10){
		ui.levelComplete.txtCoinCount.text = "Coins: 0" + data.coinCount;
	}
		else {
		ui.levelComplete.txtCoinCount.text = "Coins: " + data.coinCount;
		}
		ui.levelComplete.levelCompletePanel.SetActive(true);
	}

		public void HurtPlayer(){
		if (playerPrefab != null){
			DisableAndPushPlayer();
			Destroy(player.gameObject, timeToKill);
			DecrementLives();
			if (data.lifeCount > 0){
				Invoke("RespawnPlayer", timeToKill + timeToRespawn);
			}
			else {
				GameOver();
			}
		}
	}

	void DisableAndPushPlayer(){
		player.transform.GetComponent<PlayerCtrl>().enabled = false;
		foreach (Collider2D c2d in player.transform.GetComponents<Collider2D>()){
			c2d.enabled = false;
		}
		foreach (Transform child in player.transform){
			child.gameObject.SetActive(false);
		}
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.AddForce(new Vector2(-150.0f, 400f));
	}
}

