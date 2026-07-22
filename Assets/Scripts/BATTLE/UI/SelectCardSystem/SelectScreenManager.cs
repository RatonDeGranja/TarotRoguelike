using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScreenManager : MonoBehaviour
{
    public static SelectScreenManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private GameObject visualPanel; // El panel opaco que tapa el juego
    
    [Header("Card Positioning")]
    [SerializeField] private Transform cardsContainer; // El LayoutGroup donde salen las opciones
    [SerializeField] private Transform centerSlot;     // El hueco en el centro de la pantalla
    [SerializeField] private GameObject cardPrefab;    // El prefab visual de la carta

    [Header("Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button hideButton;

    // --- VARIABLES INTERNAS ---
    private Action<Card> onConfirmCallback; 
    private CardDisplay currentlySelectedVisual; // El script visual de la carta seleccionada
    private Card currentlySelectedData;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        screenPanel.SetActive(false);
        
        // Configuramos los botones desde aquí
        confirmButton.onClick.AddListener(ConfirmSelection);
        hideButton.onClick.AddListener(ToggleVisibility);
    }


    /// Esta funcion de llama desde cualquier carta que necesite que el jugador elija algo.

    public void OpenSelection(List<Card> cardsToChoose, Action<Card> callback)
    {
        // 1. Guardamos qué hay que hacer cuando el jugador confirme
        onConfirmCallback = callback;
        BattleManager.Instance.state = BattleState.SELECTING_CARD;
        // 2. Limpiamos selecciones anteriores
        ClearContainer();
        currentlySelectedVisual = null;
        currentlySelectedData = null;
        confirmButton.interactable = false; // Bloqueamos el botón de confirmar

        // 3. Generamos las cartas para elegir
        foreach (Card cardData in cardsToChoose)
        {
            GameObject newCardObj = Instantiate(cardPrefab, cardsContainer);
            // Asumiendo que tu prefab tiene un script CardVisual (o CardDisplay)
            CardDisplay visual = newCardObj.GetComponent<CardDisplay>();
            visual.Setup(cardData);
            
            // Le decimos al visual que cuando le hagan clic, nos avise
            visual.GetComponent<Button>().onClick.AddListener(() => OnCardClicked(visual, cardData));
        }

        // 4. Mostramos la pantalla
        screenPanel.SetActive(true);
        visualPanel.SetActive(true);
    }

    // --- LÓGICA DE SELECCIÓN ---

    private void OnCardClicked(CardDisplay clickedVisual, Card clickedData)
    {
        // Si ya había una carta seleccionada, la devolvemos a la lista (deselect)
        if (currentlySelectedVisual != null)
        {
            currentlySelectedVisual.transform.SetParent(cardsContainer);
            // Aquí puedes quitarle el brillo de selección si lo tiene
        }

        // Actualizamos la nueva selección
        currentlySelectedVisual = clickedVisual;
        currentlySelectedData = clickedData;

        // La movemos al centro
        currentlySelectedVisual.transform.SetParent(centerSlot);
        currentlySelectedVisual.transform.localPosition = Vector3.zero; 
        
        // ¡Ya podemos confirmar!
        confirmButton.interactable = true;
    }

    private void ConfirmSelection()
    {
        if (currentlySelectedData != null)
        {
            // Ejecutamos el efecto específico que nos pidieron al abrir la pantalla
            onConfirmCallback?.Invoke(currentlySelectedData);
            
            // Cerramos y limpiamos
            screenPanel.SetActive(false);
            BattleManager.state = BattleState.SELECTING_CARD;
            ClearContainer();
        }
    }

    // --- LÓGICA DE OCULTAR/VER CAMPO ---

    private void ToggleVisibility()
    {
        // Apaga o enciende el panel visual, pero mantiene el mánager activo
        visualPanel.SetActive(!visualPanel.activeSelf);
    }

    private void ClearContainer()
    {
        foreach (Transform child in cardsContainer) Destroy(child.gameObject);
        foreach (Transform child in centerSlot) Destroy(child.gameObject);
    }
}