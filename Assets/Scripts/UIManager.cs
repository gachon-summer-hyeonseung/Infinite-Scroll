using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform buttonPrefab;

    [SerializeField] private TextMeshProUGUI popupText;

    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private TMP_Dropdown filterDropdown;
    [SerializeField] private TMP_Dropdown orderDropdown;

    List<UserData> userDataList;
    List<UserData> currentDataList;

    bool isAddable = true;


    // Start is called before the first frame update
    void Start()
    {
        userDataList = UserManager.Instance.GetUserData();
        currentDataList = userDataList;

        RunQuery();
    }

    void Update()
    {
        if (isAddable && scrollRect.verticalNormalizedPosition < 0.01f) AddNextUsers();
    }

    public void RunQuery()
    {
        IEnumerable<UserData> query = userDataList.AsEnumerable();
        UpdateSearch(ref query);
        UpdateFilter(ref query);
        UpdateOrder(ref query);

        currentDataList = query.ToList();
        RefreshUsers();
    }

    void UpdateSearch(ref IEnumerable<UserData> query)
    {
        string searchQuery = searchInput.text;
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(x => x.name.Contains(searchQuery));
        }
    }

    void UpdateFilter(ref IEnumerable<UserData> query)
    {
        int filter = filterDropdown.value;
        switch (filter)
        {
            case 0:
                break;
            case 1:
                query = query.Where(x => x.gender == "남");
                break;
        }
    }

    void UpdateOrder(ref IEnumerable<UserData> query)
    {
        int order = orderDropdown.value;
        switch (order)
        {
            case 0:
                query = query.OrderBy(x => x.name);
                break;
            case 1:
                query = query.OrderByDescending(x => x.name);
                break;
        }
    }

    void RefreshUsers()
    {
        isAddable = false;
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        isAddable = true;
    }

    void AddNextUsers()
    {
        int count = content.childCount;
        if (count >= currentDataList.Count) return;

        int itemCount = count + 20 > currentDataList.Count ? currentDataList.Count - count : 20;

        for (int i = count; i < count + itemCount; i++)
        {
            UserData userData = currentDataList[i];
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
