using UnityEngine;

public class scr_wall : scr_edge {
	public Transform wall;

	public override void Initialize (scr_node cell, scr_node other, Direction direction) {
		base.Initialize(cell, other, direction);
		wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
	}
}