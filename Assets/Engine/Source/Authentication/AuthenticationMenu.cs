using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KAG
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
            SceneManager.LoadScene("Matchmaking");
        }

        private void OnLoginFailure()
        {
            throw new NotImplementedException();
        }

        #region Dialog UI events
        public void OnLoginClicked()
        {
            GameSession.Instance.Login(loginEmail.text, loginPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnRegisterClicked()
        {
            GameSession.Instance.Register(registerUsername.text, registerEmail.text, registerPassword.text, OnLoginSuccess, OnLoginFailure);
        }

        public void OnGuestClicked()
        {
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
