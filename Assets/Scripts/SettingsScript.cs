using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SettingsScript : MonoBehaviour {
    public Toggle CRTEffectButton;
    public GameObject ScreenEffect;
    private bool isStarting;

    // Start is called before the first frame update
    void Start() {
        isStarting = true;
        this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
            bool.Parse(PlayerPrefs.GetString("CRTToggle", "True"));        
        CRTEffectButton.GetComponent<Toggle>().isOn = bool.Parse(PlayerPrefs.GetString("CRTToggle", "True"));
        isStarting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableCRT() {
        if (!isStarting) {
            this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
                !this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing;
            String val = this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing.ToString();
            PlayerPrefs.SetString("CRTToggle", val);
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
