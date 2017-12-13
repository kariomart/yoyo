using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefDel : MonoBehaviour {


	[MenuItem("Edit/Reset Playerprefs")]
	public static void DeletePlayerPrefs() {

		PlayerPrefs.DeleteAll();

	}
}
