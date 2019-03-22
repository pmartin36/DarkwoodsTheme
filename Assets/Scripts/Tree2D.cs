using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree2D : MonoBehaviour
{
    void Start() {
		
    }

    void Update() {
        this.transform.LookAt(Camera.main.transform);
		this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
    }
}
