using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_maze : MonoBehaviour {

	public IntVector2 size;
	public scr_node pre_node;
	//public float gen_delay;
	public scr_passage pre_passage;
	public scr_wall[] pre_wall;
	public scr_room_set[] room_sets;
	public scr_door pre_door;
	public scr_broken pre_broke;
	public scr_roof pre_roof;
	[Range(0f, 1f)]
	public float db_prob;


	private scr_node[,] maze_arr;
	private List<scr_room> rooms = new List<scr_room>();


	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	private scr_room CreateRoom (int indexToExclude) {
		scr_room new_room = ScriptableObject.CreateInstance<scr_room>();
		new_room.settingsIndex = Random.Range(0, room_sets.Length);
		if (new_room.settingsIndex == indexToExclude) {
			new_room.settingsIndex = (new_room.settingsIndex + 1) % room_sets.Length;
		}
		new_room.settings = room_sets[new_room.settingsIndex];
		rooms.Add(new_room);
		return new_room;
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public scr_node GetCell (IntVector2 coordinates) {
		return maze_arr[coordinates.x, coordinates.z];
	}

	public void Generate () {
		maze_arr = new scr_node[size.x, size.z];
		List<scr_node> active_nodes = new List<scr_node>();
		DoFirstGenerationStep(active_nodes);
		while (active_nodes.Count > 0) {
			DoNextGenerationStep(active_nodes);
		}
	}

	private void DoFirstGenerationStep (List<scr_node> active_nodes) {
		scr_node new_node = InitNode (RandomCoordinates);
		new_node.Initialize (CreateRoom (-1));
		active_nodes.Add(new_node);
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
			} else if ( current_cell.room.settingsIndex == neighbor.room.settingsIndex) {
				CreatePassageInSameRoom (current_cell, neighbor, direction);
			} else {
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
		scr_roof roof = Instantiate (pre_roof as scr_roof);
		new_node.roof = roof;
		Transform height_child  = pre_wall [0].transform.GetChild (0);
		roof.transform.localPosition = new Vector3(coordinates.x * 2 - size.x + 2f, height_child.localScale.y, coordinates.z * 2 - size.z + 2f);
		return new_node;
	}

	private void CreatePassage (scr_node cell, scr_node other, Direction direction) {
		scr_passage prefab = null;
		if (Random.value < db_prob) {
			if (Random.value < 0.7) {
				prefab = pre_door;
			} else {
				prefab = pre_broke;
			}
		} else {
			prefab = pre_passage;
		}
		scr_passage passage = Instantiate(prefab) as scr_passage;
		passage.Initialize(cell, other, direction);
		cell.roof.Initialize (cell);
		passage = Instantiate(prefab) as scr_passage;
		if (passage is scr_door || passage is scr_broken) {
			other.Initialize (CreateRoom (cell.room.settingsIndex));
		} else {
			other.Initialize (cell.room);
		}

		passage.Initialize(other, cell, direction.GetOpposite());
	}


	private void CreatePassageInSameRoom (scr_node cell, scr_node other, Direction direction) {
		scr_passage passage = Instantiate(pre_passage) as scr_passage;
		passage.Initialize(cell, other, direction);
		passage = Instantiate(pre_passage) as scr_passage;
		passage.Initialize(other, cell, direction.GetOpposite());
		cell.roof.Initialize (cell);
		if (cell.room != other.room) {
			scr_room assim_room = other.room;
			cell.room.Assim (assim_room);
			rooms.Remove (assim_room);
			Destroy (assim_room);
		}
	}

	private void CreateWall (scr_node cell, scr_node other, Direction direction) {
		scr_wall wall = Instantiate(pre_wall[Random.Range(0, pre_wall.Length)]) as scr_wall;
		wall.Initialize(cell, other, direction);
		cell.roof.Initialize (cell);
		if (other != null) {
			wall = Instantiate(pre_wall[Random.Range(0, pre_wall.Length)]) as scr_wall;
			wall.Initialize(other, cell, direction.GetOpposite());
		}
	}
}