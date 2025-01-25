using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog"))
        {
            DialogManager.Instance.ShowNextDialog();
        }
    }
}
