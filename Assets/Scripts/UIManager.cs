using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scroll View")]
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform buttonPrefab;

    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI popupText;

    [Header("Search")]
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

        Init();
    }

    void Update()
    {
        if (isAddable && scrollRect.verticalNormalizedPosition < 0.01f) AddNextUsers();
    }

    void Init()
    {
        searchInput.text = ScrollData.searchQuery;
        filterDropdown.value = ScrollData.filter;
        orderDropdown.value = ScrollData.order;

        RunQuery();

        for (int i = 0; i < ScrollData.itemCount; i++)
        {
            UserData userData = currentDataList[i];
            InsertUser(userData);
        }

        scrollRect.verticalNormalizedPosition = ScrollData.scrollValue;
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
            InsertUser(userData);
        }
    }

    void InsertUser(UserData userData)
    {
        RectTransform userObject = Instantiate(buttonPrefab, content);
        userObject.GetComponentInChildren<TextMeshProUGUI>().text = userData.name;
        userObject.GetComponent<Button>().onClick.AddListener(() => OpenPopup(userData));
    }

    void OpenPopup(UserData userData)
    {
        SaveData();
        UserManager.Instance.SetUserData(userData);
        SceneManager.LoadScene("PopupScene");
        // popup.SetActive(true);
        // UpdatePopup(userData);
    }

    void SaveData()
    {
        ScrollData.searchQuery = searchInput.text;
        ScrollData.filter = filterDropdown.value;
        ScrollData.order = orderDropdown.value;
        ScrollData.itemCount = content.childCount;
        ScrollData.scrollValue = scrollRect.verticalNormalizedPosition;
    }

    // void UpdatePopup(UserData data)
    // {
    //     string text = $"이름 : {data.name}\n나이 : {data.age}\n성별 : {data.gender}\n취미 : {data.hobby}\n직업 : {data.job}";
    //     popupText.text = text;
    // }

    // public void ClosePopup()
    // {
    //     popup.SetActive(false);
    // }
}
