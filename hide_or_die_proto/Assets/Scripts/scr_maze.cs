using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_maze : MonoBehaviour {

	public IntVector2 size;
	public scr_node pre_node;
	//public float gen_delay;
	public scr_passage pre_passage;
	public scr_wall pre_wall;

	private scr_node[,] maze_arr;

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public scr_node GetCell (IntVector2 coordinates) {
		return maze_arr[coordinates.x, coordinates.z];
	}

	public void Generate () {
		//WaitForSeconds delay = new WaitForSeconds(gen_delay);
		maze_arr = new scr_node[size.x, size.z];
		List<scr_node> active_nodes = new List<scr_node>();
		DoFirstGenerationStep(active_nodes);
		while (active_nodes.Count > 0) {
			//yield return delay;
			DoNextGenerationStep(active_nodes);
		}
	}

	private void DoFirstGenerationStep (List<scr_node> active_nodes) {
		active_nodes.Add(InitNode(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<scr_node> active_nodes) {
		int index = 0;
		// Pick a node to initialize a new node from;
		if (Random.value < 0.5) {
			index = Random.Range (0, active_nodes.Count);
		} else {
			index = active_nodes.Count - 1;
		}
		scr_node current_cell = active_nodes[index];

		// If picked node is initialized, exit;
		if (current_cell.IsInit) {
			active_nodes.RemoveAt(index);
			return;
		}
		// Pikc a random uninitialized direction;
		Direction direction = current_cell.RandomUninitDir;
		IntVector2 coordinates = current_cell.coordinates + direction.ToIntVector2();

		// Get the coordinates of the new node to initialize and check if it is in bounds;
		if (ContainsCoordinates(coordinates)) {
			scr_node neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = InitNode(coordinates);
				CreatePassage(current_cell, neighbor, direction);
				active_nodes.Add(neighbor);
			}
			else {
				CreateWall(current_cell, neighbor, direction);
			}
		}
		else {
			CreateWall(current_cell, null, direction);
		}
	}

	private scr_node InitNode (IntVector2 coordinates) {
		scr_node new_node = Instantiate(pre_node) as scr_node;
		maze_arr[coordinates.x, coordinates.z] = new_node;
		new_node.coordinates = coordinates;
		new_node.name = "Node[" + coordinates.x + ", " + coordinates.z + "]";
		new_node.transform.parent = transform;
		new_node.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 1f, 0f, coordinates.z - size.z * 0.5f + 1f);
		return new_node;
	}

	private void CreatePassage (scr_node cell, scr_node other, Direction direction) {
		scr_passage passage = Instantiate(pre_passage) as scr_passage;
		passage.Initialize(cell, other, direction);
		passage = Instantiate(pre_passage) as scr_passage;
		passage.Initialize(other, cell, direction.GetOpposite());
	}

	private void CreateWall (scr_node cell, scr_node other, Direction direction) {
		scr_wall wall = Instantiate(pre_wall) as scr_wall;
		wall.Initialize(cell, other, direction);
		//wall.transform.localPosition = new Vector3 (0f, 0.5f, 0f);
		if (other != null) {
			wall = Instantiate(pre_wall) as scr_wall;
			wall.Initialize(other, cell, direction.GetOpposite());
			//wall.transform.localPosition = new Vector3 (0f, 0.5f, 0f);
		}

	}
}