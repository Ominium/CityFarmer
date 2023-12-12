using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{
    public TMP_InputField IdInputField;
    public TMP_InputField PassWordInputField;
    public TMP_InputField NameInputField;
    public Button LoginChangeButton;
    public Button SignChangeButton;
    public Button LoginSubmitButton;
    public Button SignSubmitButton;

    public Button[] Buttons;
    private void Start()
    {
        
        LoginSubmitButton.onClick.AddListener(() => Login());
        SignSubmitButton.onClick.AddListener(() => Sign());
        LoginChangeButton.onClick.AddListener(() => ChangeForm(false));
        SignChangeButton.onClick.AddListener(() => ChangeForm(true));
    }
    private void OnEnable()
    {
        ChangeForm(false);
    }
    private void ChangeForm(bool Sign)
    {
        if (Sign)
        {
            ChangeColor(LoginChangeButton, Color.white);
            ChangeColor(SignChangeButton, Color.red);
            SignChangeButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            LoginChangeButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        else
        {
            ChangeColor(LoginChangeButton, Color.red);
            ChangeColor(SignChangeButton, Color.white);
            LoginChangeButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            SignChangeButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        NameInputField.gameObject.SetActive(Sign);
        LoginSubmitButton.gameObject.SetActive(!Sign);
        SignSubmitButton.gameObject.SetActive(Sign);
    }
    private void ChangeColor(Button button,Color color)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.selectedColor = color;
        colorBlock.normalColor = color;
        button.colors = colorBlock;
    }
    private void Login()
    {
        if (InfoManager.Instance.Login(IdInputField.text,PassWordInputField.text))
        {
            InfoManager.Instance.LoadFood();
            InfoManager.Instance.LoadItem();
            InfoManager.Instance.LoadMoney();
            InfoManager.Instance.LoadShop();
            InfoManager.Instance.LoadWater();
            gameObject.SetActive(false);
        }
    }
    public void TestLogin()
    {
        if (InfoManager.Instance.Login("kos1515", "dnflskfk1"))
        {
            InfoManager.Instance.LoadFood();
            InfoManager.Instance.LoadItem();
            InfoManager.Instance.LoadMoney();
            InfoManager.Instance.LoadShop();
            InfoManager.Instance.LoadWater();
            gameObject.SetActive(false);
        }

        SceneManager.LoadScene("FarmScene");

    }
    private void Sign()
    {
        InfoManager.Instance.SignUp(IdInputField.text, PassWordInputField.text,NameInputField.text);
        InfoManager.Instance.InsertSQL(InfoManager.Instance.MoneyInsertQuery);
        InfoManager.Instance.InsertSQL(InfoManager.Instance.WaterInsertQuery);
        Login();
    }
}
