using UnityEngine;

public abstract class scr_edge : MonoBehaviour {

	public scr_node cell, other;

	public Direction direction;

	public void Initialize (scr_node cell, scr_node other, Direction direction) {
		this.cell = cell;
		this.other = other;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation ();
	}
}
