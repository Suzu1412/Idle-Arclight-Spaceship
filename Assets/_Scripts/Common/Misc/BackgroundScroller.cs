using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Transform bg1, bg2;
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] Transform fg1, fg2;
    [SerializeField] private float _positionToReset = 20f;

    // Update is called once per frame
    void Update()
    {
        bg1.position -= new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
        bg2.position -= new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);

        if (bg1.position.y < _positionToReset)
        {
            bg1.position += new Vector3(0f, 50, 0);
        }

        if (bg2.position.y < _positionToReset)
        {
            bg2.position += new Vector3(0f, 50, 0);
        }

        fg1.position -= new Vector3(0f, scrollSpeed * 3 * Time.deltaTime, 0f);
        fg2.position -= new Vector3(0f, scrollSpeed * 3 * Time.deltaTime, 0f);

        if (fg1.position.y < _positionToReset)
        {
            fg1.position += new Vector3(0f, 50, 0);
        }

        if (fg2.position.y < _positionToReset)
        {
            fg2.position += new Vector3(0f, 50, 0);
        }
    }
}
