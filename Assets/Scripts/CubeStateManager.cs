using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeStateManager : MonoBehaviour {

	public static UnityEvent stopRecording = new UnityEvent();

	public static bool isRecording = false;

	// Use this for initialization
	void Start () {

		isRecording = false;
		stopRecording = new UnityEvent ();

		
	}
	
	// Update is called once per frame
	void Update () {

		isRecording = Input.GetKey(KeyCode.A);

		if (Input.GetKeyUp (KeyCode.A)) {
			Debug.Log ("key released");
			stopRecording.Invoke ();
		}

		
	}

	public static void StartListening(UnityAction listener) {
		stopRecording.AddListener (listener);
	}
}
