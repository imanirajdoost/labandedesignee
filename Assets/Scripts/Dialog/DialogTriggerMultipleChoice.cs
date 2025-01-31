using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private float _disableTime = 5f;


    [SerializeField] private bool _spawnLong = false;

    [SerializeField] private PlayerManager _playerManager;

    private Coroutine _disableCoroutine;


    private void Start()
    {
        DialogManager.Instance.OnChoiceSelected += OnChoiceSelected;
    }

    private void OnDestroy()
    {
        DialogManager.Instance.OnChoiceSelected -= OnChoiceSelected;
    }

    private IEnumerator DisableFor()
    {
        _isEnabled = false;
        yield return new WaitForSeconds(_disableTime);
        _isEnabled = true;
    }

    public override void SetTriggered()
    {
        base.SetTriggered();

        if(_isEnabled == false)
            return;

        if (_disableCoroutine != null)
            StopCoroutine(_disableCoroutine);
        _disableCoroutine = StartCoroutine(DisableFor());


        if (_triggerCount <= 1)
            DialogManager.Instance.ShowDialog(Index);
        else
        {
            int levelIndex = GetLevelIndex();
            // Get a random tone from the factory
            DialogData dialogData = DialogDataFactory.Instance.GetRandomGenericDialogFromList(levelIndex);
            DialogManager.Instance.ShowDialogByData(dialogData);
        }
    }

    private int GetLevelIndex()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        switch(sceneName)
        {
            case "Level_01":
                return 1;
                break;
            case "Level_02":
                return 2;
                break;
            case "Level_03":
                return 3;
                break;
            case "Level_04":
                return 4;
                break;
            case "Level_05":
                return 5;
                break;
            default:
                break;
        }
        return 0;
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
            obj = Instantiate(_bubbleSoftFly, transform.position, _softTarget.rotation);
        else
            obj = Instantiate(_bubbleSoftFlyLong, transform.position, _softTarget.rotation);

        obj.GetComponent<BubbleSoft>().SetTarget(_softTarget);
        obj.GetComponent<BubbleBase>().DoYourThing();

        _playerManager.SetAnimation("SOFT");
    }

    private void CreateAggressive()
    {
        GameObject obj = null;
        if (!_spawnLong)
            obj = Instantiate(_bubbleAggressiveAttack, 
                new Vector3(_playerManager.transform.position.x, _playerManager.transform.position.y + 1, _playerManager.transform.position.z),
                Quaternion.identity);
        else
            obj = Instantiate(_bubbleAggressiveAttackLong, 
                new Vector3(_playerManager.transform.position.x, _playerManager.transform.position.y + 1, _playerManager.transform.position.z),
                Quaternion.identity);

        obj.GetComponent<BubbleAggressive>().SetTarget(_objectToDestroy);
        obj.GetComponent<BubbleBase>().DoYourThing();

        _playerManager.SetAnimation("AGGR");
    }

    private void CreateCold()
    {
        GameObject obj = null;
        if (!_spawnLong)
            obj = Instantiate(_bubbleColdCreatePlatform, _coldSpawnTarget.transform.position, _coldSpawnTarget.rotation);
        else
            obj = Instantiate(_bubbleColdCreatePlatformLong, _coldSpawnTarget.transform.position, _coldSpawnTarget.rotation);

        obj.GetComponent<BubbleBase>().DoYourThing();
    }
}
