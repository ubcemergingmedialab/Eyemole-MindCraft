using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class EEGData : MonoBehaviour {

	public OSC osc;

	public bool useEEG = true;
	public bool useAlpha = true;
	public bool useBeta = true;
	public bool useDelta = true;
	public bool useTheta = true;
	public bool useGamma = true;
	public bool useAcc = true;
	public bool useBlink = true;

	public string museName = "/muse";

	public static float[][] eegData;
	public static float[] accData;

	public enum EEG_BANDS : int { DELTA = 0, THETA = 1, ALPHA = 2, BETA = 3, GAMMA = 4 };

	public enum EEG_CHANNEL : int { TP9 = 0, AF7 = 1, AF8 = 2, TP10 = 3 };

	public static float[][][] freqData;

	public static int bufferSize = 256;
	public static int currBufferPositionEEG = 0;
	public static int[] currBufferPositionFreq;

	public static float maxTimeBetweenUpdates = 3000f;
	public static float lastTimeUpdated = 0f;

	// Script initialization
	void Start() {

		UnityEngine.Debug.Log("Started");

		osc = GetComponent<OSC>();

		eegData = new float[bufferSize][];
		accData = new float[3];
		freqData = new float[5][][];

		// Position in the buffer for each frequency band
		currBufferPositionFreq = new int[] { 0, 0, 0, 0, 0 };

		int numChannels = Enum.GetNames(typeof(EEG_CHANNEL)).Length;

		// Initialize the buffer for EEG data 

		for (int i = 0; i < bufferSize; i++) {
			eegData[i] = new float[numChannels];
		}

		// Initialize the buffer for frequency data

		foreach (EEG_BANDS eegBand in Enum.GetValues(typeof(EEG_BANDS))) {
			freqData[(int)eegBand] = new float[bufferSize][];
			for (int i = 0; i < bufferSize; i++) {
				freqData[(int)eegBand][i] = new float[numChannels];
			}
		}

		if (useEEG) osc.SetAddressHandler(museName + "/eeg", OnReceiveEEG);
		if (useAlpha) osc.SetAddressHandler(museName + "/elements/alpha_absolute", OnReceiveAlpha);
		if (useBeta) osc.SetAddressHandler(museName + "/elements/beta_absolute", OnReceiveBeta);
		if (useTheta) osc.SetAddressHandler(museName + "/elements/theta_absolute", OnReceiveTheta);
		if (useGamma) osc.SetAddressHandler(museName + "/elements/gamma_absolute", OnReceiveGamma);
		if (useDelta) osc.SetAddressHandler(museName + "/elements/delta_absolute", OnReceiveDelta);
		if (useAcc) osc.SetAddressHandler(museName + "/acc", OnReceiveAcc);
	}

	// NOTE: The received messages at each server are updated here
	void Update() {

	}

	void OnReceiveEEG(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			eegData[currBufferPositionEEG % bufferSize][i] = message.GetFloat(i);
		}
		currBufferPositionEEG++;

		lastTimeUpdated = Time.time;
	}

	void OnReceiveAlpha(OscMessage message) {

		int currPos = currBufferPositionFreq[(int)EEG_BANDS.ALPHA];
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.ALPHA][currPos % bufferSize][i] = message.GetFloat(i);
		}

		currBufferPositionFreq[(int)EEG_BANDS.ALPHA]++;
		lastTimeUpdated = Time.time;
	}

	void OnReceiveBeta(OscMessage message) {

		int currPos = currBufferPositionFreq[(int)EEG_BANDS.BETA];
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.BETA][currPos % bufferSize][i] = message.GetFloat(i);
		}

		currBufferPositionFreq[(int)EEG_BANDS.BETA]++;
		lastTimeUpdated = Time.time;
	}

	void OnReceiveGamma(OscMessage message) {

		int currPos = currBufferPositionFreq[(int)EEG_BANDS.GAMMA];
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.GAMMA][currPos % bufferSize][i] = message.GetFloat(i);
		}
		currBufferPositionFreq[(int)EEG_BANDS.GAMMA]++;
		lastTimeUpdated = Time.time;
	}

	void OnReceiveDelta(OscMessage message) {

		int currPos = currBufferPositionFreq[(int)EEG_BANDS.DELTA];
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.DELTA][currPos % bufferSize][i] = message.GetFloat(i);
		}
		currBufferPositionFreq[(int)EEG_BANDS.DELTA]++;
		lastTimeUpdated = Time.time;
	}

	void OnReceiveTheta(OscMessage message) {

		int currPos = currBufferPositionFreq[(int)EEG_BANDS.THETA];
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.THETA][currPos % bufferSize][i] = message.GetFloat(i);
		}
		currBufferPositionFreq[(int)EEG_BANDS.THETA]++;
		lastTimeUpdated = Time.time;
	}

	void OnReceiveAcc(OscMessage message) {
		for (int i = 0; i < 3; i++) {
			accData[i] = message.GetFloat(i);
		}
		lastTimeUpdated = Time.time;
	}

	/// <summary>
	/// Gets the raw EEG data - at the most recent update if samplesBefore is not specified,
	/// or the samplesBefore'th most recent update.
	/// </summary>
	/// <param name="samplesBefore"></param>
	/// <returns></returns>
	public static float[] GetEEGData(int samplesBefore = 0) {

		return EEGData.eegData[(EEGData.currBufferPositionEEG - samplesBefore) % EEGData.bufferSize];
	}

	/// <summary>
	/// Gets the absolute power of the specified frequency band. If samplesBefore is not specified,
	/// return the most recently updated value, otherwise return the samplesBefore'th most recent value.
	/// </summary>
	/// <param name="freqBand"></param>
	/// <param name="samplesBefore"></param>
	/// <returns></returns>

	public static float[] GetAbsoluteFrequency(EEG_BANDS freqBand, int samplesBefore = 0) {

		int currPos = currBufferPositionFreq[(int)freqBand];
        Debug.Log(currPos);
		return EEGData.freqData[(int)freqBand][(Mathf.Max(currPos - 1, 0) - samplesBefore) % EEGData.bufferSize];
	}

	public static float GetAverage(EEG_BANDS freqBand, int samplesBefore = 0) {
		return EEGData.GetAbsoluteFrequency(freqBand, samplesBefore).Average();
	}

	public static float GetRelativeFrequency(EEG_BANDS freqBand, int samplesBefore = 0) {

		float sumBands = 0f;

		foreach (EEG_BANDS eegBand in Enum.GetValues(typeof(EEG_BANDS))) {
			sumBands += Mathf.Abs(EEGData.GetAverage(eegBand, samplesBefore));
		}

		float relFreq = 0f;
		if (sumBands > 0) relFreq = Mathf.Abs(EEGData.GetAverage(freqBand, samplesBefore) / sumBands);

		return relFreq;
	}


	/// <summary>
	/// Define concentration as the difference between avg alpha over the set of channels
	/// between the current sample and the last sample. Low values indicate high stability 
	/// = high concentration.
	/// </summary>
	/// <param name="chans"> Channels that will be included in calculating
	/// the concentration.</param>
	/// <returns> The concentration value over the set of channels.</returns>

	public static float GetConcentration(EEG_CHANNEL[] chans) {

		float[] alphaData = GetAbsoluteFrequency(EEG_BANDS.ALPHA);
		float[] oldAlphaData = GetAbsoluteFrequency(EEG_BANDS.ALPHA, 1);

		float[] data = chans.Select(x => alphaData[(int)x]).ToArray();
		float[] oldData = chans.Select(x => oldAlphaData[(int)x]).ToArray();

		Debug.Log(Mathf.Abs(data.Average() - oldData.Average()));
		return Mathf.Abs(data.Average() - oldData.Average());

	}

	public static bool IsEEGConnected() {

		return (Time.time - lastTimeUpdated) < maxTimeBetweenUpdates;
	}


}