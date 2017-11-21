using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Accord.Audio;

public class ChangeFaceColour : MonoBehaviour {

	private Texture2D texture;
	private Material material;
	public int recordingTime = 5;
	public int numFreqs = 60;
	public int windowSize = 128;
	public int windowOverlap = 10;
	private const int SAMPLE_RATE = 256;
	public enum EEG_CHANNEL: int {TP9=0, AF7=1, AF8=2, TP10=3};
	public EEG_CHANNEL channelToRecordFrom = EEG_CHANNEL.TP9;
	private ArrayList dataBuffer;
	private int dataBufferLength;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;
		texture = null;
		dataBuffer = new ArrayList();
		dataBufferLength = 0; 
	}
	
	// Update is called once per frame
	void Update () {

		//Hold A to record EEG data
		if (Input.GetKey(KeyCode.A)) {
			RecordData ();
		}

		//Release A to convert EEG data to cube texture 
		if (Input.GetKeyUp (KeyCode.A)) {
			StopRecording ();
		}
	}

	void StopRecording() {


		//Need to convert from ArrayList to double[] to use the Accord library 
		double[] fullConvertedBuffer = dataBuffer.ToArray().Select((object arg) => double.Parse(arg.ToString())).ToArray ();

		//Data array length must be a power of 2 in order to convert to a Signal
		int maxLength = (int)Math.Pow(2, (Math.Floor (Math.Log (fullConvertedBuffer.Length, 2.0))));

		//Need to have enough data to be able to get the full power spectrum 
		if (maxLength >= windowSize) {


			double[] convertedBuffer = new double[maxLength];
			Array.ConstrainedCopy (fullConvertedBuffer, 0, convertedBuffer, 0, maxLength);

			ComplexSignal cs = Signal.FromArray (convertedBuffer, SAMPLE_RATE).ToComplex ();

			//This array will hold the power spectrum values which will later be converted to colours 
			double[] spectrogramMatrix = new double[(int) convertedBuffer.Length / windowOverlap * numFreqs];

			int windowCount = 0;
			for (int i = 0; i < convertedBuffer.Length - windowSize; i += windowOverlap) {

				//New array to hold just the data points for this time window 
				double[] timeWindowArray = new double[windowSize];
				Array.ConstrainedCopy (convertedBuffer, i, timeWindowArray, 0, windowSize);


				cs = Signal.FromArray (timeWindowArray, SAMPLE_RATE).ToComplex ();
				cs.ForwardFourierTransform ();
				double[] powerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));

				//Normalize the values 
				double minPowerSpectrum = (powerSpectrum.Min ());
				double maxPowerSpectrum = (powerSpectrum.Max ());
				powerSpectrum = powerSpectrum.Select ((double arg) => (arg - minPowerSpectrum) / (maxPowerSpectrum - minPowerSpectrum)).ToArray ();

				Array.ConstrainedCopy (powerSpectrum, 0, spectrogramMatrix, windowCount * numFreqs, numFreqs);
				windowCount++;
			}

			Color[] spectrogramColours = spectrogramMatrix.Select (((double arg) => ColourUtility.HSL2RGB(arg, 0.75, 0.5))).ToArray ();

			//Set the texture 
			texture = new Texture2D ((int)convertedBuffer.Length / windowSize, numFreqs);
			texture.SetPixels (spectrogramColours);
			material.SetTexture ("_MainTex", texture);
		}


	}



	void RecordData() {

		dataBuffer.Add (EEGData.eegData[(int)channelToRecordFrom]);

	}
}
