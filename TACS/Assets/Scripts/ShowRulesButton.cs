using UnityEngine;
using UnityEngine.UI;

public class ShowRulesButton : MonoBehaviour
{
    public RulesPanelController rulesPanelController; // Drag your RulesPanelController script here in the Inspector

    void Start()
    {
        Button rulesButton = GetComponent<Button>();
        rulesButton.onClick.AddListener(ShowPanel);
    }

    void ShowPanel()
    {
        rulesPanelController.ShowPanel();
    }
}
