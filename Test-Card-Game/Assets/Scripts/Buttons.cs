using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

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
        audioSource.clip = ClickSFX;
        audioSource.Play();
        GameData.instance.ClearData();
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        GameData.instance.LoadData();
        SceneManager.LoadScene(GameData.instance.level); // Load saved scene
    }

    public void Quit()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        Application.Quit();
        Debug.Log("Game Quit.");
    }
    public void Back()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        SceneManager.LoadScene(0);

    }
    public void Retry()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        GameManager.instance.gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Easy()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        SceneManager.LoadScene("Level_2x2");
    }
    public void Medium()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        SceneManager.LoadScene("Level_2x3");

    }
    public void Hard()
    {
        audioSource.clip = ClickSFX;
        audioSource.Play();
        SceneManager.LoadScene("Level_5x6");

    }
    public void PannelOpenCloss(GameObject pannelObj)
    {
        if (pannelObj.activeSelf == true)
        {
            audioSource.clip = ClickSFX;
            audioSource.Play();
            pannelObj.SetActive(false);
        }
        else
        {
            audioSource.clip = ClickSFX;
            audioSource.Play();
            pannelObj.SetActive(true);
        }
    }
}
