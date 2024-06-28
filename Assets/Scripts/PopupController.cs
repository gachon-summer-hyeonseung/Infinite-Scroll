using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;

    void Start()
    {
        UserData data = UserManager.Instance.selectedUserData;
        infoText.text = $"이름 : {data.name}\n나이 : {data.age}\n성별 : {data.gender}\n취미 : {data.hobby}\n직업 : {data.job}";
    }

    public void ClosePopup()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
