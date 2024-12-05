using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private CanvasRenderer[] _renderers;


    private void Awake()
    {
        _renderers = transform.GetComponentsInChildren<CanvasRenderer>();
        foreach (var image in _renderers)
            image.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        bool cond = !_renderers[0].gameObject.activeSelf;

        Time.timeScale = cond ? 0f : 1f;

        foreach (var image in _renderers)
            image.gameObject.SetActive(cond);
    }

    public void Continue()
    {
        Time.timeScale = 1f;

        foreach (var image in _renderers)
            image.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        Time.timeScale = 1f;

        foreach (var image in _renderers)
            image.gameObject.SetActive(false);
    }
}