using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Fíjate que hemos cambiado IPointerClick por IPointerDown e IPointerUp
public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Datos lógicos")]
    private Minor cardData; 
    public Minor CardData => cardData;

    [Header("Referencias Visuales")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image artImage;

    [Header("Ajustes de Animación")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float scaleSpeed = 15f;
    [SerializeField] private float hoverHeight = 40f;
    [SerializeField] private float hoverScale = 1.2f;

    private Vector3 basePosition;
    private Quaternion baseRotation = Quaternion.identity;
    
    private Vector3 targetPosition;
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 targetScale = Vector3.one;

    private int originalSiblingIndex;
    
    private bool isDragging = false; 
    private bool wasDragged = false; // <- NUEVO: Para saber si movimos el ratón

    private void OnEnable()
    {
        GameEvents.onLanguageChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        GameEvents.onLanguageChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        if (cardData != null)
        {
            nameText.text = cardData.CardName;
            descriptionText.text = cardData.GetFormattedDescription();;
            artImage.sprite = cardData.CardArt;
            if (costText != null) costText.text = cardData.WisdomCost.ToString();
        }
    }

    public void Setup(Card data)
    {
        cardData = data as Minor;
        if (cardData != null)
        {
            nameText.text = cardData.CardName;
            descriptionText.text = cardData.GetFormattedDescription();;
            artImage.sprite = cardData.CardArt;
            if (costText != null) costText.text = cardData.WisdomCost.ToString();
        }
    }

    private void Update()
    {
        bool isSelected = false;
        if (BattleManager.Instance != null)
        {
            isSelected = (BattleManager.Instance.State == BattleManager.BattleState.WAITING_TARGET && BattleManager.Instance.PendingCard == cardData);
        }

        if (!isDragging)
        {
            Vector3 finalPos = isSelected ? basePosition + new Vector3(0, hoverHeight, 0) : targetPosition;
            Quaternion finalRot = isSelected ? Quaternion.identity : targetRotation;
            Vector3 finalScale = isSelected ? Vector3.one * hoverScale : targetScale;

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * moveSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, finalRot, Time.deltaTime * rotationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void SetTargetTransform(Vector3 pos, Quaternion rot)
    {
        basePosition = pos;
        baseRotation = rot;

        if (!isDragging)
        {
            targetPosition = basePosition;
            targetRotation = baseRotation;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleManager.Instance == null || BattleManager.Instance.State != BattleManager.BattleState.PLAYER_TURN || isDragging) return;

        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        targetPosition = basePosition + new Vector3(0, hoverHeight, 0);
        targetRotation = Quaternion.identity; 
        targetScale = Vector3.one * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (BattleManager.Instance == null || BattleManager.Instance.State != BattleManager.BattleState.PLAYER_TURN || isDragging) return;

        transform.SetSiblingIndex(originalSiblingIndex);
        
        targetPosition = basePosition;
        targetRotation = baseRotation;
        targetScale = Vector3.one;
    }

    // --- MAGIA DEL CLIC ---

    public void OnPointerDown(PointerEventData eventData)
    {
        wasDragged = false; // Al pulsar, asumimos que va a ser un clic
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Si levantamos el botón y el ratón NO fue arrastrado, es un clic puro
        if (eventData.button == PointerEventData.InputButton.Left && !wasDragged)
        {
            if (BattleManager.Instance != null && BattleManager.Instance.State == BattleManager.BattleState.PLAYER_TURN)
            {
                Debug.Log("¡Clic detectado en la carta!");
                BattleManager.Instance.PlayCard(cardData);
            }
        }
    }

    // --- MAGIA DEL DRAG & DROP ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BattleManager.Instance == null || BattleManager.Instance.State != BattleManager.BattleState.PLAYER_TURN) return;

        isDragging = true;
        wasDragged = true; // Como se movió el ratón, cancelamos el "clic puro"
        
        transform.SetAsLastSibling(); 
        targetRotation = Quaternion.identity;
        targetScale = Vector3.one * 0.8f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = eventData.position; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Entra en OnEndDrag");
        if (!isDragging) return;
        isDragging = false;

        bool cardWasPlayed = false;

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (cardData.Target == TargetType.ToEnemy)
        {
            if (hit.collider != null)
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    BattleManager.Instance.PlayCard(cardData); 
                    BattleManager.Instance.OnEnemyClicked(enemy); 
                    cardWasPlayed = true;
                }
            }
        }
        else if (cardData.Target == TargetType.ToPlayer)
        {
            if (eventData.position.y > Screen.height * 0.35f) 
            {
                BattleManager.Instance.PlayCard(cardData); 
                cardWasPlayed = true;
            }
        }

        if (!cardWasPlayed)
        {
            transform.SetSiblingIndex(originalSiblingIndex);
            targetPosition = basePosition;
            targetRotation = baseRotation;
            targetScale = Vector3.one;
        }
    }
}