using System;
using UnityEngine;

public class DialogTriggerMultipleChoice : DialogTrigger
{
    [SerializeField] private GameObject _bubbleSoftFly;
    [SerializeField] private GameObject _bubbleAggressiveAttack;
    [SerializeField] private GameObject _bubbleColdCreatePlatform;

    [SerializeField] private GameObject _bubbleSoftFlyLong;
    [SerializeField] private GameObject _bubbleAggressiveAttackLong;
    [SerializeField] private GameObject _bubbleColdCreatePlatformLong;

    [SerializeField] private Transform _softTarget;
    [SerializeField] private Transform _coldSpawnTarget;
    [SerializeField] private GameObject _objectToDestroy;


    [SerializeField] private bool _spawnLong = false;

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
        GameObject obj = null;
        if(!_spawnLong)
            obj = Instantiate(_bubbleSoftFly, transform.position, Quaternion.identity);
        else
            obj = Instantiate(_bubbleSoftFlyLong, transform.position, Quaternion.identity);

        obj.GetComponent<BubbleSoft>().SetTarget(_softTarget);
        obj.GetComponent<BubbleBase>().DoYourThing();

        _playerManager.SetAnimation("SOFT");
    }

    private void CreateAggressive()
    {
        GameObject obj = null;
        if (!_spawnLong)
            obj = Instantiate(_bubbleAggressiveAttack, transform.position, Quaternion.identity);
        else
            obj = Instantiate(_bubbleAggressiveAttackLong, transform.position, Quaternion.identity);

        obj.GetComponent<BubbleBase>().DoYourThing();

        Destroy(_objectToDestroy);

        // TODO: Set player animation
    }

    private void CreateCold()
    {
        GameObject obj = null;
        if (!_spawnLong)
            obj = Instantiate(_bubbleColdCreatePlatform, _coldSpawnTarget.transform.position, Quaternion.identity);
        else
            obj = Instantiate(_bubbleColdCreatePlatformLong, _coldSpawnTarget.transform.position, Quaternion.identity);

        obj.GetComponent<BubbleBase>().DoYourThing();
    }
}
