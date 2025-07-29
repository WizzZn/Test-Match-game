using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] int gridRow;

    public static GameManager instance;
    public Sprite[] puzzle;
    public List<Sprite> gamepuzzle = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    public int countCorrectGuesses;
    public int totalScores;

    private bool firstTry, secondTry;
    private bool comboFound;
    private int comboCount;
    private int countGuesses, gameGusses;
    private int firstGuessIntex, secondGuessIntex;
    private string firstGussePuzzle, secondGussePuzzle;
    [SerializeField] private Sprite bgSprite;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI chanceText;

    // Start is called before the first frame update
    private void Awake()
    {
        totalScores = GameData.instance.score;
        if (instance == null)
        {
            instance = this;
        }

        puzzle = Resources.LoadAll<Sprite>("Sprites/Die");
    }
    void Start()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Cards");
        for (int i = 0; i < buttons.Length; i++)
        {
            btns.Add(buttons[i].GetComponent<Button>());
            btns[i].image.sprite = bgSprite;
        }
        AddGameCards();
        grid.constraintCount = gridRow;
        gameGusses = gamepuzzle.Count / 2;
        ShuffleCards(gamepuzzle);
        StartCoroutine(CardShowing());
    }
    IEnumerator CardShowing()
    {
        yield return new WaitForSeconds(1f);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Cards");
        for (int i = 0; i < buttons.Length; i++)
        {
            // btns.Add(buttons[i].GetComponent<Button>());
            btns[i].image.sprite = gamepuzzle[i];
        }

        yield return new WaitForSeconds(2f);
        GetButtons();
        addListener();


    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score:\n" + totalScores;
        comboText.text = "Combo:\n" + comboCount;
        chanceText.text = "Chance:\n" + (gameGusses + 2) + "/" + (gameGusses + 2 - countGuesses); 

        chance();

    }
    void AddGameCards()
    {
        int looper = btns.Count;
        int intex = 0;
        for (int i = 0; i < looper; i++)
        {
            if (intex == looper / 2)
            {
                intex = 0;
            }
            gamepuzzle.Add(puzzle[intex]);
            intex++;
        }
    }
    void GetButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Cards");
        for (int i = 0; i < buttons.Length; i++)
        {
            // btns.Add(buttons[i].GetComponent<Button>());
            btns[i].image.sprite = bgSprite;
        }

    }
    public void PickCard()
    {
        if (!firstTry)
        {
            firstTry = true;
            firstGuessIntex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGussePuzzle = gamepuzzle[firstGuessIntex].name;
            btns[firstGuessIntex].image.sprite = gamepuzzle[firstGuessIntex];
        }
        else if (!secondTry)
        {
            secondTry = true;
            secondGuessIntex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGussePuzzle = gamepuzzle[secondGuessIntex].name;
            btns[secondGuessIntex].image.sprite = gamepuzzle[secondGuessIntex];
            

            if (firstGuessIntex == secondGuessIntex)
            {
                Debug.Log("You can't pick the same card twice");
                secondTry = false;
                return;
            }
            StartCoroutine(CardsChecking());

            IEnumerator CardsChecking()
            {
                yield return new WaitForSeconds(.5f);
                if (firstGussePuzzle == secondGussePuzzle)
                {
                    Debug.Log("You found a match!");
                    countCorrectGuesses++;
                    totalScores += 10; 
                    countGuesses++;
                    GameData.instance.score = totalScores;
                    btns[firstGuessIntex].interactable = false;
                    btns[secondGuessIntex].interactable = false;
                    comboFoundCheck();
                    if (comboFound == false)
                    {
                        comboFound = true;

                    }

                }
                else
                {
                    yield return new WaitForSeconds(1f);

                    Debug.Log("Not a match, try again.");
                    countGuesses++;
                    comboFound = false;
                    comboFoundCheck();
                }
                firstTry = secondTry = false;
                btns[firstGuessIntex].image.sprite = bgSprite;
                btns[secondGuessIntex].image.sprite = bgSprite;
                checkguesses();
            }
        }
    }
    void addListener()
    {
        foreach (Button butn in btns)
        {
            butn.onClick.AddListener(() => PickCard());
        }
    }
    void checkguesses()
    {
        if (countCorrectGuesses == gameGusses)
        {
            Debug.Log("You win!");
            GameData.instance.level = SceneManager.GetActiveScene().buildIndex + 1;
            GameData.instance.score = countCorrectGuesses;
            GameData.instance.SaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void comboFoundCheck()
    {
        if (comboFound)
        {
            comboCount++;
            Debug.Log($"Combo Found!" + comboCount);
        }
        else
        {
            comboCount = 0;
            Debug.Log("No combo found, reset count." + comboCount);
        }

    }
    void ShuffleCards(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = gamepuzzle[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    void chance()
    {
        if (countGuesses >= (gameGusses +2))
        {
            countGuesses = 0;
            Debug.Log("You have used all your chances, try again.");
            foreach (Button butn in btns)
            {
                butn.interactable = false;
            }
            
        }
    }
   
}
