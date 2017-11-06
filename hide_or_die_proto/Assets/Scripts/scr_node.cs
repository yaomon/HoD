using UnityEngine;

public class scr_node : MonoBehaviour {
	public IntVector2 coordinates;
	public scr_edge[] edges = new scr_edge[Directions.count];
	private int initializedEdgecount;
	private bool dot = true;
	private bool pl_on = false;
	private int dot_count = 0;
	public scr_room room;
	public scr_roof roof;

	public void Initialize (scr_room room) {
		room.Add (this);
		transform.GetChild (0).GetComponent<Renderer> ().material = room.settings.floorMaterial;
	}

	public void OnPlayerEnter () {
		for (int i = 0; i < edges.Length; i++) {
			if (edges [i] != null) {
				edges [i].OnPlayerEnter ();
			}
			if (edges [i].other != null) {
				edges [i].other.GetEdge (edges [i].direction.GetOpposite ()).OnPlayerEnter ();
			}
		}
		pl_on = true;
	}

	public void OnPlayerExit () {
		for (int i = 0; i < edges.Length; i++) {
			if (edges [i] != null) {
				edges [i].OnPlayerExit ();
			}
			if (edges [i].other != null) {
				edges [i].other.GetEdge (edges [i].direction.GetOpposite ()).OnPlayerExit ();
			}
		}
		pl_on = false;
	}

	public bool IsInit {
		get {
			return initializedEdgecount == Directions.count;
		}
	}

	public Direction RandomUninitDir {
		get {
			int skips = Random.Range(0, Directions.count - initializedEdgecount);
			for (int i = 0; i < Directions.count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (Direction)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("Node has no uninitialized directions left.");
		}
	}

	public scr_edge GetEdge (Direction direction) {
		return edges[(int)direction];
	}

	public void SetEdge (Direction direction, scr_edge edge) {
		edges[(int)direction] = edge;
		initializedEdgecount += 1;
	}
		
	public void GenerateDeadEnd(GameObject obj) {
		int wall_count = 0;
		int dir = 0;
		for (int i = 0; i < 4; i++) {
			if ((edges [i]) is scr_wall) {
				wall_count++;
			} else {
				dir = i;
			}
		}
		if (wall_count == 3) {
			GameObject new_obj = Instantiate (obj);
			new_obj.transform.position = transform.position;
			new_obj.transform.rotation = edges [dir].direction.ToRotation();
		}
	}
}
