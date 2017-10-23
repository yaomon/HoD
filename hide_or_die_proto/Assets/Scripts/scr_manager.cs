using System.Collections.Generic;
using UnityEngine;

public class scr_manager : MonoBehaviour {

	public scr_maze maze_prefab;
	private scr_maze maze_inst;

	private void Start () {
		StartGame();
		GlobalRef.player = Instantiate (Resources.Load("FPSController")) as scr_player;
	}
			
	private void StartGame () {
		maze_inst = Instantiate (maze_prefab) as scr_maze;
		maze_inst.Generate ();
	}

	private void RestartGame () {
		Destroy (maze_inst.gameObject);
		StartGame();
	}
}