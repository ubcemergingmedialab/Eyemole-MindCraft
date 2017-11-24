using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Accord.Audio;
using VRTK;

public class ChangeFaceColour : MonoBehaviour {

	private Texture2D texture;
	private Material material;
	public int numFreqs = 40;
	public int windowSize = 128;
	public int windowOverlap = 10;
	public int alphaCutoff = 8;
	public int betaCutoff = 13;
	private const int SAMPLE_RATE = 256;
	public enum EEG_CHANNEL: int {TP9=0, AF7=1, AF8=2, TP10=3};
	public EEG_CHANNEL channelToRecordFrom = EEG_CHANNEL.TP9;
	public enum COLOUR_SCHEME {BLUE_GREEN, YELLOW_RED, PURPLE_RED};
	private COLOUR_SCHEME currentScheme = COLOUR_SCHEME.BLUE_GREEN;
	public int maxTextureWidth = 50;
	private double[] dataBuffer;
	private double[] baselineArray;
	private double[] baselinePowerSpectrum;
	private VRTK_InteractableObject interactableObject;
	private int currentWindow;
	private int currentEEGCount;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;
		texture = new Texture2D (numFreqs, maxTextureWidth);
		material.SetTexture ("_MainTex", texture);
		dataBuffer = new double[windowSize];
		baselineArray = new double[windowSize];
		baselinePowerSpectrum = new double[windowSize];
		currentWindow = 0;
		currentEEGCount = 0;
		interactableObject = GetComponentInParent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectUngrabbed += new InteractableObjectEventHandler (StopRecordingData);
	}
	
	// Update is called once per frame
	void Update () {

		//Hold the object to record EEG data

		if (interactableObject.IsGrabbed()) {
			RecordData ();
		}

	}

	void ProcessBaselinePowerSpectrum() {
		
		ComplexSignal cs = Signal.FromArray (baselineArray, SAMPLE_RATE).ToComplex ();
		cs.ForwardFourierTransform ();
		baselinePowerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));
		EEGStateManager.baselineEEG [(int)channelToRecordFrom] = baselineArray.Average ();
	}

	void ProcessData() {

		double[] powerSpectrum;
		ComplexSignal cs = Signal.FromArray (dataBuffer, SAMPLE_RATE).ToComplex ();
		cs.ForwardFourierTransform ();
		powerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));

		//Convert to dB
		powerSpectrum = powerSpectrum.Select ((double arg, int index) => (10 * Math.Log10 (arg / baselinePowerSpectrum[index]))).ToArray ();

		// Normalzie values
		double minPowerSpectrum = (powerSpectrum.Min ());
		double maxPowerSpectrum = (powerSpectrum.Max ());
		powerSpectrum = powerSpectrum.Select ((double arg, int index) => (arg - minPowerSpectrum) / (maxPowerSpectrum - minPowerSpectrum)).ToArray ();

		//Pick the color scheme according to the peak frequency - Blue-Green if peak below alpha,
		//Yellow-Red otherwise 
		int highestFrequency = Array.IndexOf (powerSpectrum, powerSpectrum.Max ());
		currentScheme = highestFrequency >= alphaCutoff ? (highestFrequency >= betaCutoff ? COLOUR_SCHEME.YELLOW_RED : COLOUR_SCHEME.PURPLE_RED) : COLOUR_SCHEME.BLUE_GREEN;

		Color[] spectrogramColours;
			
		if (currentScheme == COLOUR_SCHEME.BLUE_GREEN) {
			spectrogramColours = powerSpectrum.Select (((double arg) => ColourUtility.HSL2BlueGreen (arg, 0.75, 0.5))).ToArray ();
		} else if (currentScheme == COLOUR_SCHEME.PURPLE_RED) {
			spectrogramColours = powerSpectrum.Select (((double arg) => ColourUtility.HSL2PurpleRed (arg, 0.75, 0.5))).ToArray ();
		} else {
			spectrogramColours = powerSpectrum.Select (((double arg) => ColourUtility.HSL2YellowRed (arg, 0.75, 0.5))).ToArray ();
						
		}
		//Set the texture 

		for (int i = 0; i < numFreqs; i++) {
			texture.SetPixel (i, (currentWindow % maxTextureWidth), spectrogramColours [i]);
		}

		currentWindow++;
		texture.Apply();
		
	}

	void StopRecording() {

		dataBuffer = new double[windowSize];
		currentEEGCount = 0;

	}

	void StopRecordingData(object sender, InteractableObjectEventArgs e) {
		StopRecording ();
	}



	void RecordData() {

		if (currentEEGCount > windowSize) {
			for (int i = windowSize - 2; i > 0; i--) {
				dataBuffer [i] = dataBuffer [i + 1];
			}
			dataBuffer [windowSize - 1] = EEGData.eegData [(int)channelToRecordFrom];
		} else {
			dataBuffer [currentEEGCount % windowSize] = EEGData.eegData [(int)channelToRecordFrom];
		}

		if (currentEEGCount < windowSize) {
			baselineArray[currentEEGCount] = EEGData.eegData[(int)channelToRecordFrom];
		}
		if (currentEEGCount == windowSize) {
			ProcessBaselinePowerSpectrum ();
		}

		if ((currentEEGCount > windowSize) && ((currentEEGCount - windowSize) % 10 == 0)) {
			ProcessData ();
		}

		currentEEGCount++;

	}
}
