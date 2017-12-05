using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnToCheckPoint : MonoBehaviour {
    public Serializer serializer;

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.layer == 8) {
            serializer.LoadCheckPoint();
        } 
    }
}
