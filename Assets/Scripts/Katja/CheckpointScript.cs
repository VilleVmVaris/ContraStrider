using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {
    public Serializer serializer;
    public bool triggered = false;

    public void LoadCheckPoint() {
        // 
    }
    void OnTriggerEnter2D(Collider2D other) {
        triggered = true;
        serializer.SetCheckPoint();
    }
}
