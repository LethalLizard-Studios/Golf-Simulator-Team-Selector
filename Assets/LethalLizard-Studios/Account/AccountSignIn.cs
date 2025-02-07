using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AccountSignIn : MonoBehaviour
{
    [SerializeField] private ActiveAccounts activeAccounts;

    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private UnityEvent onSuccess;
    [SerializeField] private UnityEvent onFailure;
    [SerializeField] private UnityEvent onReset;

    private void Awake()
    {
        WindowsVoice.initSpeech();
    }

    private void OnEnable()
    {
        onReset.Invoke();
    }

    public void SignIn()
    {
        Account loadedAccount = AccountManager.LoadAccount(inputField.text);

        if (loadedAccount != null)
        {
            activeAccounts.AddAccount(loadedAccount);
            Debug.Log($"Welcome back, {loadedAccount.username} ({loadedAccount.identification})");
            onSuccess.Invoke();
            WindowsVoice.speak($"Welcome back {loadedAccount.username}");
            return;
        }

        onFailure.Invoke();
    }
}
