using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour {

    // Current movement direction
    // (by default it moves to the right)
    public Vector2 dir = Vector2.right;
    public List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    private bool ate = false;

    // Tail prefab
    public GameObject tailPrefab;

	// Use this for initialization
	void Start () {

        // Move the snake every 300ms
	    InvokeRepeating("Move", 0.3f, 0.3f);

	}
	
	// Update is called once per frame
	void Update () {
		
        // Move in a new direction
	    if (Input.GetKey(KeyCode.RightArrow))
	    {
	        dir = Vector2.right;
	    }
        else if (Input.GetKey(KeyCode.DownArrow))
	    {
	        dir = -Vector2.up; // "-up" means down
	    }
	    else if (Input.GetKey(KeyCode.UpArrow))
	    {
	        dir = Vector2.up;
	    }
	    else if (Input.GetKey(KeyCode.LeftArrow))
	    {
	        dir = -Vector2.right; // "-right" means left
	    }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("foodPrefab"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the food
            Destroy(coll.gameObject);
            Debug.Log("Collided with food!");


        }
        else if (coll.name.StartsWith("Border") || coll.name.StartsWith("Tail"))
        {
            KillSnake();
        }
        else
        {
            // ToDo: You lose screen

        }
    }

    void KillSnake()
    {
        GameObject head = GameObject.Find("Head");
        head.SetActive(false);
        Debug.Log("Collided with wall!");
        for (int i = 0; i < tail.Count; ++i)
        {
            tail[i].gameObject.SetActive(false);
        }
    }

    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction
        // now there is a gap
        // translate = add this vector to my position
        transform.Translate(dir);

        if (ate)
        {
            // Load prefab into the world
            GameObject g = (GameObject) Instantiate(tailPrefab, v, Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a tail?
        else if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }
}
