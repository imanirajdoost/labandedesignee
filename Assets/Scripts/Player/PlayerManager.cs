using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;

    [Header("Slow Motion")]
    [SerializeField] private float _slowMotionTime = 5;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void Start()
    {
        DialogManager.Instance.OnChoiceSelected += OnChoiceSelected;
        DialogManager.Instance.OnDialogPanelClosed += OnDialogPanelClosed;
    }

    private void OnDialogPanelClosed()
    {
        FreezePlayer(false);
    }

    private void OnDestroy()
    {
        DialogManager.Instance.OnChoiceSelected -= OnChoiceSelected;
        DialogManager.Instance.OnDialogPanelClosed -= OnDialogPanelClosed;
    }

    private void OnChoiceSelected(string obj)
    {
        FreezePlayer(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog"))
        {
            var dialogTrigger = other.gameObject.GetComponent<DialogTrigger>();
            if (dialogTrigger.IsAlreadyTriggered)
                return;

            dialogTrigger.SetAlreadyTriggered();
            DialogManager.Instance.ShowDialog(dialogTrigger.Index);


            if (dialogTrigger.FreezePlayer)
                FreezePlayer(true);

            if (dialogTrigger.SlowMotion)
            {
                StartCoroutine(SetSlowMotionFor(_slowMotionTime));
            }
            
        }
    }

    private IEnumerator SetSlowMotionFor(float t)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(t);
        Time.timeScale = 1;
    }

    private void FreezePlayer(bool freeze)
    {
        _movementController.SetEnabled(!freeze);
    }
}
