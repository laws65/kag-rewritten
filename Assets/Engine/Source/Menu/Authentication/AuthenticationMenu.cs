using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KAG.Menu
{
    public class AuthenticationMenu : MonoBehaviour
    {
        private GameManager gameManager;
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

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameSession = GameManager.Instance.session;

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
            gameManager.LoadScene(GameScene.Menu);
        }

        private void OnLoginFailure(Exception e)
        {
            gameManager.ShowError(e.Message);
        }

        #region Dialog UI events
        public void OnLoginClicked()
        {
            gameManager.ShowMessage("Logging in...");
            gameSession.Login(loginEmail.text, loginPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnRegisterClicked()
        {
            gameManager.ShowMessage("Registering...");
            gameSession.Register(registerUsername.text, registerEmail.text, registerPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnGuestClicked()
        {
            gameManager.ShowMessage("Logging in as a guest...");
            gameSession.LoginAsGuest(OnLoginSuccess, OnLoginFailure);
        }

        public void OnOfflineClicked()
        {
            gameSession.LoginOffline(OnLoginSuccess);
        }
        #endregion
    }
}
