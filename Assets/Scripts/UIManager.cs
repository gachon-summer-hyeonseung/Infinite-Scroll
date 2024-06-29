using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void ActionRef<T>(ref T item);

public class DropDownOptions
{
    public string text;
    public ActionRef<IEnumerable<UserData>> action;

    public DropDownOptions(string text, ActionRef<IEnumerable<UserData>> action)
    {
        this.text = text;
        this.action = action;
    }
}

public class FilterOptions
{
    public string text;
    public List<DropDownOptions> options;

    public FilterOptions(string text, List<DropDownOptions> options)
    {
        this.text = text;
        this.options = options;
    }
}

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
    [SerializeField] private TMP_Dropdown orderDropdown;

    [Header("Filter")]
    [SerializeField] private TMP_Dropdown filterDropdown;
    [SerializeField] private TMP_Dropdown filterDropdown2;

    List<FilterOptions> filterOptions = new()
    {
        new(
            "없음",
            new() {
                new("없음", (ref IEnumerable<UserData> query) => {})
            }
        )
    };

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
        InitFilter();
        searchInput.text = ScrollData.searchQuery;
        orderDropdown.value = ScrollData.order;

        RunQuery();

        for (int i = 0; i < ScrollData.itemCount; i++)
        {
            UserData userData = currentDataList[i];
            InsertUser(userData);
        }

        scrollRect.verticalNormalizedPosition = ScrollData.scrollValue;
    }

    #region Filter

    void InitFilter()
    {
        CreateFilterOptions();
        List<string> filterNames = filterOptions.Select(x => x.text).ToList();
        filterDropdown.ClearOptions();
        filterDropdown.AddOptions(filterNames);
        filterDropdown.value = ScrollData.filter;
        ChangeFilter();
        filterDropdown2.value = ScrollData.filter2;
    }

    public void CreateFilterOptions()
    {
        List<DropDownOptions> options = new(){
            new("없음", (ref IEnumerable<UserData> query) => {})
        };

        string[] genders = UserManager.Instance.GetGenders();
        foreach (string gender in genders)
        {
            options.Add(new(gender, (ref IEnumerable<UserData> query) => query = query.Where(x => x.gender == gender)));
        }
        filterOptions.Add(new("성별", options.ToList()));

        options.Clear();
        options.Add(new("없음", (ref IEnumerable<UserData> query) => { }));

        string[] hobbies = UserManager.Instance.GetHobbies();
        foreach (string hobby in hobbies)
        {
            options.Add(new(hobby, (ref IEnumerable<UserData> query) => query = query.Where(x => x.hobby == hobby)));
        }
        filterOptions.Add(new("취미", options.ToList()));

        options.Clear();
        options.Add(new("없음", (ref IEnumerable<UserData> query) => { }));

        string[] jobs = UserManager.Instance.GetJobs();
        foreach (string job in jobs)
        {
            options.Add(new(job, (ref IEnumerable<UserData> query) => query = query.Where(x => x.job == job)));
        }
        filterOptions.Add(new("직업", options.ToList()));
    }

    public void OnFilterChanged()
    {
        ChangeFilter();
        RunQuery();
    }

    void ChangeFilter()
    {
        int index = filterDropdown.value;
        List<DropDownOptions> options = filterOptions[index].options;

        List<string> optionNames = options.Select(x => x.text).ToList();
        filterDropdown2.ClearOptions();
        filterDropdown2.AddOptions(optionNames);
    }

    #endregion

    #region Query
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
        DropDownOptions filter = filterOptions[filterDropdown.value].options[filterDropdown2.value];
        filter.action(ref query);
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

    #endregion

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
        ScrollData.filter2 = filterDropdown2.value;
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
