﻿using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	public String saveDirectoryName = "Saves";
	public String saveFileName = "saveFile.dat";

	public static List<CubeStateManager.PersonalityCube> savedCubes;

	void Save() {

		if (!Directory.Exists (saveDirectoryName)) {
			Directory.CreateDirectory (saveDirectoryName);
		}

		BinaryFormatter formater = new BinaryFormatter ();
		FileStream saveFile = File.Create (Path.Combine (saveDirectoryName, saveFileName));




	}

}
