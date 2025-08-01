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
    [SerializeField] private Sprite bgSprite;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI chanceText;
    [SerializeField] AudioClip correctSFX;
    [SerializeField] AudioClip flipSFX;
    [SerializeField] AudioClip wrrongSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip gameOverSFX;

    public static GameManager instance;
    public Sprite[] puzzle;
    public List<Sprite> gamepuzzle = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    public int countCorrectGuesses;
    public int totalScores;
    public GameObject gameOverPanel;

    private AudioSource audioSource;
    private bool firstTry, secondTry;
    private bool comboFound;
    private int comboCount;
    private int countGuesses, gameGusses;
    private int firstGuessIntex, secondGuessIntex;
    private string firstGussePuzzle, secondGussePuzzle;
    private bool winbool;
    //private bool facedUp;

    // Start is called before the first frame update
    private void Awake()
    {
        totalScores = GameData.instance.score;
        SAVE();
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        puzzle = Resources.LoadAll<Sprite>("Sprites/Die");
    }
    void Start()
    {
        grid.constraintCount = gridRow;
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Cards");
        for (int i = 0; i < buttons.Length; i++)
        {
            btns.Add(buttons[i].GetComponent<Button>());
            btns[i].image.sprite = bgSprite;
        }
        winbool = false;
        AddGameCards();
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
            //btns[i].image.sprite = gamepuzzle[i];
            StartCoroutine(RotateCard(btns[i], gamepuzzle[i],false));
           /* audioSource.clip = flipSFX;
            audioSource.Play();*/
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

    }
    private void LateUpdate()
    {
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
           // btns[i].image.sprite = bgSprite;
            StartCoroutine(RotateCard(btns[i], gamepuzzle[i], true));
            audioSource.clip = flipSFX;
            audioSource.Play();
        }

    }
    public void PickCard()
    {
        if (!firstTry)
        {
            firstTry = true;
            firstGuessIntex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGussePuzzle = gamepuzzle[firstGuessIntex].name;
            audioSource.clip = flipSFX;
            audioSource.Play();
            //btns[firstGuessIntex].image.sprite = gamepuzzle[firstGuessIntex];
            StartCoroutine(RotateCard(btns[firstGuessIntex], gamepuzzle[firstGuessIntex], false));
        }
        else if (!secondTry)
        {
            secondTry = true;
            secondGuessIntex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGussePuzzle = gamepuzzle[secondGuessIntex].name;
            audioSource.clip = flipSFX;
            audioSource.Play();
            //btns[secondGuessIntex].image.sprite = gamepuzzle[secondGuessIntex];
            StartCoroutine(RotateCard(btns[secondGuessIntex], gamepuzzle[secondGuessIntex], false));

            if (firstGuessIntex == secondGuessIntex)
            {
                Debug.Log("You can't pick the same card twice");
                secondTry = false;
                audioSource.clip = wrrongSFX;
                audioSource.Play();
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
                    audioSource.clip = correctSFX;
                    audioSource.Play();
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
                    audioSource.clip = wrrongSFX;
                    audioSource.Play();
                    comboFoundCheck();
                }
                firstTry = secondTry = false;
                StartCoroutine(RotateCard(btns[firstGuessIntex], gamepuzzle[firstGuessIntex], true));
                StartCoroutine(RotateCard(btns[secondGuessIntex], gamepuzzle[secondGuessIntex], true));
                /*btns[firstGuessIntex].image.sprite = bgSprite;
                btns[secondGuessIntex].image.sprite = bgSprite;*/
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
            winbool = true;
            Debug.Log("You win!");
            SAVE();
            audioSource.clip = winSFX;
            audioSource.Play();
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
        if (countGuesses >= (gameGusses +2) && winbool == false)
        {
            countGuesses = 0;
            Debug.Log("You have used all your chances, try again.");
            foreach (Button butn in btns)
            {
                butn.interactable = false;
                audioSource.clip = gameOverSFX;
                audioSource.Play();
                StartCoroutine(GameOver());
            }
            
        }
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        gameOverPanel.SetActive(true); 

    }
    private IEnumerator RotateCard(Button buttons,Sprite cardPuzzle,bool facedUp)
    {

        if (!facedUp)
        {
            for (float j = 0f; j <= 180f; j += 10f)
            {
                buttons.transform.rotation = Quaternion.Euler(0f, j, 0f);
                if (j == 90f)
                {
                    buttons.image.sprite = cardPuzzle;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        else if (facedUp)
        {
            for (float j = 180f; j >= 0f; j -= 10f)
            {
                buttons.transform.rotation = Quaternion.Euler(0f, j, 0f);
                if (j == 90f)
                {
                    buttons.image.sprite = bgSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void SAVE()
    {
        GameData.instance.level = SceneManager.GetActiveScene().buildIndex;
        GameData.instance.score = totalScores;
        GameData.instance.SaveData();
        Debug.Log($"Game Saved");
    }
   
}
