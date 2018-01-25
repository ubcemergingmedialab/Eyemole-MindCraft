using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

public class RaiseTerrain : MonoBehaviour {

	// This script will raise the terrain at the pointer location 
	// when the touchpad on the parent controller is pressed 

	private VRTK_ControllerEvents evts;
	private VRTK_StraightPointerRenderer pointerRenderer;

	public Terrain terrain;

	private int xResolution;
	private int zResolution;
	private float[,] heights;
	private float[,] heightMapBackup;
	protected int alphaMapWidth;
	protected int alphaMapHeight;
	protected int numOfAlphaLayers;
	private float[,,] alphaMapBackup;

	//The parameters below specify the size and rate of change of the terrain patch under the pointer
	//KEEP THESE PARAMETERS SMALL - the current terrain is 513x513 to give you an idea of the scale 

	public int lenx = 1; //the x-width of the rectangle of raised terrain
	public int lenz = 1; //the z-width of the rectangle of raised terrain
	public int smooth = 1; //the area of smoothed terrain that will have a slope 

	public float maxRaiseRate = 0.00001f; // the maximum rate at which the terrain will be raised (occurs at peak alpha)

	public float alphaThreshold = 0.2f; //threshold for the relative alpha - if it's below threshold, terrain will be lowered 

	// Use this for initialization
	void Start() {
		evts = GetComponentInParent<VRTK_ControllerEvents>();

		pointerRenderer = GetComponentInParent<VRTK_StraightPointerRenderer>();

		xResolution = terrain.terrainData.heightmapWidth;
		zResolution = terrain.terrainData.heightmapHeight;
		Debug.Log(xResolution.ToString());
		Debug.Log(zResolution.ToString());
		alphaMapWidth = terrain.terrainData.alphamapWidth;
		alphaMapHeight = terrain.terrainData.alphamapHeight;
		numOfAlphaLayers = terrain.terrainData.alphamapLayers;


		//Back up the terrain's height and alpha maps so that you don't permanently alter it 
		heightMapBackup = terrain.terrainData.GetHeights(0, 0, xResolution, zResolution);
		alphaMapBackup = terrain.terrainData.GetAlphamaps(0, 0, alphaMapWidth, alphaMapHeight);

	}

	void OnApplicationQuit() {
		//Reset the terrain's shape to what it was originally 

		terrain.terrainData.SetHeights(0, 0, heightMapBackup);
		terrain.terrainData.SetAlphamaps(0, 0, alphaMapBackup);

	}

	private void FixedUpdate() {

		if (evts.touchpadPressed) {
			RaiseTerrainUnderPointer();
		}
	}

	void RaiseTerrainUnderPointer() {

		RaycastHit hit = pointerRenderer.GetDestinationHit();
		Vector3 point = hit.point;

		int areax;
		int areaz;
		float smoothing;
		int terX = (int)((point.x / terrain.terrainData.size.x) * xResolution);
		int terZ = (int)((point.z / terrain.terrainData.size.z) * zResolution);
		lenx += smooth;
		lenz += smooth;
		terX -= (lenx / 2);
		terZ -= (lenz / 2);
		if (terX < 0) terX = 0;
		if (terX > xResolution) terX = xResolution;
		if (terZ < 0) terZ = 0;
		if (terZ > zResolution) terZ = zResolution;

		float[,] heights = terrain.terrainData.GetHeights(terX, terZ, lenx, lenz);

		float rateMultiplier = 1f;
		if (EEGData.GetEEGData()[1] != 0) {
			rateMultiplier = GetRelativeAlpha();
		}

		float y = heights[lenx / 2, lenz / 2];
		y += maxRaiseRate * rateMultiplier;
		for (smoothing = 1; smoothing < smooth + 1; smoothing++) {
			float multiplier = smoothing / smooth;
			for (areax = (int)(smoothing / 2); areax < lenx - (smoothing / 2); areax++) {
				for (areaz = (int)(smoothing / 2); areaz < lenz - (smoothing / 2); areaz++) {
					if ((areax > -1) && (areaz > -1) && (areax < xResolution) && (areaz < zResolution)) {
						heights[areax, areaz] = Mathf.Clamp((float)y * multiplier, 0, 1);
					}
				}
			}
		}
		lenx -= smooth;
		lenz -= smooth;

		UpdateObjectPositions(point, Mathf.Max(lenx, lenz), maxRaiseRate * rateMultiplier);

		terrain.terrainData.SetHeights(terX, terZ, heights);
		terrain.terrainData.RefreshPrototypes();
		TerrainCollider terrainCollider = terrain.GetComponent<TerrainCollider>();
		terrainCollider.terrainData = terrain.terrainData;
		terrain.Flush();
	}

	private void UpdateObjectPositions(Vector3 point, int radius, float deltaY) {

		//Update the y-position of each object by deltaY within the sphere specified by point and radius

		Collider[] hitColliders = Physics.OverlapSphere(point, radius);

		foreach (Collider col in hitColliders) {
			Transform colTransform = col.gameObject.transform;
			Vector3 newPos = new Vector3(colTransform.position.x, colTransform.position.y + deltaY, colTransform.position.z);
			colTransform.position = Vector3.Lerp(colTransform.position, newPos, Time.deltaTime);
		}

	}

	private float GetRelativeAlpha() {

		float relAlpha = EEGData.GetRelativeFrequency(EEGData.EEG_BANDS.ALPHA);

		return relAlpha > alphaThreshold ? relAlpha : ((1f - relAlpha / alphaThreshold) * -1f);
	}
}