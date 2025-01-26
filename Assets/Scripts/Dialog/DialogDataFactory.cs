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

        TextAsset dialogGeneric = Resources.Load<TextAsset>("dialog_generic");

        _dialogGeneric = JsonConvert.DeserializeObject<DialogDataList>(dialogGeneric.text);
    }

    public DialogData GetDialogData(int index)
    {
        return _dialogDataList.dialogData[index];
    }

    public DialogDataList GetDialogDataList()
    {
        return _dialogDataList;
    }

    public DialogData GetRandomGenericDialogFromList()
    {
        return _dialogGeneric.dialogData[Random.Range(0, _dialogGeneric.dialogData.Length)];
    }
}
