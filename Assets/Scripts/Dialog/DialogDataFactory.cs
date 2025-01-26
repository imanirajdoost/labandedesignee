using Newtonsoft.Json;
using UnityEngine;

public class DialogDataFactory : MonoBehaviour
{
    public static DialogDataFactory Instance;

    private DialogDataList _dialogDataList;
    private DialogDataList _dialogGeneric;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Parse json file for dialog data from Resources folder
        TextAsset dialogData = Resources.Load<TextAsset>("dialog");

        _dialogDataList = JsonConvert.DeserializeObject<DialogDataList>(dialogData.text);

        RefreshList();
    }

    public DialogData GetDialogData(int index)
    {
        return _dialogDataList.dialogData[index];
    }

    public DialogDataList GetDialogDataList()
    {
        return _dialogDataList;
    }

    private void RefreshList()
    {
        TextAsset dialogGeneric = Resources.Load<TextAsset>("dialog_generic");

        _dialogGeneric = JsonConvert.DeserializeObject<DialogDataList>(dialogGeneric.text);
    }

    public DialogData GetRandomGenericDialogFromList(int level)
    {
        RefreshList();
        // if it's the first level, remove the second and third replies from the 

        var dialog = _dialogGeneric.dialogData[Random.Range(0, _dialogGeneric.dialogData.Length)];
        // remove the second and third replies
        if (level == 1)
        {
            dialog.replies[2] = null;
            dialog.replies[1] = null;
        }
        else if (level == 2)
        {
            dialog.replies[2] = null;
        }
        return dialog;
    }
}
