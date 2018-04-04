using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CreateObject : MonoBehaviour {

	public Transform Spawnpoint;
	public Rigidbody Prefab;
	private VRTK_InteractableObject interactableObject;
	private bool canSpawn;
    private float timeLastSpawned = 0f;
    private float timeBetweenSpawns = 3f;

	void Start() {
		canSpawn = true;
		interactableObject = GetComponent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectTouched+= new InteractableObjectEventHandler (InstantiatePrefab);
	}

    private void Update() {
        if (Time.time - timeLastSpawned >= timeBetweenSpawns) {
            canSpawn = true;
            timeLastSpawned = Time.time;
        }
    }

    void InstantiatePrefab (object sender, InteractableObjectEventArgs e) {
		if (canSpawn) {
			Rigidbody RigidPrefab;
			RigidPrefab = Instantiate (Prefab, Spawnpoint.position, Spawnpoint.rotation) as Rigidbody;
			canSpawn = false;
		}
	}

	

}
