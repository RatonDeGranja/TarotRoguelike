using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<Card> deck; // Asigna ScriptableObjects aquí
    private Queue<Card> drawPile = new Queue<Card>();
    private Queue<Card> discardPile = new Queue<Card>();
    private List<Card> hand = new List<Card>();


    [SerializeField] private int startingHandSize = 5;
    [SerializeField] int maxHandSize;
    [SerializeField] private int currentHandSize;
    
    public static DeckManager Instance; // <- esta es la clave

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //InitializeDeck();
        //DrawStartingHand();
    }


    public void InitializeDeck()
    {
        List<Card> tempDeck = new List<Card>(deck);

        // Barajar
        FisherYatesShuffle(tempDeck);

        // Llenar el mazo de robo
        foreach (Card card in tempDeck)
        {
            drawPile.Enqueue(card);
            
        }
    }

    void DrawStartingHand()
    {
        for (int i = 0; i < startingHandSize; i++)
            DrawCard();
    }
    /*
     * Enqueue(x) → Agrega al final

        Dequeue() → Saca y devuelve el primer elemento

        Peek() → Mira el primero sin sacarlo
    */

    public void DrawCard()
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Mano llena. No puedes robar más.");
            return;
        }

        // Si no hay cartas para robar, reciclamos el descarte
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0)
            {
                Debug.Log("No hay cartas en el mazo ni en el descarte.");
                return; 
            }
            RecycleDiscardPile();
        }

        Card drawnCard = drawPile.Dequeue();
        hand.Add(drawnCard);

        // Avisamos de que una carta ha entrado a la mano
        GameEvents.onCardDrawn?.Invoke(drawnCard);
    }

    public bool DiscardCard(int index)
    {
        if (currentHandSize >= 1)
        {
            currentHandSize--;
            //Debug.Log("Descartando");
            Card carta = hand[index];
            hand.RemoveAt(index);
            discardPile.Enqueue(carta);
            return true;
        }
        else
        {
            //Debug.Log("No hay cartas que descartar");
            return false;
        }

    }

    private void RecycleDiscardPile()
    {
        List<Card> tempDiscard = new List<Card>(discardPile);
        discardPile.Clear();
        
        FisherYatesShuffle(tempDiscard);

        foreach (Card card in tempDiscard)
        {
            drawPile.Enqueue(card);
        }
        
        Debug.Log("Mazo de descarte barajado en el mazo de robo.");
    }

    public static void FisherYatesShuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // desde 0 hasta i, inclusivo
            (list[i], list[j]) = (list[j], list[i]); // swap
        }
    }

    public bool HasCards()
    {
        return drawPile.Count > 0;
    }

    public Queue<Card> getDeck()
    {
        return drawPile;
    }

    public Queue<Card> Deck => drawPile;
    public Queue<Card> Discard => discardPile;
    public List<Card> Hand => hand;
    public int HandSize => currentHandSize;
    public int MaxHandSize => maxHandSize;
}
