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

	public static float[] eegData;
	public static float[] accData;

	public enum EEG_BANDS: int {DELTA=0, THETA=1, ALPHA=2, BETA=3, GAMMA=4};

	public static float[][] freqData;
	
	// Script initialization
	void Start() {	
		
		UnityEngine.Debug.Log("Started");

		osc = GetComponent<OSC>();

		eegData = new float[4];
		accData = new float[3];
		freqData = new float[5][];
		foreach (EEG_BANDS eegBand in Enum.GetValues(typeof(EEG_BANDS))) {
			freqData[(int)eegBand] = new float[4];
		}

		if (useEEG) osc.SetAddressHandler( museName + "/eeg" , OnReceiveEEG );
		if (useAlpha) osc.SetAddressHandler( museName + "/elements/alpha_absolute" , OnReceiveAlpha);
		if (useBeta) osc.SetAddressHandler( museName + "/elements/beta_absolute" , OnReceiveBeta);
		if (useTheta) osc.SetAddressHandler( museName + "/elements/theta_absolute" , OnReceiveTheta);
		if (useGamma) osc.SetAddressHandler( museName + "/elements/gamma_absolute" , OnReceiveGamma);
		if (useDelta) osc.SetAddressHandler( museName + "/elements/delta_absolute" , OnReceiveDelta);
		if (useAcc) osc.SetAddressHandler( museName + "/acc" , OnReceiveAcc);
	}

	// NOTE: The received messages at each server are updated here
	void Update() {

	}

	void OnReceiveEEG(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			eegData[i] = message.GetFloat(i);
		}
	}

	void OnReceiveAlpha(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			freqData[(int) EEG_BANDS.ALPHA][i] = message.GetFloat(i);
		}
	}

    void OnReceiveBeta(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.BETA][i] = message.GetFloat(i);
		}
	}

	void OnReceiveGamma(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.GAMMA][i] = message.GetFloat(i);
		}
	}

	void OnReceiveDelta(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.DELTA][i] = message.GetFloat(i);
		}
	}

	void OnReceiveTheta(OscMessage message) {
		for (int i = 0; i < 4; i++) {
			freqData[(int)EEG_BANDS.THETA][i] = message.GetFloat(i);
		}
	}

	void OnReceiveAcc(OscMessage message) {
		for (int i = 0; i < 3; i++) {
			accData[i] = message.GetFloat(i);
		}
	}

	public static float[] GetAbsoluteFrequency(EEG_BANDS freqBand) {
		return EEGData.freqData[(int)freqBand];
	}

	public static float GetAverage(EEG_BANDS freqBand) {
		return EEGData.freqData[(int)freqBand].Average();
	}

	public static float GetRelativeFrequency(EEG_BANDS freqBand) {

		float sumBands = 0f;

		foreach (EEG_BANDS eegBand in Enum.GetValues(typeof(EEG_BANDS))) {
			sumBands += EEGData.GetAverage(eegBand);
		}

		float relFreq = EEGData.GetAverage(freqBand) / sumBands;

		Debug.Log(relFreq.ToString());

		return relFreq;
	}

}
