﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEGStateManager : MonoBehaviour {

	public static double[] baselineEEG;

	// Use this for initialization
	void Start () {

		baselineEEG = new double[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
