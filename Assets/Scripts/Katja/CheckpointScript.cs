using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {
    public Serializer serializer;
    public bool triggered = false;

    void SetCheckPoint() {
        triggered = true;
        serializer.SetCheckPoint();
    }
    public void LoadCheckPoint() {
        // 
    }
}
