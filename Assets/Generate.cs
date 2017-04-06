using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

	// 2 - 7 recommend <= 6
	public static int clusterCount = 5;

	//Lots
	public static int plotCount = 10000;
	private bool run = true;

	private Color[] colors = {Color.blue, Color.red, Color.yellow, Color.green, Color.cyan, Color.magenta, Color.grey};
	public Transform cluster;
	public Transform plot;

	private Vector3[] oldCluster = new Vector3[clusterCount];
	private Transform[] clusters = new Transform[clusterCount];
	private Transform[] data = new Transform[plotCount];
	private int nearest = 0;


	// Use this for initialization
	void Start () {
		// X, 0, Z for random space. 0-25

		if (clusterCount < 1 || clusterCount > colors.Length) {
				print ("ERROR CLUSTER COUNT INVALID");
		}
		for (int i = 0; i < clusterCount; i++) {
			clusters[i] = Instantiate(cluster, new Vector3 (Random.Range (0.0f, 100.0f), 0, Random.Range (0.0f, 100.0f)), Quaternion.identity);
			clusters[i].GetComponent<Renderer>().material.color = colors[i];
		}

		if (plotCount < 1) {
			print ("ERROR PLOT COUNT INVALID");
		}

		for (int i = 0; i < plotCount; i++) {
			data[i] = Instantiate(plot, new Vector3 (Random.Range (0.0f, 100.0f), 0, Random.Range (0.0f, 100.0f)), Quaternion.identity);
		}
		for (int i = 0; i < clusters.Length; i++) {
			oldCluster [i] = clusters [i].position;
		}
	}
	
	void Update () {
		if (run) {
			for (int i = 0; i < data.Length; i++) {
				// Set Colors
				nearest = 0;
				for (int j = 0; j < clusters.Length; j++) {
					if (Vector3.Distance (data [i].position, clusters [j].position) < Vector3.Distance (data [i].position, clusters [nearest].position)) {
						nearest = j;
					}
				}
				data [i].GetComponent<Renderer> ().material.color = clusters [nearest].GetComponent<Renderer> ().material.color;
			}

			for (int i = 0; i < clusters.Length; i++) {
				float x = 0;
				float z = 0;
				int count = 0;
				for (int j = 0; j < data.Length; j++) {
					if (data [j].GetComponent<Renderer> ().material.color == clusters [i].GetComponent<Renderer> ().material.color) {
						count++;
						x += data [j].position.x;
						z += data [j].position.z;
					}
				}	
				x = x / count;
				z = z / count;
				clusters [i].position = new Vector3 (x, 0, z);
			}

			//Add check to watch if cluster have moved and if they dont, don't do all the checks.
			run = false;
			for (int i = 0; i < clusters.Length; i++) {
				if (oldCluster [i] != clusters [i].position) {
					run = true;
					break;
				}
			}
			for (int i = 0; i < clusters.Length; i++) {
				oldCluster [i] = clusters [i].position;
			}
		} else {
			for (int i = 0; i < clusterCount; i++) {
				clusters[i].GetComponent<Renderer>().material.color = Color.black;
				clusters[i].position = new Vector3 (clusters [i].position.x, 1, clusters [i].position.z);
			}
		}
	}
}
