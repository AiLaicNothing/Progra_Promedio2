
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class Pickup_UI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemValueText;

    [Header("Settings")]
    [SerializeField] private float showDistance = 4f;
    [SerializeField] private bool faceCamera = true;

    private Canvas canvas;
    private Item item;
    private Rigidbody itemRb;

    private Transform player;
    private Camera playerCamera;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        item = GetComponentInParent<Item>();

        if (item != null) itemRb = item.GetComponent<Rigidbody>();

        canvas.enabled = false;
    }

    private void Start()
    {
        Player_Controller playerController = FindFirstObjectByType<Player_Controller>();

        if (playerController != null) player = playerController.transform;

        playerCamera = Camera.main;

        if (item != null)
        {
            itemNameText.text = item.GetItemName();
            itemValueText.text = "Value: $" + item.GetItemValue();
        }
    }

    private void LateUpdate()
    {
        if (player == null || item == null)
        {
            canvas.enabled = false;
            return;
        }

        float distance = Vector3.Distance(player.position, item.transform.position);

        bool isHeld = itemRb != null && itemRb.isKinematic;

        canvas.enabled = distance <= showDistance && !isHeld;

        if (canvas.enabled && faceCamera && playerCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position, Vector3.up);
        }
    }
}
