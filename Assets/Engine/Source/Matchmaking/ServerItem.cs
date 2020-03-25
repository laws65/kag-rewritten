using TMPro;
using UnityEngine;

public class ServerItem : MonoBehaviour
{
    public TextMeshProUGUI label;
    public ServerInfo serverInfo;

    private void Start()
    {
        label.text = string.Format("{0} ({1})", serverInfo.server_name, serverInfo.server_ip);
    }
}
