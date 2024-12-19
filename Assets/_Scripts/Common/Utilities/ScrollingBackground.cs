using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private float _speed = 1f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _renderer.material.mainTextureOffset += new Vector2(0f, _speed * Time.deltaTime);
    }
}
