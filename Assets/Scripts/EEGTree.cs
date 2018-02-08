using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EEGTree : MonoBehaviour {


	public enum Concentration { Left, Right, None }; // Whether the user has higher concentration on left side, right side, or is not concentrated
	public float threshold;
	public GameObject tree;
	private TreeObject currentTreeObject;
	private TreeObject treeRoot;

	private VRTK_ControllerEvents controllerEvents;
	private VRTK_StraightPointerRenderer pointerRenderer;

	private EEGData.EEG_CHANNEL[] allChan = new EEGData.EEG_CHANNEL[4] { EEGData.EEG_CHANNEL.AF7, EEGData.EEG_CHANNEL.TP9, EEGData.EEG_CHANNEL.TP10, EEGData.EEG_CHANNEL.AF8};
	private EEGData.EEG_CHANNEL[] chanLeft = new EEGData.EEG_CHANNEL[2] { EEGData.EEG_CHANNEL.AF7, EEGData.EEG_CHANNEL.TP9};
	private EEGData.EEG_CHANNEL[] chanRight = new EEGData.EEG_CHANNEL[2] {EEGData.EEG_CHANNEL.TP10, EEGData.EEG_CHANNEL.AF8 };

	private float updateRate = 0.1f;
	private float timeLastFrame = 0f;

	// Use this for initialization
	void Start () {
		
		controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();
		controllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(InstantiateTree);
		controllerEvents.TouchpadReleased += new ControllerInteractionEventHandler(RemoveTreeObject);
		pointerRenderer = GetComponentInParent<VRTK_StraightPointerRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - timeLastFrame > updateRate) {
			if (controllerEvents.touchpadPressed) { // EEGData.IsEEGConnected()) {

				DrawTree();
			}
			timeLastFrame = Time.time;
		}

	}

	/// <summary>
	/// Creates the tree trunk at the location the controller is pointing at.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void InstantiateTree(object sender, ControllerInteractionEventArgs e) {

		RaycastHit hit = pointerRenderer.GetDestinationHit();
		Vector3 point = hit.point;
		currentTreeObject = new TreeObject(tree, null, point, 0f);
	}
	
	/// <summary>
	/// Removes the reference to the tree once you stop pressing the touchpad
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void RemoveTreeObject(object sender, ControllerInteractionEventArgs e) {
		
		treeRoot = null;
	}

	/// <summary>
	/// Add or remove tree segments to the current tree object based on the user's concentration.
	/// </summary>
	private void DrawTree() {

		Concentration concentration = GetConcentration();
		Debug.Log(concentration);
		switch (concentration) {

			case Concentration.None:

				if (IsMoreConcentratedLeft()) {
					currentTreeObject.RemoveBranch(currentTreeObject.FindBranchRight());
				} else currentTreeObject.RemoveBranch(currentTreeObject.FindBranchLeft());
				break;

			case Concentration.Left:

				currentTreeObject.AddBranchLeft(currentTreeObject.GetRoot().FindFirstBranchWithoutLeft(), tree);
				break;

			case Concentration.Right:

				currentTreeObject.AddBranchRight(currentTreeObject.GetRoot().FindFirstBranchWithoutRight(), tree);
				break;

		}
	}

	private Concentration GetConcentration() {

		return IsConcentrated() ? (IsMoreConcentratedLeft() ? Concentration.Left : Concentration.Right) : Concentration.None;
	}

	/// <remarks> 
	/// This should call on EEGData to get the user's concentration. 
	/// Change this and IsMoreConcentratedLeft if you change the assessement of concentration.
	/// </remarks>
	private bool IsConcentrated() {

		return controllerEvents.AnyButtonPressed();

		//return EEGData.GetConcentration(allChan) < threshold;
	}

	private bool IsMoreConcentratedLeft() {

		return controllerEvents.buttonOnePressed;
		//return EEGData.GetConcentration(chanLeft) - EEGData.GetConcentration(chanRight) < 0;
	}
	
}
