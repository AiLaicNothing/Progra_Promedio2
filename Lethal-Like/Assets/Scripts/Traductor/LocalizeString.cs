using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizeString : MonoBehaviour
{
    [SerializeField] private LocalizedString stringReference;
    private TextMeshProUGUI textToLocalize;

    private void Awake()
    {
        textToLocalize = GetComponent<TextMeshProUGUI>();
        stringReference.StringChanged += OnStringChanged;
        stringReference.GetLocalizedStringAsync().Completed += OnCompleted;
    }

    private void OnCompleted(AsyncOperationHandle<string> result)
    {
        textToLocalize.text = result.Result;
    }

    private void OnStringChanged(string text)
    {
        textToLocalize.text = text;
    }

}