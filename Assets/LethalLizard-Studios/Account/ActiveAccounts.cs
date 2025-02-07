using System.Collections.Generic;
using UnityEngine;

public class ActiveAccounts : MonoBehaviour
{
    [SerializeField] private Transform userContentHolder;
    [SerializeField] private GameObject userPrefab;

    private List<Account> _accounts = new List<Account>();

    public void AddAccount(Account account)
    {
        UserActive user = Instantiate(userPrefab, userContentHolder).GetComponent<UserActive>();
        user.Initialize(account.username, account.preferredColor);
        _accounts.Add(account);
    }

    public Account[] GetAllAcounts()
    {
        return _accounts.ToArray();
    }
}
