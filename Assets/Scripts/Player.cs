using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour {
    public GameObject player;
    public GameObject head;
    public Material headMaterial, tailMaterial;
    public List<GameObject> tail;

    public List<GameObject> Tail {
        get { return tail; }
    }

    public void setTail(List<GameObject> newTail) {
        tail = newTail;
    }

    public Vector2 dir;
    public bool isAlive = true;
    private bool dirChanged = false;
    public float passedTime;
    private Vector3 newPlayerPosition;
    
    public Vector3 NewPlayerPosition {
        get { return newPlayerPosition; }
    }

    public void SetNewPlayerPosition(Vector3 position) {
        newPlayerPosition = position;
    }

    private Vector3 oldPlayerPosition;

    public Vector3 OldPlayerPosition {
        get { return oldPlayerPosition; }
    }

    public void SetOldPlayerPosition(Vector3 position) {
        oldPlayerPosition = position;
    }

    private bool grown;

    // Start is called before the first frame update
    void Start() {
        dir = Vector2.left;
        CreatePlayer();
    }

    private void CreatePlayer() {
        head = Instantiate(player) as GameObject;
        head.name = "Player";
        head.GetComponent<MeshRenderer>().material = headMaterial;
        tail = new List<GameObject>();
        head.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.DownArrow) && dir != Vector2.up && !dirChanged) {
            dir = Vector2.down;
            dirChanged = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && dir != Vector2.down && !dirChanged) {
            dir = Vector2.up;
            dirChanged = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && dir != Vector2.left && !dirChanged) {
            dir = Vector2.right;
            dirChanged = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && dir != Vector2.right && !dirChanged) {
            dir = Vector2.left;
            dirChanged = true;
        }
    }

    public void moveSnake() {
        if (isAlive) {
            //Move
            SetNewPlayerPosition(head.GetComponent<Transform>().position + new Vector3(dir.x, dir.y, 0));
            //Check if collides with border
            OnTriggerEnter(head.GetComponent<BoxCollider>());
            //check collides with body
            CollidesWithSelf();
            // reset input
            dirChanged = false;
            //check length
            if (tail.Count == 0) {
                head.transform.position = newPlayerPosition;
            }
            else {
                head.GetComponent<MeshRenderer>().material = tailMaterial;
                tail.Add(head);
                head = tail[0];
                head.GetComponent<MeshRenderer>().material = headMaterial;
                tail.RemoveAt(0);
                head.transform.position = newPlayerPosition;
            }
        }
    }

    public void growSnake(Vector3 foodPosition) {
        if (isAlive) {
            SetNewPlayerPosition(head.GetComponent<Transform>().position + new Vector3(dir.x, dir.y, 0));
            head.GetComponent<MeshRenderer>().material = headMaterial;
            GameObject newSegment = Instantiate(player);
            newSegment.SetActive(true);
            newSegment.transform.position = NewPlayerPosition;
            head.GetComponent<MeshRenderer>().material = tailMaterial;
            tail.Add(head);
            head = newSegment;
            head.GetComponent<MeshRenderer>().material = headMaterial;

        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Boundary")) {
            // Set the player's new position to the boundary position
            Death();
        }
    }

    public void Death() {
        isAlive = false;
    }

    public void CollidesWithSelf() {
        //Check id collides with any tail tile.
        foreach (var item in tail) {
            if (item.transform.position == newPlayerPosition) {
                Death();
            }
        }
    }
}