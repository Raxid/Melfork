using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunState : MonoBehaviour {
    [SerializeField] float rotationX = 1f;
    [SerializeField] int dayTimeInSeconds = 1200;
    // Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        float degressPerFrame = Time.deltaTime /60 * 360 * rotationX / dayTimeInSeconds;
               transform.RotateAround(transform.position,transform.right,degressPerFrame);
               
    
        }
}
