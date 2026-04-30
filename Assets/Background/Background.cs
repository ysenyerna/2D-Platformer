using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Background : MonoBehaviour
{

	Camera cam;

	private readonly List<ParallaxLayer> layers = new();

	private void Start()
	{
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

		// Get children
		List<GameObject> children = new();
		for (int i = 0; i < transform.childCount; i++)
		{
			children.Add(transform.GetChild(i).gameObject);
		}

		// Create layer out of each child
		foreach (var child in children)
		{
			// Get ParallaxLayer from each child
			if (!child.TryGetComponent<ParallaxLayer>(out var layer))
				continue;

			// Set up properties for each layer
			layer.offset = new (child.transform.position.x, child.transform.position.y);
			layer.mid = child;
			layer.length = child.GetComponent<SpriteRenderer>().bounds.size.x;
	
			// Create copies on the left and right for infinite scrolling
			var left = Instantiate(child, child.transform);
			left.transform.position = new (child.transform.position.x - layer.length, child.transform.position.y, child.transform.position.z);
			var right = Instantiate(child, child.transform);
			right.transform.position = new (child.transform.position.x + layer.length, child.transform.position.y, child.transform.position.z);
	
			layers.Add(layer);
		}

	}

	private void Update()
	{
		foreach (var layer in layers)
		{
			// Get the current parallax scroll distance
			var scrollDistanceX = cam.transform.position.x * layer.scrollSpeed.x;
			var scrollDistanceY = cam.transform.position.y * layer.scrollSpeed.y;
			
			// Get the distance offset to allow for infinite scrolling
			var reverseDistance = cam.transform.position.x * (1 - layer.scrollSpeed.x);
			var distanceOffset = (0.5f + Mathf.Floor(reverseDistance / layer.length)) * layer.length;

			// Set the new position
			var newPos = new Vector2(layer.offset.x + scrollDistanceX + distanceOffset, layer.offset.y + scrollDistanceY);
			layer.mid.transform.position = new Vector3(newPos.x, newPos.y, layer.mid.transform.position.z);


		}

	}




}
