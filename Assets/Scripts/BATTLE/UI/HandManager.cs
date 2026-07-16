using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // No necesitamos un Singleton aquí, el gestor vive y muere en la escena de combate
    
    public static HandManager Instance;

    [Header("Referencias Visuales")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handParent;

    [Header("Ajustes del Abanico (Curva)")]
    [SerializeField] private float handWidth = 100; // Ancho máximo que puede ocupar toda la mano
    [SerializeField] private float cardSpacing = 5f; // Espacio ideal entre cada carta
    [SerializeField] private float curveHeight = 1f; // Cuánto se elevan las cartas centrales (el arco)
    [SerializeField] private float maxCardRotation = 12f; // Ángulo máximo al que se inclinan las de los extremos

    // Lista puramente visual de los objetos que existen en el Canvas
    private List<CardDisplay> visualCards = new List<CardDisplay>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.onCardDrawn += OnCardDrawnVisual;
        GameEvents.onCardPlayed += OnCardPlayedVisual;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar si este script se apaga
        GameEvents.onCardDrawn -= OnCardDrawnVisual;
        GameEvents.onCardPlayed -= OnCardPlayedVisual;
    }

    private void OnCardDrawnVisual(Card drawnCardLogic)
    {
        // 1. Nace el "cartón" en blanco en la pantalla
        GameObject newCardGO = Instantiate(cardPrefab, handParent);
        
        // 2. Le inyectamos los datos del ScriptableObject para que dibuje a la Espada, la Vida, etc.
        CardDisplay display = newCardGO.GetComponent<CardDisplay>();
        display.Setup(drawnCardLogic); // (Asumiendo que mantienes este método en CardDisplay)
        
        // 3. Lo añadimos a nuestra lista visual
        visualCards.Add(display);

        // 4. Recalculamos el abanico
        UpdateHandLayout();
    }

    public void OnCardPlayedVisual(Card playedCardLogic)
    {
        // Buscamos cuál es el "cartón" en pantalla que tiene esta lógica
        CardDisplay cardToDiscard = visualCards.Find(c => c.CardData == playedCardLogic);
        if (cardToDiscard != null)
        {
            RemoveCardVisual(cardToDiscard); // Esto ya destruye el objeto y reordena el abanico
        }
    }

    private void RemoveCardVisual(CardDisplay cardToRemove)
    {
        // Este método lo llamaremos cuando el jugador juegue o descarte la carta
        if (visualCards.Contains(cardToRemove))
        {
            visualCards.Remove(cardToRemove);
            Destroy(cardToRemove.gameObject);
            
            // Al borrar una, las demás se reajustan solas para tapar el hueco
            UpdateHandLayout();
        }
    }

    

    // La matemática pura del abanico de cartas
    private void UpdateHandLayout()
    {
        int cardCount = visualCards.Count;
        if (cardCount == 0) return;

        // Calculamos el ancho real que van a ocupar (para que si hay 2 cartas, no se vayan a los bordes de la pantalla)
        float currentWidth = Mathf.Min(handWidth, (cardCount - 1) * cardSpacing);
        float startX = -currentWidth / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            // NormalizedPosition nos da un valor de 0 a 1 (0 = carta más a la izquierda, 0.5 = centro, 1 = derecha)
            float normalizedPosition = cardCount > 1 ? (float)i / (cardCount - 1) : 0.5f;

            // X: Posición horizontal
            float xPos = startX + (i * (currentWidth / Mathf.Max(1, cardCount - 1)));
            if (cardCount == 1) xPos = 0f; // Si solo hay una, va al centro absoluto

            // Y: Ecuación para el arco. Las del centro suben, las de los bordes bajan.
            float distanceFromCenter = normalizedPosition - 0.5f; 
            float yPos = (1f - (Mathf.Abs(distanceFromCenter) * 2f)) * curveHeight;

            // Rotación: Las de la izquierda miran a la derecha, las de la derecha miran a la izquierda
            float zRot = -distanceFromCenter * maxCardRotation * 2f;

            // Aplicamos todo al transform de la carta en la interfaz
            visualCards[i].SetTargetTransform(new Vector3(xPos, yPos, 0f), Quaternion.Euler(0f, 0f, zRot));
            
            // Garantizamos que la carta de la derecha tape visualmente a la de la izquierda (como en la vida real)
            visualCards[i].transform.SetSiblingIndex(i);
        }
    }
}