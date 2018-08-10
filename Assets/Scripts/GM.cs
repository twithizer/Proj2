﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

	public static GM instance = null;

	public float yMinLive = -9f;

	public Transform spawnPoint;
	public GameObject playerPrefab;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null){
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if (obj != null){
				player = obj.GetComponent<PlayerCtrl>();
			}
		}
		DisplayHudData();
	}
	
	void DisplayHudData(){
		if (data.coinCount < 10){
		ui.hud.txtCoinCount.text = "x 0" + data.coinCount;
	}
		else {
		ui.hud.txtCoinCount.text = "x " + data.coinCount;
		}
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
}

