using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStateManager : MonoBehaviour {

	public static double[] baselineEEG;

	// Use this for initialization
	void Start () {

		baselineEEG = new double[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SaveFunction() {
		
	}

	[Serializable]
	public class PersonalityCube {
		public double[] baselineEEG;
		public List<PersonalityCubeFace> faces;

	}

	[Serializable]
	public class PersonalityCubeFace{
		public float positionX, positionY, positionZ;
		public float rotationX, rotationY, rotationZ, rotationW;
		public Texture2D texture;
	}

}
