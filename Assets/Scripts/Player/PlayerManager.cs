using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private MovementController _movementController;

    [Header("Slow Motion")]
    [SerializeField] private float _slowMotionTime = 5;

    [SerializeField] private Animator _animator;

    private Collider col;

    private Rigidbody rb;

    private Coroutine _slowMotionCoroutine;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();

        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
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

    private void OnChoiceSelected(string tone)
    {
        //FreezePlayer(false);
        switch(tone)
        {
            case "SOFT":
                //Fly();
                break;
            default:
                break;
        }
    }

    public void SetAnimation(string animation)
    {
        switch(animation)
        {
            case "SOFT":
                _animator.SetBool("Fly", true);
                break;
            default:
                break;
        }
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

    public void SetFreeze(bool freeze)
    {
        FreezePlayer(freeze);
    }

    public void ForceAttach()
    {
        _movementController.SetEnabled(false);
        _movementController.SetSlowMotion(false);
        if (_slowMotionCoroutine != null)
            StopCoroutine(_slowMotionCoroutine);

        // Disable collider and rb for the fly time
        col.enabled = false;
        rb.isKinematic = true;
    }

    public void ForceDetach()
    {
        col.enabled = true;
        rb.isKinematic = false;
        _animator.SetBool("Fly", false);
        _movementController.SetEnabled(true);
    }
}
