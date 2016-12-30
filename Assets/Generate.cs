using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

	// 2 - 8 recommend <= 6
	public static int clusterCount = 6;

	//Lots
	public static int plotCount = 10000;

	private Color[] colors = {Color.blue, Color.red, Color.yellow, Color.green, Color.cyan, Color.magenta, Color.grey, Color.black};
	public Transform cluster;
	public Transform plot;

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
	}
	
	void Update () {
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
	}
}
