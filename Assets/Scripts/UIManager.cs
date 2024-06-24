using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform buttonPrefab;

    [SerializeField] private TextMeshProUGUI popupText;

    List<UserData> userDataList;

    // Start is called before the first frame update
    void Start()
    {
        userDataList = UserManager.Instance.GetUserData();

        AddNextUsers();
    }

    void Update()
    {
        if (scrollRect.verticalNormalizedPosition < 0.04f) AddNextUsers();
    }

    void AddNextUsers()
    {
        int count = content.childCount;
        if (count >= userDataList.Count) return;

        for (int i = count; i < count + 20; i++)
        {
            UserData userData = userDataList[i];
            RectTransform userObject = Instantiate(buttonPrefab, content);
            userObject.GetComponentInChildren<TextMeshProUGUI>().text = userData.name;
            userObject.GetComponent<Button>().onClick.AddListener(() => UpdatePopup(userData));
        }
    }

    void UpdatePopup(UserData data)
    {
        string text = $"이름 : {data.name}\n나이 : {data.age}\n성별 : {data.gender}\n취미 : {data.hobby}\n직업 : {data.job}";
        popupText.text = text;
    }
}
