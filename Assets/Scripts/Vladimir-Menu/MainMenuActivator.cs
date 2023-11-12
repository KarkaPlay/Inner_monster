using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenuActivator : MonoBehaviour
{
    public string Scene;
    public Button mainMenu;
    public Button desktop;
    public bool EscapeMenuOpen;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (EscapeMenuOpen == false)
            {
                Time.timeScale = 0f;
                EscapeMenuOpen = true;
               mainMenu.gameObject.SetActive(true);
                desktop.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                EscapeMenuOpen = false;
                mainMenu.gameObject.SetActive(false);
                desktop.gameObject.SetActive(false);
            }
        }
    }


    public void GoToMenu()
    {
        SceneManager.LoadScene(Scene);
    }

    public void GoToDesktop()
    {
        Application.Quit();
    }
}
