using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_TESTING: MonoBehaviour {
    float x;
    float y;

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
            x = transform.position.x;
            y = transform.position.y;
            transform.position = new Vector2(x + 1, y);
            x = transform.position.x;
        }
	}
}
