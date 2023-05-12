using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneHistory : MonoBehaviour {
    // Reference to the Player script or component
    public Player player;

    // Reference to the GameHandler script or component
    public GameHandler gameHandler;

    // The interval at which to save the data state
    public float saveInterval = 0.3f;

    // The maximum number of saved data lists to keep in the list
    public int maxSavedData = 5;

    // The list of saved data
    List<SavedGameData> savedGameDatas = new List<SavedGameData>();

    void Start() {
        // Start the coroutine to save scenes at regular intervals
        StartCoroutine(SaveSceneCoroutine());
    }

    private void Update() {
        if (gameHandler.rewindInt == 0) {
            if (gameHandler.pauseUI.activeSelf == false) {
                if (Input.GetKeyDown(KeyCode.R)) {
                    if (savedGameDatas.Count > maxSavedData) {
                        LoadSavedData(savedGameDatas.Count - maxSavedData);
                        gameHandler.rewindInt = 5;
                    }
                }
            }
        }
    }

    public struct SavedGameData {
        public Vector3 oldHeadPosition;
        public Vector3 direction;
        public List<Vector3> tailSegments;
        public Vector3 fruitLocation;
        public Text score;
        public float timeBetweenMovements;
    }

    private IEnumerator SaveSceneCoroutine() {
        yield return new WaitForSeconds(saveInterval);
        if (savedGameDatas.Count > maxSavedData) {
            // If so, delete the oldest saved data
            savedGameDatas.RemoveAt(savedGameDatas.Count - 6);
        }
        while (true) {
            //CREATE NEW SAVED DATA OBJECT
            SavedGameData savedGameData = new SavedGameData();
            //SET HEAD TRANSOFRM
            savedGameData.oldHeadPosition = player.head.transform.position;
            //SET PLAYER DIRECTION
            savedGameData.direction = player.dir;
            //IF TAIL IS NOT NULL
            savedGameData.tailSegments = new List<Vector3>();
            if (player.tail.Count > 0) {
                foreach (GameObject tailSegment in player.tail) {
                    Vector3 tailPosition = tailSegment.transform.position;
                    savedGameData.tailSegments.Add(tailPosition);
                }
            }
            //SET FRUIT LOCATION
            savedGameData.fruitLocation = gameHandler.food.transform.position;
            //SET SCORE
            savedGameData.score = gameHandler.score;
            //SET TIME INTERVAL
            savedGameData.timeBetweenMovements = gameHandler.timeBetweenMovements;
            //ADD THIS OBJECT TO A LIST OF SAVED DATA OBJECTS
            savedGameDatas.Add(savedGameData);
            //WAIT FOR SNAKE MOVEMENT BEFORE SAVING NEXT DATA OBJECT
            yield return new WaitForSeconds(saveInterval);
        }
    }

    public void LoadSavedData(int index) {
        SavedGameData savedGameData = savedGameDatas[index];
        player.head.transform.position = savedGameData.oldHeadPosition;
        int endTailSegment = player.tail.Count - 1;
        if (player.tail.Count > 0) {
            if (savedGameData.tailSegments.Count == 0 && player.tail.Count > 0) {
                DestroyImmediate(player.tail[0]);
                player.tail.Clear();
            }
            if (savedGameData.tailSegments.Count > 0) {
                if (player.tail.Count > savedGameData.tailSegments.Count) {
                    DestroyImmediate(player.tail[endTailSegment]);
                    player.tail.RemoveAt(endTailSegment);
                }
                for (int i = 0; i < savedGameData.tailSegments.Count; i++) {
                    // Make sure the tail segment exists at the corresponding index
                    if (i < player.tail.Count) {
                        GameObject tailSegment = player.tail[i];
                        Vector3 savedPosition = savedGameData.tailSegments[i];
                        tailSegment.transform.position = savedPosition;
                    }
                }
            }
        }
        player.dir = savedGameData.direction;
        gameHandler.food.transform.position = savedGameData.fruitLocation;
        gameHandler.score = savedGameData.score;
        gameHandler.timeBetweenMovements = savedGameData.timeBetweenMovements;
        int saveLoad = savedGameDatas.Count - maxSavedData;
        while (saveLoad != savedGameDatas.Count) {
            savedGameDatas.RemoveAt(savedGameDatas.Count - 1);
        }
    }
}