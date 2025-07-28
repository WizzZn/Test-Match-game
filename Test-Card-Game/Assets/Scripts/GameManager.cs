using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] int gridRow;

    public Sprite[] puzzle;
    public List<Sprite> gamepuzzle = new List<Sprite>();
    public List<Button> btns = new List<Button>();

    private bool firstTry, secondTry;
    private bool comboFound;
    private int comboCount;
    private int countGuesses, countCorrectGuesses, gameGusses;
    private int firstGuessIntex, secondGuessIntex;
    private string firstGussePuzzle, secondGussePuzzle;
    [SerializeField] private Sprite bgSprite;
    // Start is called before the first frame update
    private void Awake()
    {
        puzzle = Resources.LoadAll<Sprite>("Sprites/Die");
    }
    void Start()
    {
        GetButtons();
        addListener();
        AddGameCards();
        grid.constraintCount = gridRow;
        gameGusses = gamepuzzle.Count / 2;
    }

    // Update is called once per frame
    void Update()
    {
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
            btns.Add(buttons[i].GetComponent<Button>());
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
            countGuesses++;

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
                    btns[firstGuessIntex].interactable = false;
                    btns[secondGuessIntex].interactable = false;
                   // comboFoundCheck();
                   /* if (comboFound == false)
                    {
                        comboFound = true;

                    }*/

                }
                else
                {
                    yield return new WaitForSeconds(1f);

                    Debug.Log("Not a match, try again.");
                    //comboFound = false;
                    //comboFoundCheck();
                }
                firstTry = false;
                secondTry = false;
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
            // Here you can add logic to reset the game or show a win message
        }
    }
    /*void comboFoundCheck()
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
      
    }*/
}
