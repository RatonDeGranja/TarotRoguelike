using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Fíjate que hemos cambiado IPointerClick por IPointerDown e IPointerUp
public class MajorDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Datos lógicos")]
    private Major cardData; 
    public Major Data => cardData;

    [Header("Referencias Visuales")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image artImage;

    [Header("Ajustes de Animación")]
    [SerializeField] private float scaleSpeed = 15f;
    [SerializeField] private float hoverHeight = 40f;
    [SerializeField] private float hoverScale = 1.2f;

    private Vector3 basePosition;
    private Quaternion baseRotation = Quaternion.identity;
    
    private Vector3 targetPosition;
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 targetScale = Vector3.one;

    private void Start()
    {
        if (PlayerController.Player != null && PlayerController.Player.EquippedMajor != null)
        {
            Setup(PlayerController.Player.EquippedMajor);
        }
    }

    public void Setup(Card data)
    {
        cardData = data as Major;
        if (cardData != null)
        {
            nameText.text = cardData.CardName;
            descriptionText.text = cardData.CardDescription;
            artImage.sprite = cardData.CardArt;
        }
    }

    public void SetTargetTransform(Vector3 pos, Quaternion rot)
    {
        basePosition = pos;
        baseRotation = rot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleManager.Instance == null || BattleManager.Instance.State != BattleManager.BattleState.PLAYER_TURN) return;

        targetPosition = basePosition + new Vector3(0, hoverHeight, 0);
        targetRotation = Quaternion.identity; 
        targetScale = Vector3.one * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (BattleManager.Instance == null || BattleManager.Instance.State != BattleManager.BattleState.PLAYER_TURN) return;
        
        targetPosition = basePosition;
        targetRotation = baseRotation;
        targetScale = Vector3.one;
    }

}