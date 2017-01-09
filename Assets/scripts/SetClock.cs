using UnityEngine;
using System.Collections;

public class SetClock : MonoBehaviour {
    public GameObject clockPrefab;
	// Use this for initialization
	void Start () {
        GameObject clock = (GameObject)Instantiate(clockPrefab, new Vector2(10, -50), Quaternion.identity);
        clock.transform.SetParent(transform, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
