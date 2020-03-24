using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        GameSession.Instance.onLoginSuccess += OnLoginSuccess;

        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
        guestButton.onClick.AddListener(OnGuestClicked);

        goToLoginButton.onClick.AddListener(ShowLogin);
        goToRegisterButton.onClick.AddListener(ShowRegister);
    }

    private void OnLoginSuccess(PlayerInfo pinfo)
    {
        SceneManager.LoadScene("Matchmaking");
    }

    private void OnLoginClicked()
    {
        GameSession.Instance.Login(loginEmail.text, loginPassword.text);
    }

    private void OnRegisterClicked()
    {
        GameSession.Instance.Register(registerUsername.text, registerEmail.text, registerPassword.text);
    }

    private void OnGuestClicked()
    {
        GameSession.Instance.LoginAsGuest();
    }

    private void ShowLogin()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private void ShowRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
}
