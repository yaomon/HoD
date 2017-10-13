using UnityEngine;

public class scr_node : MonoBehaviour {
	public IntVector2 coordinates;
	private scr_edge[] edges = new scr_edge[Directions.count];
	private int initializedEdgecount;

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
}