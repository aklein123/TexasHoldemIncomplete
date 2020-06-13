using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class CardSprites
{
    public static Sprite[] cardSprites;
}

// enum GameStates{
//     DEALING,
//     BLINDS,
//     ROUND1,
//     CARD3,
//     ROUND2,
//     CARD4,
//     ROUND3,
//     CARD5,
//     ROUND4,
//     WINNER
// }
public class GameManager : MonoBehaviour
{ 
    string[] gameStates = new string[]{"DEALING", "BLINDS", "ROUND1", "CARD3", "ROUND2", "CARD4", "ROUND3", "CARD5", "ROUND4", "WINNER"};  
    Deck deck;
    Hand player; 
    public GameObject testObject;
    public GameObject restartButton;
    public GameObject checkButton;
    int index = 0;
    string currentState;
    int pot = 0;
    int bet = 0;
    List<Card> table = new List<Card>();
    void Start()
    {
        testObject = new GameObject();
        testObject.AddComponent<SpriteRenderer>();
        restartButton.SetActive(false);
        checkButton.SetActive(true);
        CardSprites.cardSprites = Resources.LoadAll<Sprite>("Cards");
        testObject.GetComponent<SpriteRenderer>().sprite = CardSprites.cardSprites[0];
        Destroy(testObject);
        deck = new Deck();
        player = new Hand();
        deck.shuffle();
        currentState = gameStates[0];
    }

    void Update()
    {
        switch(currentState)
        {
            case ("DEALING"):
                index = 0;
                pot = 0;
                Card a = deck.passCard();
                Card b = deck.passCard();
                player.takeCards(a,b);
                player.revealCards();
                currentState = gameStates[1];
                break;
            case ("BLINDS"):
                index =1;
                bet = 0;
                pot+=player.placeMoney(10);
                currentState = gameStates[2];
                break;
            case ("ROUND1"):
                index = 2;
                currentState = gameStates[3];
                break;
            case ("CARD3"):
                index = 3;
                for (int i = 0; i<3; i++)
                {
                    table.Add(deck.passCard());
                    table[i].sprite.transform.position = new Vector3(i-5,0,-i);
                }
                bet = 0;
                currentState = gameStates[4];
                break;
            case ("ROUND2"):
                index = 4;
                break;
            case ("CARD4"):
                index = 5;
                bet = 0;
                table.Add(deck.passCard());
                table[3].sprite.transform.position = new Vector3(-2,0,-3);
                currentState=gameStates[6];
                break;
            case ("ROUND3"):
                index = 6;
                break;
            case ("CARD5"):
                index = 7;
                bet = 0;
                table.Add(deck.passCard());
                table[4].sprite.transform.position = new Vector3(-1,0,-4);
                currentState = gameStates[8];
                break;
            case ("ROUND4"):
                index = 8;
                break;
            case ("WINNER"):
                index = 9;
                restartButton.SetActive(true);
                checkButton.SetActive(false);  
                break;
            
        }
    }

    public void check()
    {
        player.placeMoney(bet);
        pot = pot + bet;
        index++;
        Debug.Log(index);
        currentState = gameStates[index];
    }
    public void restart()
    {
        currentState = gameStates[0];
        pot = 0;
        player.clearHand();
        deck.newDeck();
        checkButton.SetActive(true);
        restartButton.SetActive(false);

    }
}

class Card
{
    public string suit;
    public int number;
    public GameObject sprite;
    public Card(string suit, int number, Sprite sprite)
    {
        this.suit = suit;
        this.number = number;
        this.sprite = new GameObject();
        this.sprite.AddComponent<SpriteRenderer>();
        this.sprite.GetComponent<SpriteRenderer>().sprite = sprite;
        this.sprite.transform.localScale = new Vector3(2,2,2);
    }
    public void deleet()
    {
        GameManager.Destroy(sprite);
    }
}

class Deck
{
    public List<Card> cards = new List<Card>();
    public Deck()
    {
        create();
    }

    public void create()
    {
        int j = 0;
        for (int z = 2; z< 15; z++)
        {
            for (int i = 0; i<4; i++)
            {
                if (i == 0)
                {
                    Sprite s = CardSprites.cardSprites[j];
                    Card c = new Card("Diamond", z, s);
                    cards.Add(c);
                    j++;
                }
                if (i == 1)
                {
                    Sprite s = CardSprites.cardSprites[j];
                    Card c = new Card("Clover", z, s);
                    cards.Add(c);
                    j++;
                }
                if (i == 2)
                {
                    Sprite s = CardSprites.cardSprites[j];
                    Card c = new Card("Heart", z, s);
                    cards.Add(c);
                    j++;
                }
                if (i == 3)
                {
                    Sprite s = CardSprites.cardSprites[j];
                    Card c = new Card("spades", z, s);
                    cards.Add(c);
                    j++;
                }

            }
        }
    }

    public void newDeck()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].deleet();
        }
        cards.Clear();
        create();
    }
    
    public Card passCard()
    {
        Card c = cards[0];
        cards.RemoveAt(0);
        return c;
    }

    public void shuffle()
    {
        for (int i = 0; i<52; i++)
        {
            Card temp = cards[i];
            int r = Random.Range(0,52);
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }
}

class Hand
{
    Card a;
    Card b;
    int money = 500;
    public Hand()
    {

    }

    public void takeCards(Card a, Card b)
    {
        this.a = a;
        this.b = b;
        this.a.sprite.transform.position = new Vector3(-0.25f,-3,0);
        this.b.sprite.transform.position = new Vector3(0.25f, -3,-1);
    }

    public void revealCards()
    {
        Debug.Log(this.a.suit);
        Debug.Log(this.a.number);
        Debug.Log(this.b.suit);
        Debug.Log(this.b.number);
    }

    public int placeMoney(int n)
    {
        this.money = this.money-n;
        return n;
    }
    public void clearHand()
    {
        this.a.deleet();
        this.b.deleet();
    }


}

