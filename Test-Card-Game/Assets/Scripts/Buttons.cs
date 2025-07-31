using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private AudioSource audioSource;
    public static Buttons instance;
    public AudioClip ClickSFX;
    void Start()
    {
         audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }
    public void NewGame()
    {
        GameData.instance.ClearData();
        SceneManager.LoadScene(1);
        audioSource.clip = ClickSFX;
        audioSource.Play();
    }

    public void LoadGame()
    {
        GameData.instance.LoadData();
        SceneManager.LoadScene(GameData.instance.level); // Load saved scene
        audioSource.clip = ClickSFX;
        audioSource.Play();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit.");
        audioSource.clip = ClickSFX;
        audioSource.Play();
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
        audioSource.clip = ClickSFX;
        audioSource.Play();

    }
    public void Retry()
    {
        GameManager.instance.gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
