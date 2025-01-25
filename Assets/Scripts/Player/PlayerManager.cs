using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog"))
        {
            var dialogTrigger = other.gameObject.GetComponent<DialogTrigger>();
            if (dialogTrigger.FreezePlayer)
                FreezePlayer();
            DialogManager.Instance.ShowDialog(dialogTrigger.Index);
        }
    }

    private void FreezePlayer()
    {
        _movementController.SetEnabled(false);
    }
}
