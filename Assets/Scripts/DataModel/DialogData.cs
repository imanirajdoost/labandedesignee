using System;
using System.Collections.Generic;

[Serializable]
public class DialogData
{
    public int index;
    public string line;
    public string tone;
    public ReplyData[] replies;
}

[Serializable]
public class ReplyData
{
    public string type;
    public string line;
}

[Serializable]
public class DialogDataList
{
    public DialogData[] dialogData;
}
