using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {
	public LayerMask collisionMask;

	protected const float skinWidth = 0.015f;
	public int horizontalRaycount = 4;
	public int verticalRaycount = 4;

	protected float maxClimbingAngle = 50.0f;
	protected float maxDescentAngle = 75.0f;

	protected float horizontalRaySpacing;
	protected float verticalRaySpacing;

	protected BoxCollider2D col;
	protected RaycastOrigins raycastOrigins;

	public virtual void Start () {
		col = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	//Update das origens dos raycasts
	protected void UpdateRaycastOrigins() {
		Bounds bounds = col.bounds;
		bounds.Expand(skinWidth * (-2));

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	//Espaçamento dos raycast
	protected void CalculateRaySpacing() {
		Bounds bounds = col.bounds;
		bounds.Expand(skinWidth * (-2));

		horizontalRaycount = Mathf.Clamp (horizontalRaycount, 2, int.MaxValue);
		verticalRaycount = Mathf.Clamp (verticalRaycount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRaycount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRaycount - 1);

	}

	protected struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

}