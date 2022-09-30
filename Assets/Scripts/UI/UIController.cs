using Main;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public string UserName { get; set; }
    [SerializeField] Button okButton;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject main;

    [SerializeField] GameObject table;
    [SerializeField] TMP_Text tableText;


    // Start is called before the first frame update
    void Start()
    {
        okButton.onClick.AddListener(() => onOkButton());
    }

    private void onOkButton()
    {
        if (nameField?.text == null || nameField?.text?.Length == 0)
        {
            return;
        }

        UserName = nameField.text;
        panel.SetActive(false);
        main.SetActive(true);
    }

    public void ShowTable(string text)
    {
        tableText.text = "Таблица очков\n" + text;
        table.SetActive(true);
    }
}
