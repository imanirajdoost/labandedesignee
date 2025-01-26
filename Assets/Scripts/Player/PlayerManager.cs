using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private MovementController _movementController;

    [Header("Slow Motion")]
    [SerializeField] private float _slowMotionTime = 5;

    [SerializeField] private Animator _animator;

    private Collider col;

    private Rigidbody rb;

    private Coroutine _slowMotionCoroutine;

    [Header("End Game")]
    [SerializeField] private GameObject _entity;
    [SerializeField] private GameObject _bubbleAggressiveAttack;

    private int _currentStep = 0;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();

        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
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

    public void SetAnimation(string animation)
    {
        switch(animation)
        {
            case "SOFT":
                _animator.SetBool("Fly", true);
                break;
            case "AGGR":
                _animator.SetTrigger("Aggressive");
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

            if (!dialogTrigger.IsEnabled)
                return;

            if (dialogTrigger.TriggerCount > 0 && dialogTrigger.ShouldTriggerOnce)
                return;

            dialogTrigger.SetTriggered();

            if (dialogTrigger.FreezePlayer)
                FreezePlayer(true);

            if (dialogTrigger.SlowMotion)
            {
                if (_slowMotionCoroutine != null)
                    StopCoroutine(_slowMotionCoroutine);

                _slowMotionCoroutine = StartCoroutine(SetSlowMotionFor(_slowMotionTime));
            }
            
        } else if(other.CompareTag("EndLevel"))
        {
            // Go To Next Level
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if(currentIndex != 4)
                SceneManager.LoadScene(currentIndex + 1);
            else
            {
                StartCoroutine(StartEndGameCinematic());
            }
        }
    }

    private IEnumerator StartEndGameCinematic()
    {
        Debug.Log("End Game Cinematic");
        _movementController.SetEnabled(false);
        _movementController.EnableCompletely = false;

        _movementController.LookRight();

        yield return new WaitForSeconds(1.5f);

        // entity moves away

        _entity.transform.DOLocalMoveX(5f, 2f).SetEase(Ease.Linear);

        // Show 1st dialog
        DialogManager.Instance.OnChoiceSelected += OnChoiceSelected;

        _currentStep = 1;
        DialogManager.Instance.ShowDialogWithoutUnfreezing(22);

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator ContinueDialog1()
    {
        yield return new WaitForSeconds(1f);
        _currentStep = 2;
        DialogManager.Instance.ShowDialogWithoutUnfreezing(23);
    }

    private IEnumerator ContinueDialog2()
    {
        yield return new WaitForSeconds(1f);
        _currentStep = 3;
        DialogManager.Instance.ShowDialogWithoutUnfreezing(24);
    }

    private IEnumerator ContinueDialog3()
    {
        yield return new WaitForSeconds(1f);
        _currentStep = 4;
        DialogManager.Instance.ShowDialogWithoutUnfreezing(25);
    }

    private IEnumerator ContinueDialog4()
    {
        yield return new WaitForSeconds(1f);
        _currentStep = 4;
        DialogManager.Instance.ShowDialogWithoutUnfreezing(25);
    }

    private void OnChoiceSelected(string tone)
    {
        switch(_currentStep)
        {
            case 1:
                CreateAggressive();
                StartCoroutine(ContinueDialog1());
                break;
            case 2:
                CreateAggressive();
                StartCoroutine(ContinueDialog2());
                break;
            case 3:
                CreateAggressive();
                StartCoroutine(ContinueDialog3());
                break;
            case 4:
                if (tone == "AGGR")
                {
                    CreateAggressive(true, true);
                    StartCoroutine(ContinueDialog4());
                } 
                else if(tone == "SOFT")
                {

                }
                else if(tone == "COLD")
                {

                }
                break;
                default:
                break;
        }
    }

    private void CreateAggressive(bool overrideTime = false, bool shoulDestroy = false)
    {
            GameObject obj = Instantiate(_bubbleAggressiveAttack,
                new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
                Quaternion.identity);
        if(overrideTime)
            obj.GetComponent<BubbleAggressive>().OverrideTime(4);

        obj.GetComponent<BubbleAggressive>().SetDestroyObject(shoulDestroy);
        obj.GetComponent<BubbleAggressive>().SetTarget(_entity);
        obj.GetComponent<BubbleBase>().DoYourThing();

        SetAnimation("AGGR");
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

    public void OnCreatedPlatform(GameObject targetPlatform)
    {
        ForceAttach();
        // move the player to the platform
        transform.position = targetPlatform.transform.position;
        // set the player to climb
        _animator.SetTrigger("Climb");

        StartCoroutine(WaitForClimbAnim(targetPlatform));
    }

    private IEnumerator WaitForClimbAnim(GameObject targetPlatform)
    {
        yield return new WaitForSeconds(2.167f);
        // put player up the platform
        transform.position = new Vector3(targetPlatform.transform.position.x, targetPlatform.transform.position.y + 1f, targetPlatform.transform.position.z);
        ForceDetach();
    }

    public void OnPlatformDestroyed()
    {
        _movementController.SetSlowMotion(false);
        if (_slowMotionCoroutine != null)
            StopCoroutine(_slowMotionCoroutine);

        _movementController.SetEnabled(false);
    }
}
