using System.Collections.Generic;
using UnityEngine;

public class scr_manager : MonoBehaviour {

	public scr_maze maze_prefab;

	private scr_maze maze_inst;

	private void Start () {
		StartGame();
		GameObject player = Instantiate (Resources.Load("FPSController")) as GameObject;
	}

	/*private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}*/

	private void StartGame () {
		maze_inst = Instantiate (maze_prefab) as scr_maze;
		maze_inst.Generate ();
		//StartCoroutine ();
	}

	private void RestartGame () {
		//StopAllCoroutines ();
		Destroy (maze_inst.gameObject);
		StartGame();
	}
}