using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class ChangeSceneOnGrab : MonoBehaviour {

	public int sceneToChangeTo = 1;
	private VRTK_InteractableObject interactableObject;

	// Use this for initialization
	void Start () {
		interactableObject = GetComponentInParent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler (ChangeScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeScene(object sender, InteractableObjectEventArgs e){
		SceneManager.LoadScene (sceneToChangeTo);
	}
}
