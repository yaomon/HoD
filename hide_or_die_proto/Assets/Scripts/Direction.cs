﻿using UnityEngine;

public enum Direction {
	North,
	East,
	South,
	West
}

public static class Directions {
	public const int count = 4;
	public static Direction RandomValue {
		get {
			return (Direction)Random.Range (0, count);
		}
	}

	private static IntVector2[] vectors = {
		new IntVector2(0, 1),
		new IntVector2(1, 0),
		new IntVector2(0, -1),
		new IntVector2(-1, 0)
	};

	private static Direction[] opposites = {
		Direction.South,
		Direction.West,
		Direction.North,
		Direction.East
	};

	private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	public static Quaternion ToRotation (this Direction direction) {
		return rotations[(int)direction];
	}

	public static IntVector2 ToIntVector2 (this Direction direction) {
		return vectors[(int)direction];
	}

	public static Direction GetOpposite (this Direction direction) {
		return opposites[(int)direction];
	}
}
