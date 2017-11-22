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
	public int recordingTime = 5;
	public int numFreqs = 40;
	public int windowSize = 128;
	public int windowOverlap = 10;
	public int alphaCutoff = 8;
	private const int SAMPLE_RATE = 256;
	public enum EEG_CHANNEL: int {TP9=0, AF7=1, AF8=2, TP10=3};
	public EEG_CHANNEL channelToRecordFrom = EEG_CHANNEL.TP9;
	public enum COLOUR_SCHEME {BLUE_GREEN, YELLOW_RED};
	private COLOUR_SCHEME currentScheme = COLOUR_SCHEME.BLUE_GREEN;
	private ArrayList dataBuffer;
	private ArrayList baselineArray;
	private double[] baselinePowerSpectrum;
	private VRTK_InteractableObject interactableObject;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;
		texture = null;
		dataBuffer = new ArrayList();
		baselineArray = new ArrayList ();
		baselinePowerSpectrum = new double[windowSize];
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
		
		double[] fullConvertedBuffer = baselineArray.ToArray().Select((object arg) => double.Parse(arg.ToString())).ToArray ();
		ComplexSignal cs = Signal.FromArray (fullConvertedBuffer, SAMPLE_RATE).ToComplex ();
		cs.ForwardFourierTransform ();
		baselinePowerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));
	}

	void ProcessData() {

		double[] convertedBuffer = dataBuffer.ToArray().Select((object arg) => double.Parse(arg.ToString())).ToArray ();

		//Data array length must be a power of 2 in order to convert to a Signal
		//int maxLength = (int)Math.Pow(2, (Math.Floor (Math.Log (fullConvertedBuffer.Length, 2.0))));

		//double[] convertedBuffer = new double[maxLength];
		//Array.ConstrainedCopy (fullConvertedBuffer, 0, convertedBuffer, 0, maxLength);

		//This array will hold the power spectrum values which will later be converted to colours 
		double[] spectrogramMatrix = new double[(int) convertedBuffer.Length / windowOverlap * numFreqs];

		int windowCount = 0;
		for (int i = 0; i < convertedBuffer.Length - windowSize; i += windowOverlap) {

			//New array to hold just the data points for this time window 
			double[] timeWindowArray = new double[windowSize];
			Array.ConstrainedCopy (convertedBuffer, i, timeWindowArray, 0, windowSize);


			ComplexSignal cs = Signal.FromArray (timeWindowArray, SAMPLE_RATE).ToComplex ();
			cs.ForwardFourierTransform ();
			double[] powerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));

			//Convert to dB
			powerSpectrum = powerSpectrum.Select ((double arg, int index) => (10 * Math.Log10 (arg / baselinePowerSpectrum[index]))).ToArray ();

			// Normalzie values
			double minPowerSpectrum = (powerSpectrum.Min ());
			double maxPowerSpectrum = (powerSpectrum.Max ());
			powerSpectrum = powerSpectrum.Select ((double arg, int index) => (arg - minPowerSpectrum) / (maxPowerSpectrum - minPowerSpectrum)).ToArray ();


			int highestFrequency = Array.IndexOf (powerSpectrum, powerSpectrum.Max ());
			Debug.Log (highestFrequency.ToString ());
			currentScheme = highestFrequency >= alphaCutoff ? COLOUR_SCHEME.YELLOW_RED : COLOUR_SCHEME.BLUE_GREEN;
			Array.ConstrainedCopy (powerSpectrum, 0, spectrogramMatrix, windowCount * numFreqs, numFreqs);
			windowCount++;
		}

		Color[] spectrogramColours;
			
		if (currentScheme == COLOUR_SCHEME.BLUE_GREEN) {
			spectrogramColours = spectrogramMatrix.Select (((double arg) => ColourUtility.HSL2BlueGreen(arg, 0.75, 0.5))).ToArray ();
		} else {
			spectrogramColours = spectrogramMatrix.Select (((double arg) => ColourUtility.HSL2YellowRed (arg, 0.75, 0.5))).ToArray ();

		}
		//Set the texture 
		texture = new Texture2D ((int)convertedBuffer.Length / windowSize, numFreqs);
		texture.SetPixels (spectrogramColours);
		//material.mainTexture = null;
		material.SetTexture ("_MainTex", texture);


		
	}

	void StopRecording() {

         dataBuffer = new ArrayList();

	}

	void StopRecordingData(object sender, InteractableObjectEventArgs e) {
		StopRecording ();
	}



	void RecordData() {

        dataBuffer.Add (EEGData.eegData[(int)channelToRecordFrom]);

		if (baselineArray.Count < windowSize) {
			baselineArray.Add (EEGData.eegData[(int)channelToRecordFrom]);
		}
		if (baselineArray.Count == windowSize) {
			ProcessBaselinePowerSpectrum ();
		}

		if (dataBuffer.Count > windowSize) {
			ProcessData ();
		}

	}
}
