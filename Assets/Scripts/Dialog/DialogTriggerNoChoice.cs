using System.Collections;
using UnityEngine;

public class DialogTriggerNoChoice : DialogTrigger
{
    public override void SetTriggered()
    {
        base.SetTriggered();
        DialogManager.Instance.ShowDialog(Index);
        if (_shouldDestroyAfterTriggered)
            StartCoroutine(DisableAfter(2));
    }

    private IEnumerator DisableAfter(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
