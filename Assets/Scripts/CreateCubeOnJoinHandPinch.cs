using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class CreateCubeOnJoinHandPinch : MonoBehaviour {

	public PinchDetector leftHandPinchDetector;
	public PinchDetector rightHandPinchDetector;

	public LeapServiceProvider controller;

	public float triggerDistance = 0.05f;
	public float rechargeTime = 3f; //force some wait time between generated cubes 

	private float timeLastCreated; //the time when a cube was created last 

	public Rigidbody prefab;

	// Use this for initialization
	void Start () {

		timeLastCreated = 0f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (leftHandPinchDetector.IsPinching && rightHandPinchDetector.IsPinching && AreHandsCloseTogether() && IsReadyToCreate()) {

			Rigidbody RigidPrefab;
			RigidPrefab = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity) as Rigidbody;
			timeLastCreated = Time.time;
		}
	}

	private bool AreHandsCloseTogether() {
		return GetHandDistance() <= triggerDistance;
	}

	private bool IsReadyToCreate() {
		float currentTime = Time.time;
		return (currentTime - timeLastCreated) > rechargeTime;
	}

	private float GetHandDistance() {

		List<Hand> hands = controller.CurrentFrame.Hands;
		if (hands.Count == 2) {
			Hand leftHand = hands[0];
			Hand rightHand = hands[1];
			return Vector3.Distance(leftHand.GetPinchPosition(), rightHand.GetPinchPosition());

		} else {
			return 9999f;
		}

	}

	private Vector3 GetSpawnPosition() {

		List<Hand> hands = controller.CurrentFrame.Hands;
		if (hands.Count == 2) {
			Hand leftHand = hands[0];
			Hand rightHand = hands[1];
			return Vector3.Lerp(leftHand.GetPinchPosition(), rightHand.GetPinchPosition(), 0.5f);

		} else {
			return Vector3.zero;
		}
	}
}
