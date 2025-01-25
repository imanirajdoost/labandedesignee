using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;

    [Header("Slow Motion")]
    [SerializeField] private float _slowMotionTime = 5;

    private Coroutine _slowMotionCoroutine;

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
            if (dialogTrigger.TriggerCount > 0 && dialogTrigger.ShouldTriggerOnce)
                return;

            dialogTrigger.SetTriggered();
            DialogManager.Instance.ShowDialog(dialogTrigger.Index);


            if (dialogTrigger.FreezePlayer)
                FreezePlayer(true);

            if (dialogTrigger.SlowMotion)
            {
                if (_slowMotionCoroutine != null)
                    StopCoroutine(_slowMotionCoroutine);

                _slowMotionCoroutine = StartCoroutine(SetSlowMotionFor(_slowMotionTime));
            }
            
        }
    }

    public void ForceStopSlowMotion()
    {
        if(_slowMotionCoroutine != null)
            StopCoroutine(_slowMotionCoroutine);
        _movementController.SetSlowMotion(false);
    }

    private IEnumerator SetSlowMotionFor(float t)
    {
        _movementController.SetSlowMotion(true);
        yield return new WaitForSecondsRealtime(t);
        _movementController.SetSlowMotion(false);
    }

    private void FreezePlayer(bool freeze)
    {
        _movementController.SetEnabled(!freeze);
    }
}
