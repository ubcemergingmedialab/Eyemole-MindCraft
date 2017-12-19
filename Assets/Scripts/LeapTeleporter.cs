using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapTeleporter : MonoBehaviour {


	public float rechargeTime = 1f;
	private float timeLastTeleported;

	public float maxTeleportDistance = 5f;

	private Detector detector;
	private LineRenderer rend;

	public LeapServiceProvider controller;
	public Transform reference;

	void Start() {

		timeLastTeleported = 0f;

		detector = GetComponent<Detector>();
		rend = GetComponent<LineRenderer>();

		rend.startWidth = 0.01f;
		rend.endWidth = 0.01f;

		detector.OnActivate.AddListener(ActivateRenderer);
		detector.OnDeactivate.AddListener(DoClick);

		// Start the player at the level of the terrain
		var t = reference;
		if (t != null)
			t.position = new Vector3(t.position.x, Terrain.activeTerrain.SampleHeight(t.position), t.position.z);
		
	}

	public void ActivateRenderer() {

		rend.enabled = true;
	}

	public void DeactivateRenderer() {

		rend.enabled = false;
	}


	public void Update() {

		Hand hand = controller.CurrentFrame.Hands.Find(h => h.IsLeft);

		if (hand != null) {
			Finger index = hand.Fingers[(int)Finger.FingerType.TYPE_INDEX];

			Vector3 tipPosition = index.TipPosition.ToVector3();
			Vector3 direction = index.Direction.ToVector3();

			Vector3 oldOrigin = rend.GetPosition(0);
			Vector3 oldEnd = rend.GetPosition(1);

			rend.SetPositions(new Vector3[2] { Vector3.Lerp(oldOrigin, tipPosition, 0.5f), Vector3.Lerp(oldEnd, tipPosition + direction * maxTeleportDistance, 0.5f) });
		}

	}

	public void DoClick() {
		

		// First get the current Transform of the the reference space (i.e. the Play Area, e.g. CameraRig prefab)
		var t = reference;


		// Get the current Y position of the reference space
		float refY = t.position.y;
		
		// Create a Ray from the origin of the index fingertip in the direction that the index finger is pointing
		Hand hand = controller.CurrentFrame.Hands.Find(h => h.IsLeft);

		if ((hand != null) && IsReadyToTeleport()) {

			Finger index = hand.Fingers[(int)Finger.FingerType.TYPE_INDEX];
			Ray ray = new Ray(index.TipPosition.ToVector3(), index.Direction.ToVector3());

			// Set defaults
			bool hasGroundTarget = false;
			float dist = 0f;

			RaycastHit hitInfo;
			TerrainCollider tc = Terrain.activeTerrain.GetComponent<TerrainCollider>();
			hasGroundTarget = tc.Raycast(ray, out hitInfo, maxTeleportDistance);
			dist = hitInfo.distance;


			if (hasGroundTarget) {
				// Get the current Camera (head) position on the ground relative to the world
				Vector3 headPosOnGround = new Vector3(t.position.x, refY, t.position.z);

				// We need to translate the reference space along the same vector
				// that is between the head's position on the ground and the intersection point on the ground
				// i.e. intersectionPoint - headPosOnGround = translateVector
				// currentReferencePosition + translateVector = finalPosition
				t.position = t.position + (ray.origin + (ray.direction * dist)) - headPosOnGround;
				timeLastTeleported = Time.time;

			}
		}

		DeactivateRenderer();
	}

	private bool IsReadyToTeleport() {
		float currentTime = Time.time;
		return (currentTime - timeLastTeleported) > rechargeTime;
	}
}
