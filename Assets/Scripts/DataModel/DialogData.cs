using System.Collections.Generic;

public class DialogData
{
    public int index;
    public string line;
    public string tone;
    public List<ReplyData> replyList;

}

public class ReplyData
{
    public string type;
    public string line;
}
