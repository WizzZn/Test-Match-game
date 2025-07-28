using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCards : MonoBehaviour
{
    [SerializeField] int totalCards;
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Awake()
    {
        for (int i = 0; i < totalCards ; i++)
        {
            GameObject card = Instantiate(cardPrefab, cardParent);
            card.name = "" + i;
           
        }
    }
}
