using UnityEngine;
using UnityEngine.UI;

public class RulesPanelController : MonoBehaviour
{
    public GameObject rulesPanel;
    public Button exitButton;

    void Start()
    {
        rulesPanel.SetActive(false);
        exitButton.gameObject.SetActive(false);

        exitButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel()
    {
        rulesPanel.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    void HidePanel()
    {
        rulesPanel.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
}
