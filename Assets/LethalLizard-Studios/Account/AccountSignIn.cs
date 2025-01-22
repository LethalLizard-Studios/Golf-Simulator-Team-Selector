using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AccountSignIn : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private UnityEvent onSuccess;
    [SerializeField] private UnityEvent onFailure;

    public void SignIn()
    {
        Account loadedAccount = AccountManager.LoadAccount(inputField.text);

        if (loadedAccount != null)
        {
            onSuccess.Invoke();
            return;
        }

        onFailure.Invoke();
    }
}
