using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bl_UIManager : MonoBehaviour
{
    public Canvas WorldCanvas;
    public Image BlackBackground = null;

    public GameObject StartWindow = null;

    /// <summary>
    /// When Player Die Destroy text
    /// </summary>
    void OnDisable()
    {

        bl_EventHandler.LocalPlayerSpawnEvent -= OnLocalSpawn;

    }
    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        bl_EventHandler.LocalPlayerSpawnEvent += OnLocalSpawn;
    }

    /// <summary>
    /// 
    /// </summary>
    void OnLocalSpawn(GameObject player)
    {
        WorldCanvas.worldCamera = player.GetComponent<bl_PlayerPhoton>().PlayerCamera;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(float t)
    {
        if (BlackBackground == null)
            yield return null;

        BlackBackground.gameObject.SetActive(true);
        Color c = BlackBackground.color;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            c.a = t;
            BlackBackground.color = c;
            yield return null;
        }
        BlackBackground.gameObject.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(float t)
    {
        if (BlackBackground == null)
            yield return null;
        BlackBackground.gameObject.SetActive(true);
        Color c = BlackBackground.color;
        while (c.a < t)
        {
            c.a += Time.deltaTime;
            BlackBackground.color = c;
            yield return null;
        }
    }

    private static bl_UIManager _instance;
    public static bl_UIManager Instance
    {
        get
        {
            if (_instance == null) { _instance = FindObjectOfType<bl_UIManager>(); }
            return _instance;
        }
    }
}