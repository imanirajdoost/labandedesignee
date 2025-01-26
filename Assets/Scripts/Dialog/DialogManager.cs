using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [SerializeField] private TextMeshProUGUI _dialogText;

    [SerializeField] private GameObject _dialogChoiceObject;
    [SerializeField] private GameObject _dialogChoiceObjectParent;
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _dialogPanel;
    private RectTransform _dialogPanelRectTransform;

    private List<GameObject> _dialogChoices;

    private int _lastShownDialogIndex = -1;

    public Action<string> OnChoiceSelected;
    public Action OnDialogPanelClosed;
    public Action OnTimeUp;

    [SerializeField] private float _dialogReponseTime = 5f;
    [SerializeField] private Slider _dialogTimeSlider;

    private float _currentDialogTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _dialogPanelRectTransform = _dialogPanel.GetComponent<RectTransform>();
    }

    public void OpenDialogPanel()
    {
        // Use dotween to animate the dialog panel
        _dialogPanelRectTransform.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutBack);
    }

    public void CloseDialogPanel()
    {
        _dialogPanelRectTransform.DOAnchorPos(new Vector2(0, -150), 0.5f).SetEase(Ease.InBack);
    }

    public void ShowDialog(DialogData data)
    {
        RemoveAllRepliesIfExist();

        _dialogText.text = data.line;

        ShowDialogOptionsIfAvailable(data);
    }

    private void RemoveAllRepliesIfExist()
    {
        if (_dialogChoices != null && _dialogChoices.Count > 0)
        {
            foreach (var choice in _dialogChoices)
                Destroy(choice);
        }
    }

    private void Update()
    {
        if (_dialogTimeSlider.gameObject.activeSelf)
        {
            _currentDialogTime -= Time.deltaTime;
            _dialogTimeSlider.value = _currentDialogTime;
            if (_currentDialogTime <= 0)
            {
                CloseDialog();
                OnTimeUp?.Invoke();
            }
        }
    }

    private void SetCloseButtonEnabled(bool enabled)
    {
        _closeButton.interactable = enabled;
    }

    private void ShowDialogOptionsIfAvailable(DialogData data)
    {
        if (data.replies == null || data.replies.Length == 0)
        {
            SetCloseButtonEnabled(true);
            _dialogTimeSlider.gameObject.SetActive(false);
            return;
        }

        SetCloseButtonEnabled(false);

        _currentDialogTime = _dialogReponseTime;
        _dialogTimeSlider.maxValue = _dialogReponseTime;
        _dialogTimeSlider.value = _dialogReponseTime;

        _dialogTimeSlider.gameObject.SetActive(true);


        if (_dialogChoices == null)
            _dialogChoices = new List<GameObject>();
        else
            _dialogChoices.Clear();

        foreach (var reply in data.replies)
        {
            var choicObj = Instantiate(_dialogChoiceObject, _dialogChoiceObjectParent.transform);
            _dialogChoices.Add(choicObj);
            var choiceController = choicObj.GetComponent<DialogChoiceController>();
            choiceController.SetChoiceText(reply.line);
            choiceController.SetChoiceTone(reply.type);
        }
    }

    public void ChoiceSelected(string tone)
    {
        Debug.Log("Choice selected: " + tone);
        OnChoiceSelected?.Invoke(tone);
        CloseDialog();
    }

    public void ShowNextDialog()
    {
        _lastShownDialogIndex++;
        var data = DialogDataFactory.Instance.GetDialogData(_lastShownDialogIndex);

        OpenDialogPanel();
        ShowDialog(data);
    }

    public void ShowDialog(int index)
    {
        var data = DialogDataFactory.Instance.GetDialogData(index);
        OpenDialogPanel();
        ShowDialog(data);
    }

    public void CloseDialog()
    {
        CloseDialogPanel();
        _dialogTimeSlider.gameObject.SetActive(false);
        OnDialogPanelClosed?.Invoke();
    }
}
