using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

	public Transform Spawnpoint;
	public Rigidbody Prefab;

	void OnTriggerEnter () {
		Rigidbody RigidPrefab;
		RigidPrefab = Instantiate (Prefab, Spawnpoint.position, Spawnpoint.rotation) as Rigidbody;
	}
	

}
