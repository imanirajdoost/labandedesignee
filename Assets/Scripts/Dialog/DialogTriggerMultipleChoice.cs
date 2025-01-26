using System;
using UnityEngine;

public class DialogTriggerMultipleChoice : DialogTrigger
{
    [SerializeField] private GameObject _bubbleSoftFly;
    [SerializeField] private GameObject _bubbleAggressiveAttack;
    [SerializeField] private GameObject _bubbleColdCreatePlatform;

    [SerializeField] private Transform _softTarget;

    [SerializeField] private PlayerManager _playerManager;


    private void Start()
    {
        DialogManager.Instance.OnChoiceSelected += OnChoiceSelected;
    }

    private void OnDestroy()
    {
        DialogManager.Instance.OnChoiceSelected -= OnChoiceSelected;
    }

    private void OnChoiceSelected(string tone)
    {
        DoYourThing(tone);
    }

    private void Awake()
    {
        _playerManager = FindFirstObjectByType<PlayerManager>();
    }

    private void DoYourThing(string type)
    {
        // Check if player is near
        if (Vector3.Distance(transform.position, _playerManager.transform.position) > 2)
            return;

        switch (type)
        {
            case "SOFT":
                CreateSoft();
                break;
            case "AGGR":

                CreateAggressive();
                break;

                case "COLD":
                CreateCold();
                break;
            default:
                break;
        }
    }

    private void CreateSoft()
    {
        var obj = Instantiate(_bubbleSoftFly, transform.position, Quaternion.identity);
        obj.GetComponent<BubbleSoft>().SetTarget(_softTarget);
        obj.GetComponent<BubbleBase>().DoYourThing();

        _playerManager.SetAnimation("SOFT");
    }

    private void CreateAggressive()
    {
        var obj = Instantiate(_bubbleAggressiveAttack, transform.position, Quaternion.identity);
        obj.GetComponent<BubbleBase>().DoYourThing();
        // TODO: Set player animation
    }

    private void CreateCold()
    {
        var obj = Instantiate(_bubbleColdCreatePlatform, transform.position, Quaternion.identity);
        obj.GetComponent<BubbleBase>().DoYourThing();
    }
}
