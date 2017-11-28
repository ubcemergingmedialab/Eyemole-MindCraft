using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CreateObject : MonoBehaviour {

	public Transform Spawnpoint;
	public Rigidbody Prefab;
	private VRTK_InteractableObject interactableObject;
	private bool canSpawn;

	void Start() {
		canSpawn = true;
		interactableObject = GetComponent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler (InstantiatePrefab);
		interactableObject.InteractableObjectUngrabbed += new InteractableObjectEventHandler (ReenableSpawning);
	}

	void InstantiatePrefab (object sender, InteractableObjectEventArgs e) {
		if (canSpawn) {
			Rigidbody RigidPrefab;
			RigidPrefab = Instantiate (Prefab, Spawnpoint.position, Spawnpoint.rotation) as Rigidbody;
			canSpawn = false;
		}
	}

	void ReenableSpawning (object sender, InteractableObjectEventArgs e) {
		canSpawn = true;
	}
	

}
