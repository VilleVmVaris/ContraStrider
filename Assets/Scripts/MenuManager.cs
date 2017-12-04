using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject menuPanel;
    public GameObject helpPanel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene("Level_1_nu");
    }

    public void ShowHelp()
    {
        helpPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        helpPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
