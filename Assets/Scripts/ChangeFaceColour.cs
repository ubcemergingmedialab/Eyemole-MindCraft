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
	private const int SAMPLE_RATE = 256;
	public enum EEG_CHANNEL: int {TP9=0, AF7=1, AF8=2, TP10=3};
	public EEG_CHANNEL channelToRecordFrom = EEG_CHANNEL.TP9;
	private ArrayList dataBuffer;
	private int dataBufferLength;
	public UnityAction eventListener;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;
		texture = null;
		dataBuffer = new ArrayList();
		dataBufferLength = 0; 
		eventListener = new UnityAction (StopRecording);
		CubeStateManager.StartListening (eventListener);
		//texture = new Texture2D (numFreqs, (int) recordingTime / windowSize);
	}
	
	// Update is called once per frame
	void Update () {

		if (CubeStateManager.isRecording == true) {
			RecordData ();
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			StopRecording ();
		}
	}

	void StopRecording() {

		Debug.Log (dataBuffer.Count.ToString ());
		double[] fullConvertedBuffer = dataBuffer.ToArray().Select((object arg) => double.Parse(arg.ToString())).ToArray ();
		int maxLength = (int)Math.Pow(2, (Math.Floor (Math.Log (fullConvertedBuffer.Length, 2.0))));
		if (maxLength >= windowSize) {
			double[] convertedBuffer = new double[maxLength];
			Array.ConstrainedCopy (fullConvertedBuffer, 0, convertedBuffer, 0, maxLength);
			ComplexSignal cs = Signal.FromArray (convertedBuffer, SAMPLE_RATE).ToComplex ();
			double[] spectrogramMatrix = new double[(int)convertedBuffer.Length / windowSize * numFreqs];
			int windowCount = 0;
			for (int i = 0; i < convertedBuffer.Length; i += windowSize) {
				double[] timeWindowArray = new double[windowSize];
				Array.ConstrainedCopy (convertedBuffer, i, timeWindowArray, 0, windowSize);
				cs = Signal.FromArray (timeWindowArray, SAMPLE_RATE).ToComplex ();
				cs.ForwardFourierTransform ();
				double[] powerSpectrum = Tools.GetPowerSpectrum (cs.GetChannel (0));
				double minPowerSpectrum = (powerSpectrum.Min ());
				double maxPowerSpectrum = (powerSpectrum.Max ());
				powerSpectrum = powerSpectrum.Select ((double arg) => (arg - minPowerSpectrum) / (maxPowerSpectrum - minPowerSpectrum)).ToArray ();
				Array.ConstrainedCopy (powerSpectrum, 0, spectrogramMatrix, windowCount * numFreqs, numFreqs);
				windowCount++;
			}
			Color[] spectrogramColours = spectrogramMatrix.Select (((double arg) => new Color ((float)arg, (float)(1 - arg), (float)(1 - arg)))).ToArray ();
			texture = new Texture2D ((int)convertedBuffer.Length / windowSize, numFreqs);
			texture.SetPixels (spectrogramColours);
			material.SetTexture ("_MainTex", texture);
		}


	}

	void RecordData() {
		double data = EEGData.eegData [(int)channelToRecordFrom];

		dataBuffer.Add (EEGData.eegData[(int)channelToRecordFrom]);

	}
}
