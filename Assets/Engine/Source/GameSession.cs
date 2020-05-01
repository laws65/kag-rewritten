using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Nakama;
using Nakama.TinyJson;

namespace KAG
{
    public class PlayerInfo : MessageBase
    {
        public string Username;
        public string UserId;

        public PlayerInfo()
        {
            Username = "Peasant";
            UserId = "[invalid]";
        }

        public PlayerInfo(ISession session)
        {
            Username = session.Username;
            UserId = session.UserId;
        }
    }

    public class ServerInfo
    {
        public string Name;
        public string IP;
    }

    public class GameSession
    {
        public PlayerInfo player = new PlayerInfo();

        #region Session events
        public delegate void OnLoginSuccess(PlayerInfo pinfo);
        public delegate void OnLoginFailure(Exception e);
        public delegate void OnLogout();

        public delegate void OnMatchmakeRefresh(List<ServerInfo> serverList);
        #endregion

        #region API endpoint
        string api_scheme;
        string api_host;
        int api_port;
        string api_key;
        #endregion

        #region Nakama specifics
        IClient nakama;
        ISession session;
        #endregion

        public bool IsConnected { get => session != null; }

        public GameSession(string api_scheme, string api_host, int api_port, string api_key)
        {
            this.api_scheme = api_scheme;
            this.api_host = api_host;
            this.api_port = api_port;
            this.api_key = api_key;
        }

        #region Authentication helpers
        private void Authenticate(ISession p_session)
        {
            if (p_session == null) return;

            session = p_session;
            player = new PlayerInfo(session);
        }

        public async void Login(string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            nakama = new Client(api_scheme, api_host, api_port, api_key, UnityWebRequestAdapter.Instance);

            try
            {
                email = email.Replace("\u200B", "");
                password = password.Replace("\u200B", "");

                if (string.IsNullOrWhiteSpace(email))
                    email = "invalid";
                if (string.IsNullOrWhiteSpace(password))
                    password = "invalid";

                Authenticate(await nakama.AuthenticateEmailAsync(email, password, null, false));
                onSuccess?.Invoke(player);
            }
            catch (ApiResponseException e)
            {
                onFailure?.Invoke(e);
            }
        }

        public async void Register(string username, string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            nakama = new Client(api_scheme, api_host, api_port, api_key, UnityWebRequestAdapter.Instance);

            try
            {
                username = username.Replace("\u200B", "");
                email = email.Replace("\u200B", "");
                password = password.Replace("\u200B", "");

                if (string.IsNullOrWhiteSpace(username))
                    username = "invalid";
                if (string.IsNullOrWhiteSpace(email))
                    email = "invalid";
                if (string.IsNullOrWhiteSpace(password))
                    password = "invalid";

                Authenticate(await nakama.AuthenticateEmailAsync(email, password, username, true));
                onSuccess?.Invoke(player);
            }
            catch (ApiResponseException e)
            {
                onFailure?.Invoke(e);
            }
        }

        public async void LoginAsGuest(OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            nakama = new Client(api_scheme, api_host, api_port, api_key, UnityWebRequestAdapter.Instance);

            string deviceId = PlayerPrefs.GetString("Nakama.DeviceId");
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                deviceId = "";
                for (var i = 0; i < 10; i++)
                {
                    deviceId += ((char)('A' + UnityEngine.Random.Range(0, 26))).ToString();
                }
                PlayerPrefs.SetString("Nakama.DeviceId", deviceId);
            }

            try
            {
                Authenticate(await nakama.AuthenticateDeviceAsync(deviceId));
                onSuccess?.Invoke(player);
            }
            catch (ApiResponseException e)
            {
                onFailure?.Invoke(e);
            }
        }

        public void LoginOffline(OnLoginSuccess onSuccess = null)
        {
            onSuccess?.Invoke(player);
        }

        public void Logout(OnLogout callback)
        {
            player = null;
            session = null;
            nakama = null;

            callback?.Invoke();
        }
        #endregion

        #region Matchmaking helpers
        public async void MatchmakeRefresh(OnMatchmakeRefresh onResult = null)
        {
            var result = await nakama.RpcAsync(session, "get_server_list");
            var list = result.Payload.FromJson<List<ServerInfo>>();

            if (list == null)
            {
                onResult?.Invoke(new List<ServerInfo>());
            }
            else
            {
                onResult?.Invoke(list);
            }
        }

        public async void MatchmakeCreate(ServerInfo serverInfo)
        {
            await nakama.RpcAsync(session, "create_server", serverInfo.ToJson());
        }
        #endregion
    }
}
