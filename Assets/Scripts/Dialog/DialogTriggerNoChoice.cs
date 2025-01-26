using DG.Tweening;
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
        // make scale to 0 with dotween
        gameObject.transform.DOScale(Vector3.zero, time - 0.1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(time);

        // Make scale to 0 and deactivate
        gameObject.SetActive(false);
    }
}
