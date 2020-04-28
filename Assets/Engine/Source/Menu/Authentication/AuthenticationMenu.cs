using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KAG.Menu
{
    public class AuthenticationMenu : MonoBehaviour
    {
        private GameEngine gameEngine;
        private GameSession gameSession;

        public GameObject loginPanel;
        public TextMeshProUGUI loginEmail;
        public TextMeshProUGUI loginPassword;
        public Button loginButton;
        public Button guestButton;
        public Button goToRegisterButton;

        [Space]
        public GameObject registerPanel;
        public TextMeshProUGUI registerUsername;
        public TextMeshProUGUI registerEmail;
        public TextMeshProUGUI registerPassword;
        public Button registerButton;
        public Button goToLoginButton;

        private void Awake()
        {
            gameEngine = GameEngine.Instance;
            gameSession = GameEngine.Instance.gameSession;
        }

        private void Start()
        {
            ShowLogin();
        }

        public void ShowLogin()
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }

        public void ShowRegister()
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
        }

        private void OnLoginSuccess(PlayerInfo pinfo)
        {
            gameEngine.LoadScene(GameScene.Menu);
        }

        private void OnLoginFailure(Exception e)
        {
            gameEngine.ShowError(e.Message);
        }

        #region Dialog UI events
        public void OnLoginClicked()
        {
            gameEngine.ShowMessage("Logging in...");
            gameSession.Login(loginEmail.text, loginPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnRegisterClicked()
        {
            gameEngine.ShowMessage("Registering...");
            gameSession.Register(registerUsername.text, registerEmail.text, registerPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnGuestClicked()
        {
            gameEngine.ShowMessage("Logging in as a guest...");
            gameSession.LoginAsGuest(OnLoginSuccess, OnLoginFailure);
        }

        public void OnOfflineClicked()
        {
            gameSession.LoginOffline(OnLoginSuccess);
        }
        #endregion
    }
}
