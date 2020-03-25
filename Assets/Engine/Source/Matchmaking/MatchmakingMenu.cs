using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchmakingMenu : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    [Space]
    public GameObject listContent;
    public GameObject listItem;
    public Button refreshButton;

    [Space]
    public Button playButton;

    private void Awake()
    {
        GameSession.Instance.OnMatchmakeRefresh += OnMatchmakeRefresh;
        Refresh();
    }

    public void Refresh()
    {
        GameSession.Instance.MatchmakeRefresh();
    }

    private void OnMatchmakeRefresh(List<ServerInfo> list)
    {
        foreach (Transform child in listContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ServerInfo server in list)
        {
            ServerItem item = Instantiate(listItem, listContent.transform).GetComponent<ServerItem>();
            item.serverInfo = server;
        }
    }
}
