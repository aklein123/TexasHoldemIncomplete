using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class CardSprites
{
    public static Sprite[] cardSprites;
    public static Sprite[] chipSprites;
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
    public GameObject inputField;
    public GameObject betButton;
    List<Card> table = new List<Card>();
    public Text moneyText;
    public Text potText;
    void Start()
    {
        testObject = new GameObject();
        testObject.AddComponent<SpriteRenderer>();
        restartButton.SetActive(false);
        checkButton.SetActive(true);
        CardSprites.cardSprites = Resources.LoadAll<Sprite>("Cards");
        CardSprites.chipSprites = Resources.LoadAll<Sprite>("Chip Assets");
        testObject.GetComponent<SpriteRenderer>().sprite = CardSprites.cardSprites[0];
        Destroy(testObject);
        deck = new Deck();
        player = new Hand();
        deck.shuffle();
        moneyText.text = player.money.ToString();
        potText.text = pot.ToString();
        currentState = gameStates[0];
    }

    void Update()
    {
        switch(currentState)
        {
            case ("DEALING"):
                deck.shuffle();
                deck.shuffle();
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
                showTurnButtons();
                moneyText.text = "Your Money:" + player.money.ToString();
                potText.text = "Pot:" + pot.ToString();
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
                showTurnButtons();
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
                showTurnButtons();
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
                showTurnButtons();
                currentState = gameStates[8];
                break;
            case ("ROUND4"):
                index = 8;
                break;
            case ("WINNER"):
                index = 9;
                restartButton.SetActive(true);
                checkButton.SetActive(false);  
                checkTwoPair(playerHandTable(table, player));
                
                break;
            
        }
    }

    public void check()
    {
        player.placeMoney(bet);
        pot = pot + bet;
        index++;
        Debug.Log(index);
        moneyText.text = "Your Money:" + player.money.ToString();
        potText.text = "Pot:" + pot.ToString();
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
        clearTable();
    }
    public void clearTable()
    {
        for (int i = 0; i< table.Count; i++)
        {
            table[i].deleet();
        }
        table.Clear();
    }
    public void betChange(string newBetInput)
    {
        Debug.Log(newBetInput);
        int betInput = System.Int32.Parse(newBetInput);
        inputField.SetActive(false);
        bet = bet + betInput;
        check();
        Debug.Log(bet);
    }
    public void hideBetButton()
    {
        betButton.SetActive(false);
    }
    public void showTurnButtons()
    {
        betButton.SetActive(true);
        inputField.SetActive(true);
    }
    bool checkPair(List<Card> cards)
    {  
      for (int i = cards.Count-1; i>=0; i--)
      {
        if (cards[i].number == cards[i+1].number)
        {
            Debug.Log("You Have A Pair");
            return true;
        }
      } 
      return false;
    }
    bool checkTwoPair(List<Card> cards)
    {
        int var = 0;
        for (int i = cards.Count-1; i>=0; i--)
        {
            if (cards[i].number == cards[i+1].number)
            {
                var = cards[i].number;
            }
        } 
        for (int i = cards.Count-1; i>=0; i--)
        {
            if (cards[i].number == cards[i+1].number && cards[i].number != var)
            {
                Debug.Log("You have a two-pair");
                return true;
            }
        } 
        return false;
    }
    List<Card> playerHandTable(List<Card> cards, Hand player)
    {
        cards.Add(player.a);
        cards.Add(player.b);
        cards = sortCards(cards);
        return cards;
    }
    List<Card> sortCards(List<Card> cards)
    {
        for (int z = 0; z<cards.Count; z++)
        {    
            for (int i = 0; i < cards.Count-1-z; i++)
            {
                if (cards[i].number > cards[i+1].number)
                {
                    Card temp = cards[i+1];
                    cards[i+1] = cards[i];
                    cards[i] = temp;
                }
            } 
        }

        return cards;
    }
    //Function for two pair, three of a kind
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

class Money
{
    public GameObject sprite;
    public Money()
    {
        this.sprite = new GameObject();
        this.sprite.AddComponent<SpriteRenderer>();
        this.sprite.GetComponent<SpriteRenderer>().sprite = CardSprites.chipSprites[9];

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
    public Card a;
    public Card b;
    public int money = 500;
    Money handMoney = new Money();
    
    public Hand()
    {
        handMoney.sprite.transform.position = new Vector3(2.6f, -3, 0);
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

