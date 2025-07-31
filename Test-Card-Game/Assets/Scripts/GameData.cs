using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public int level;
    public int score;

    public static GameData instance;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        Debug.Log("Data Saved: Level = "+ level +" Score =" +score);
    }

    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level",0);   
        score = PlayerPrefs.GetInt("Score",0);   
        Debug.Log("Data Loaded: Level = "+ level +" Score =" +score);
    }

    public void ClearData()
    {
        level = 1;
        score = 0;
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
       
    }
   


}
