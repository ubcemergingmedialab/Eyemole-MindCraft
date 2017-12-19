using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using Leap.Unity.Interaction;

public class ChangeSceneOnGrab : MonoBehaviour {

	public int sceneToChangeTo = 1;
	private VRTK_InteractableObject interactableObject;
	private InteractionBehaviour intObject;

	// Use this for initialization
	void Start () {
		interactableObject = GetComponentInParent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler (ChangeScene);

		intObject = GetComponentInParent<InteractionBehaviour>();
		intObject.OnContactBegin += ChangeScene;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeScene() {
		SceneManager.LoadScene(sceneToChangeTo);
	}

	void ChangeScene(object sender, InteractableObjectEventArgs e){
		SceneManager.LoadScene (sceneToChangeTo);
	}
}
