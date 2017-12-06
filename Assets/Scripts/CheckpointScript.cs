using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {
    public Serializer serializer;
    public bool triggered = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (!triggered) {
            triggered = true;
            print(gameObject.name);
            serializer.SetCheckPoint();
        }
    }
}
