using Newtonsoft.Json;
using UnityEngine;

public class DialogDataFactory : MonoBehaviour
{
    public static DialogDataFactory Instance;

    private DialogDataList _dialogDataList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Parse json file for dialog data from Resources folder
        TextAsset dialogData = Resources.Load<TextAsset>("dialog");

        _dialogDataList = JsonConvert.DeserializeObject<DialogDataList>(dialogData.text);
    }

    public DialogData GetDialogData(int index)
    {
        return _dialogDataList.dialogData[index];
    }

    public DialogDataList GetDialogDataList()
    {
        return _dialogDataList;
    }
}
