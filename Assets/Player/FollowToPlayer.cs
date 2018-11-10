using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToPlayer : MonoBehaviour {

    GameObject player;
    GameObject camera;
	// Use this for initialization
	void Start () {
      player = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position;
	}
}
