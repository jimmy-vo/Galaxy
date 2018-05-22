using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour {

    public GameObject ballPreFab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Tap"))
        {
            //ballPreFab = (GameObject)Instantiate(Resources.Load("Prefab_Sphere"));
            Instantiate(ballPreFab);
            ballPreFab.
            Debug.Log("Fired");
        }
    }
}
