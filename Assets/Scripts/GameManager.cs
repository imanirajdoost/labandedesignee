using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _vfxImpact;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SpawnImpactVFX(Vector3 position)
    {
        var obj = Instantiate(_vfxImpact, position, Quaternion.identity);

        Destroy(obj, 2);
    }
}
