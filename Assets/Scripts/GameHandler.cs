using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour {
    public int xSize, ySize;
    public GameObject block;
    public Text score;
    public Text highScore;
    public Text rewindTimerText;
    public GameObject player;
    public Vector3 playerLocation;
    public Material foodMaterial;
    public Material wallMaterial;
    public GameObject food;
    public UnityEvent gamePausedEvent;
    public UnityEvent gameResumedEvent;

    private float passedTime;
    public float timeBetweenMovements;

    public GameObject gameOverUI;
    public GameObject inGameUI;
    public GameObject pauseUI;
    public int scoreInt;
    private int highScoreInt;
    private List<GameObject> tailList;
    private float maxSpeed = 0.2f;
    private bool gamePaused = false;
    public int rewindInt = 5;

    // Start is called before the first frame update
    void Start() {
        this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
            bool.Parse(PlayerPrefs.GetString("CRTToggle", "True"));
        block.SetActive(true);
        timeBetweenMovements = 0.3f;
        CreateGrid();
        playerLocation = player.GetComponent<Player>().NewPlayerPosition;
        tailList = player.GetComponent<Player>().tail;
        SpawnFood();
        block.SetActive(false);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Update() {
        CheckForPause();
        UpdateScore();
        CheckHighScore();
        AchievementCheck();
        scoreInt = tailList.Count;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!gamePaused) {
            passedTime += Time.fixedDeltaTime;
        }
        playerLocation = player.GetComponent<Player>().NewPlayerPosition;
        tailList = player.GetComponent<Player>().tail;
        player.transform.position = playerLocation;
        if (timeBetweenMovements < passedTime) {
            bool boolean = timeBetweenMovements < passedTime;
            passedTime = 0;
            if (playerLocation.x == food.transform.position.x && playerLocation.y == food.transform.position.y) {
                Vector3 foodPosition = food.transform.position;
                player.GetComponent<Player>().growSnake(foodPosition);
                DestroyImmediate(food);
                SpawnFood();
                if (rewindInt > 1) {
                    rewindInt--;
                    rewindTimerText.text = "Rewind in  " + rewindInt.ToString();
                }
                else {
                    rewindInt--;
                    rewindTimerText.text = "'R' to rewind!";
                }
            }
            else if (playerLocation.x <= -xSize / 2 || playerLocation.x >= xSize / 2 ||
                     playerLocation.y <= -ySize / 2 || playerLocation.y >= ySize / 2) {
                player.GetComponent<Player>().Death();
            }
            else {
                player.GetComponent<Player>().moveSnake();
            }
        }
        if (player.GetComponent<Player>().isAlive == false) {
            GameOver();
        }
    }

    private void CheckForPause() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            gamePaused = true;
        }
        if (gamePaused) {
            Time.timeScale = 0;
            gamePausedEvent.Invoke();
        }
    }

    public void UnpauseGame() {
        gamePaused = false;
        Time.timeScale = 1;
        gameResumedEvent.Invoke();
    }

    private void AchievementCheck() {
        // hatchling - true
        PlayerPrefs.SetString("HatchlingUnlocked", "true");
        //black mamba - top speed
        if (Mathf.Approximately(1f, maxSpeed/timeBetweenMovements)) {
            PlayerPrefs.SetString("BlackMambaUnlocked", "true");
        }
        // hungry - 5 current score
        if (scoreInt == 5) {
            PlayerPrefs.SetString("HungryUnlocked", "true");
        }
        // python - score == 33
        if (scoreInt == 33) {
            PlayerPrefs.SetString("PythonUnlocked", "true");
        }
        //millennial - if camera.Camera.postprocessing is off then true
        if (this.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing == false) {
            PlayerPrefs.SetString("MillennialUnlocked", "true");
        }
    }

    private void CreateGrid() {
        for (int x = 0; x <= xSize; x++) {
            Material mat = new WallMaterialType().GetMaterial();
            GameObject borderBottom = Instantiate(block) as GameObject;
            borderBottom.GetComponent<Transform>().position = new Vector3(x - xSize / 2, -ySize / 2, 91);
            borderBottom.SetActive(true);
            borderBottom.GetComponent<MeshRenderer>().material = mat;
            borderBottom.name = "wall";
            GameObject borderTop = Instantiate(block) as GameObject;
            borderTop.GetComponent<Transform>().position = new Vector3(x - xSize / 2, ySize - ySize / 2, 91);
            borderTop.SetActive(true);
            borderTop.GetComponent<MeshRenderer>().material = mat;
            borderTop.name = "wall";
        }

        for (int y = 0; y <= ySize; y++) {
            Material mat = new WallMaterialType().GetMaterial();
            GameObject borderRight = Instantiate(block) as GameObject;
            borderRight.GetComponent<Transform>().position = new Vector3(-xSize / 2, y - (ySize / 2), 91);
            borderRight.SetActive(true);
            borderRight.GetComponent<MeshRenderer>().material = mat;
            borderRight.name = "wall";
            GameObject borderLeft = Instantiate(block) as GameObject;
            borderLeft.GetComponent<Transform>().position = new Vector3(xSize - (xSize / 2), y - (ySize / 2), 91);
            borderLeft.SetActive(true);
            borderLeft.GetComponent<MeshRenderer>().material = mat;
            borderLeft.name = "wall";
        }
    }

    private void CheckHighScore() {
        if (scoreInt > PlayerPrefs.GetInt("HighScore", 0)) {
            PlayerPrefs.SetInt("HighScore", scoreInt);
        }
    }

    private void UpdateHighScore() {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void UpdateScore() {
        score.text = "score  " + scoreInt;
        if (scoreInt % 5 == 0) {
            if (timeBetweenMovements > maxSpeed) {
                timeBetweenMovements = 0.3f - (0.025f * Mathf.FloorToInt(scoreInt / 5));
            }
        }
    }

    private void GameOver() {
        gameOverUI.SetActive(true);
        inGameUI.SetActive(false);
        UpdateHighScore();
    }

    public void StartGame() {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    public void StartMenu() {
        inGameUI.SetActive(false);
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    public bool containedInSnake(Vector2 spawnPos) {
        bool isInHead = spawnPos.x == playerLocation.x && spawnPos.y == playerLocation.y;
        bool isInTail = false;
        foreach (var item in tailList) {
            if (item.transform.position.x == spawnPos.x && item.transform.position.y == spawnPos.y) {
                isInTail = true;
            }
        }
        return isInHead || isInTail;
    }

    private void SpawnFood() {
        Material mat = new FoodMaterialType().GetMaterial();
        Vector2 spawnPos = getRandomPos();
        while (containedInSnake(spawnPos)) {
            spawnPos = getRandomPos();
        }
        food = Instantiate(block);
        food.transform.position = new Vector3(spawnPos.x, spawnPos.y, 91);
        food.GetComponent<MeshRenderer>().material = mat;
        food.SetActive(true);
        food.name = "food";
    }

    private Vector2 getRandomPos() {
        return new Vector2(Random.Range(-xSize / 2 + 1, xSize / 2), Random.Range(-ySize / 2 + 1, ySize / 2));
    }
}