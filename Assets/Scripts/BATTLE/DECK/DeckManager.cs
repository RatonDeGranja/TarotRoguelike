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
    private void OnEnable()
    {
        GameEvents.onCardPlayed += OnCardPlayedLogic;
    }

    private void OnDisable()
    {
        GameEvents.onCardPlayed -= OnCardPlayedLogic;
    }

    private void OnCardPlayedLogic(Card playedCard)
    {
        if (hand.Contains(playedCard))
        {
            hand.Remove(playedCard);
            discardPile.Enqueue(playedCard);
            Debug.Log($"Carta descartada lógicamente. Cartas en descarte: {discardPile.Count}");
        }
    }

    public void InitializeDeck()
    {

        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();

        List<Card> tempDeck = new List<Card>(deck);

        // Barajar
        FisherYatesShuffle(tempDeck);

        // Llenar el mazo de robo
        foreach (Card card in tempDeck)
        {
            // Creamos un clon único e independiente en la memoria RAM
            Card uniqueCardInstance = Instantiate(card); 
            
            drawPile.Enqueue(uniqueCardInstance);
        }
    }

    // Esta funcion es para cuando se añade una carta a lamano de alguna forma que no sea robando
    public void AddGeneratedCardToHand(Card card)
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Mano llena. No cabe la copia.");
            return;
        }

        Card clonedCard = Instantiate(card);

        hand.Add(clonedCard);
        
        GameEvents.onCardDrawn?.Invoke(clonedCard); 
    }

    public void DrawStartingHand()
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
        // Avisamos de que una carta ha entrado a la mano
        GameEvents.onCardDrawn?.Invoke(drawnCard);
    }

    public bool DiscardCard(int index)
    {
        if (index >= 0 && index < hand.Count)
        {
            Debug.Log("Descartando");
            Card carta = hand[index];
            hand.RemoveAt(index);
            if (discardPile == null) Debug.LogError("¡La pila de descartes es null!");
            if (HandManager.Instance == null) Debug.LogError("¡El HandManager es null!");
            if (carta == null) Debug.LogError("¡La carta es null!");
            discardPile.Enqueue(carta);
            HandManager.Instance.OnCardPlayedVisual(carta);
            return true;
        }
        else
        {
            //Debug.Log("No hay cartas que descartar");
            return false;
        }

    }

    public void DiscardHand()
    {
        int contadorSeguridad = 0;
        while (hand.Count > 0)
        {
            DiscardCard(0);

            contadorSeguridad++;
            if (contadorSeguridad > 20) 
            {
                Debug.LogError("¡Bucle infinito evitado!");
                break; // Rompe el bucle a la fuerza
            }
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
