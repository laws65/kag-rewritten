using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KAG.Menu
{
    public class AuthenticationMenu : MonoBehaviour
    {
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
            ShowLogin();
        }

        private void OnLoginSuccess(PlayerInfo pinfo)
        {
            SceneManager.LoadScene(GameEngine.menuScene);
        }

        private void OnLoginFailure(Exception e)
        {
            Toast.Instance.ShowError(e.Message);
        }

        #region Dialog UI events
        public void OnLoginClicked()
        {
            Toast.Instance.Show("Logging in...");
            GameSession.Instance.Login(loginEmail.text, loginPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnRegisterClicked()
        {
            Toast.Instance.Show("Registering...");
            GameSession.Instance.Register(registerUsername.text, registerEmail.text, registerPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnGuestClicked()
        {
            Toast.Instance.Show("Logging in as a guest...");
            GameSession.Instance.LoginAsGuest(OnLoginSuccess, OnLoginFailure);
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
            #endregion
        }
    }
}
