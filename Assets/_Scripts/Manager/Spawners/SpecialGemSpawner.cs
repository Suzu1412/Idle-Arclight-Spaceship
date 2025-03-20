using UnityEngine;

public class SpecialGemSpawner : MonoBehaviour, IPausable
{
    [SerializeField] private BoolVariableSO _isPaused;

    [SerializeField] private BasePlacementStrategySO _placementStrategy;


    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Pause(bool isPaused)
    {

    }
}
