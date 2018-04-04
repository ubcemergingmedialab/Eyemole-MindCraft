using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CreateCube : MonoBehaviour {
   
    public Rigidbody Prefab;
    private VRTK_ControllerEvents evts;
    private AudioSource spawnSound;
    private bool canSpawn;

    void Start() {
        canSpawn = true;
        spawnSound = GetComponent<AudioSource>();
        evts = GetComponentInParent<VRTK_ControllerEvents>();
        evts.GripClicked += new ControllerInteractionEventHandler(InstantiatePrefab);
    }

    void InstantiatePrefab(object sender, ControllerInteractionEventArgs e) {
        Rigidbody RigidPrefab;
        RigidPrefab = Instantiate(Prefab, gameObject.transform.parent.transform.position, gameObject.transform.parent.transform.rotation) as Rigidbody;
        spawnSound.Play();
    }

}
