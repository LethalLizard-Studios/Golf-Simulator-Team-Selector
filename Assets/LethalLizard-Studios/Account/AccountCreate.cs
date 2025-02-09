using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AccountCreate : MonoBehaviour
{
    [SerializeField] private ActiveAccounts activeAccounts;
    [SerializeField] private TextMeshProUGUI displayIdentification;

    [SerializeField] private TMP_InputField accountNameInput;
    [SerializeField] private MusicKits musicKits;

    [SerializeField] private UnityEvent onSuccess;
    [SerializeField] private UnityEvent onFailure;
    [SerializeField] private UnityEvent onReset;

    private Color selectedColor = Color.green;
    private bool isRightHanded = true;

    public void Create()
    {
        string username = accountNameInput.text;
        string identification = AccountManager.GenerateUniqueIdentification();
        string musicKit = musicKits.GetMusicName();

        Account account = new Account(username, identification, musicKit, selectedColor, isRightHanded);
        AccountManager.SaveAccount(account);
        activeAccounts.AddAccount(account);

        WindowsVoice.speak("Please write down or take a picture to use this account in the future");

        displayIdentification.text = identification;
        onSuccess.Invoke();
    }
}
