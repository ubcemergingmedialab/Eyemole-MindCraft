using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class TriggerTeleportButton : MonoBehaviour {

	private InteractionButton interactionButton;
	private SimpleInteractionGlow simpleInteractionGlow;

	public Color yesTeleportColour;
	public Color noTeleportColour;

	// Use this for initialization
	void Start () {

		interactionButton = GetComponent<InteractionButton> ();
		interactionButton.OnPress += TriggerTeleport;

		simpleInteractionGlow = GetComponent<SimpleInteractionGlow> ();

	}

	void TriggerTeleport() {

		if (LeapTeleporter.teleportEnabled) {

			simpleInteractionGlow.defaultColor = noTeleportColour;
			LeapTeleporter.teleportEnabled = false;

		} 
		else {

			simpleInteractionGlow.defaultColor = yesTeleportColour;
			LeapTeleporter.teleportEnabled = true;
		}
	}

}
