using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_DrawName : bl_PhotonHelper
{
    [HideInInspector]public string PlayerName;
    /// <summary>
    /// Object to follow UI
    /// </summary>
    public Transform Target;
    /// <summary>
    /// UI Prefabs
    /// </summary>
    public GameObject UIPrefab;
    [Space(5)]
    public Vector3 OffSet = Vector3.zero;
    public float Multipler = 0.002f;
    //Privates
    private Transform m_Transform = null;
    private Text m_Text = null;
    private GameObject cacheUI;
    private Image HealthBar;
    private bl_PlayerDamage PlayerDamage;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        PlayerDamage = GetComponent<bl_PlayerDamage>();
        cacheUI = Instantiate(UIPrefab) as GameObject;
        Transform t = GameObject.Find("WorldCanvas").transform;
        cacheUI.transform.SetParent(t);
        m_Transform = cacheUI.transform;
        m_Text = cacheUI.GetComponentInChildren<Text>();
        HealthBar = cacheUI.GetComponentsInChildren<Image>()[1];
        StartCoroutine(OnUpdate());
    }

    /// <summary>
    /// When Player Die Destroy text
    /// </summary>
    void OnDisable()
    {
        if (m_Transform != null)
        {
            Destroy(m_Transform.gameObject);
        }
        if (photonView.isMine)
        {
            bl_EventHandler.LocalPlayerDeathEvent -= OnLocalDeath;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        if (photonView.isMine)
        {
            bl_EventHandler.LocalPlayerDeathEvent += OnLocalDeath;
        }
    }


    void OnLocalDeath()
    {
        ShowUI(false);
    }

    public void ShowUI(bool show)
    {
        if (cacheUI == null)
            return;

        cacheUI.SetActive(show);
    }

    IEnumerator OnUpdate()
    {
        while (true)
        {
            if (Target == null || m_Transform == null || !cacheUI.activeSelf)
                yield return null;

            Camera c = Camera.current;
            if (Camera.main != null)
            {
                c = Camera.main;
            }

            //Calculate the size and position of ui
            if (c != null)
            {
                float distance = Vector3.Distance(Target.position, c.transform.position);
                float d = (distance * 0.015f);
                d = Mathf.Clamp(d, 0.15f, 1.20f);

                //Follow Ui to the target in position and rotation
                m_Transform.localScale = new Vector3(d, d, d);
                OffSet.y = 2 + d;
                m_Transform.position = Target.position + OffSet;
                m_Transform.rotation = c.transform.rotation;
                PlayerName = gameObject.name;
                if (m_Text != null)
                {
                    m_Text.text = PlayerName;
                }
                HealthBar.fillAmount = (PlayerDamage.Health * 0.01f);
            }
            yield return null;
        }
    }
}