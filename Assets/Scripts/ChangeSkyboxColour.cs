using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxColour : MonoBehaviour {

	private Color DAY_COLOUR = new Color(244 / 255, 255 /255, 244 / 255);
	private Color NIGHT_COLOUR = new Color(0.015f, 0.00549f, 0.12156f);
	private float alphaThreshold = 0.05f;
	private float currentColour = 0.5f;
	private float stepSize = 0.001f;

	// Use this for initialization
	void Start () {
		
		RenderSettings.skybox.SetColor("_Tint", Color.Lerp(NIGHT_COLOUR, DAY_COLOUR, currentColour));
		DynamicGI.UpdateEnvironment();
	}
	
	// Update is called once per frame
	void Update () {

		float relAlpha = 0f;
		try {
			relAlpha = EEGData.GetRelativeFrequency(EEGData.EEG_BANDS.ALPHA);
			if (relAlpha >= alphaThreshold && currentColour < 1.0f) {
				currentColour += stepSize;
			} else if (currentColour > 0f) {
				currentColour -= stepSize;
			}
		} catch {
		}
		RenderSettings.skybox.SetColor("_Tint", Color.Lerp(NIGHT_COLOUR, DAY_COLOUR, currentColour));
		DynamicGI.UpdateEnvironment();
	}
}
