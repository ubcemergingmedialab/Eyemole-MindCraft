using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class TriggerRaiseButton : MonoBehaviour {

	private InteractionButton interactionButton;
	private SimpleInteractionGlow simpleInteractionGlow;

	public Color yesRaiseColour;
	public Color noRaiseColour;

	// Use this for initialization
	void Start() {

		interactionButton = GetComponent<InteractionButton>();
		interactionButton.OnPress += TriggerRaise;

		simpleInteractionGlow = GetComponent<SimpleInteractionGlow>();

	}

	void TriggerRaise() {

		if (RaiseTerrainLeap.raiseEnabled) {

			simpleInteractionGlow.defaultColor = noRaiseColour;
			RaiseTerrainLeap.raiseEnabled = false;

		} else {

			simpleInteractionGlow.defaultColor = yesRaiseColour;
			RaiseTerrainLeap.raiseEnabled = true;
		}
	}
}
