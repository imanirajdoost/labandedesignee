using TMPro;
using UnityEngine;

public class DialogChoiceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _choiceText;
    private string _choiceTone;

    public void SetChoiceText(string text)
    {
        _choiceText.text = text;
    }

    public void SetChoiceTone(string tone)
    {
        _choiceTone = tone;
    }

    public string GetChoiceTone()
    {
        return _choiceTone;
    }

    public void OnClick()
    {
        DialogManager.Instance.ChoiceSelected(_choiceTone);
    }
}
