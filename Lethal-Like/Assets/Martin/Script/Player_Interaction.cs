using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Transform cameraTransform;

    [Header("Interaction")]
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask obstructionLayer;

    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private float interactionWidth = 1f;
    [SerializeField] private float interactionHeight = 1f;

    [Header("Buttons")]
    [SerializeField] private LayerMask buttonLayer;

    [Header("Drop")]
    [SerializeField] private float dropForce = 4f;

    private Player_InputRecive inputs;
    private Rigidbody playerRb;

    private Item heldItem;
    private Rigidbody heldRb;

    private void Awake()
    {
        inputs = GetComponent<Player_InputRecive>();
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        {
            if (inputs.hasDroped && heldItem != null)
            {
                DropObject();
                return;
            }

            if (inputs.hasInteracted)
            {
                if (TryPressButton()) return;

                if (heldItem == null) PickUpObject();
                else UseItem();
            }
        }
    }

    private void PickUpObject()
    {
        Vector3 center = cameraTransform.position + cameraTransform.forward * (interactionDistance * 0.5f);

        Vector3 halfExtents = new Vector3(interactionWidth * 0.5f, interactionHeight * 0.5f, interactionDistance * 0.5f);

        Collider[] hits = Physics.OverlapBox(center, halfExtents, cameraTransform.rotation, itemLayer, QueryTriggerInteraction.Ignore);

        Item closestItem = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            Item item = hit.GetComponentInParent<Item>();

            if (item == null) continue;

            Rigidbody itemRb = item.GetComponent<Rigidbody>();

            if (itemRb == null) continue;

            Vector3 closestPoint = hit.ClosestPoint(cameraTransform.position);

            if (Physics.Linecast(cameraTransform.position, closestPoint, obstructionLayer, QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            float distance = Vector3.SqrMagnitude( closestPoint - cameraTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = item;
            }
        }

        if (closestItem == null) return;

        heldItem = closestItem;
        heldRb = heldItem.GetComponent<Rigidbody>();

        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;

        heldRb.isKinematic = true;
        heldRb.detectCollisions = false;

        heldItem.transform.SetParent(itemHolder);

        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
    }

    private void DropObject()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null, true);

        heldRb.isKinematic = false;
        heldRb.detectCollisions = true;

        heldRb.linearVelocity = playerRb.linearVelocity;

        heldRb.AddForce(cameraTransform.forward * dropForce, ForceMode.Impulse);

        heldItem = null;
        heldRb = null;
    }

    private void UseItem()
    {
        if (heldItem == null) return;

        heldItem.Use();
    }

    private bool TryPressButton()
    {
        int mask = buttonLayer.value | obstructionLayer.value;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance, mask, QueryTriggerInteraction.Ignore))
        {
            Sell_Button button = hit.collider.GetComponentInParent<Sell_Button>();

            if (button != null)
            {
                button.Interact();
                return true;
            }
        }

        return false;
    }
}
