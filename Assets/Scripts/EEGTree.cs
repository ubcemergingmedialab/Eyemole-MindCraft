using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEGTree : MonoBehaviour {


	public enum Concentration {Left, Right, None};
	public float threshold;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Concentration GetConcentration() {

		return IsConcentrated() ? (IsMoreConcentratedLeft() ? Concentration.Left : Concentration.Right) : Concentration.None;
	}

	/// <summary>
	///  
	/// </summary>
	/// <remarks> 
	/// This should call on EEGData to get the user's concentration. 
	/// Change this and IsMoreConcentratedLeft if you change the assessement of concentration.
	/// </remarks>
	private bool IsConcentrated() {

		return EEGData.GetConcentration() > threshold;
	}

	private bool IsMoreConcentratedLeft() {

		return true;
	}
	
}
