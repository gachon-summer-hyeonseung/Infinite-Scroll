using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public struct UserData
{
    public string name;
    public int age;
    public string gender;
    public string hobby;
    public string job;
}

public class UserManager : MonoBehaviour
{


    [SerializeField] private int userCount = 500;

    [Header("User Name Data")]
    [SerializeField]
    private string[] lastNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임", "한", "오", "서", "신", "권", "황", "안", "송", "류", "전", "홍", "고", "문", "양", "손", "배", "조", "백", "허", "유", "남", "심", "노", "정", "하", "곽", "성", "차", "주", "우", "구", "신", "임", "나", "전", "민", "유", "진", "지", "엄", "채", "원", "천", "방", "공", "강", "현", "함", "변", "염", "양", "변", "여", "추", "노", "도", "소", "신", "석", "선", "설", "마", "길", "주", "연", "방", "위", "표", "명", "기", "반", "왕", "금", "옥", "육", "인", "맹", "제", "모", "장", "남", "탁", "국", "여", "진", "어", "은", "편", "구", "용", "남궁", "독고", "동방", "선우", "서문" };
    [SerializeField]
    private string[] firstNames = new string[] { "상아", "찬영", "다헌", "건영", "명진", "명성", "상연", "찬연", "다형", "건우", "명세", "상영", "찬우", "다환", "건호", "명민", "상완", "찬원", "다훈", "건훈", "명재", "대언", "상우", "찬헌", "대연", "건율", "명제", "상원", "찬현", "대영", "건희", "명준", "상윤", "찬호", "대원", "건후", "명찬", "민서", "찬후", "대윤", "민준", "관영", "상헌", "찬율", "대은", "관우", "민찬", "채안", "상훈", "상현", "상호", "상환", "상후", "상희", "상율", "상일", "상엽", "상혁", "상수", "상준", "상민", "상빈", "서환", "서후", "서준", "서진", "서빈", "선후", "성영", "성완", "성우", "성원", "성윤", "성헌", "성현", "성훈", "성호", "성후", "성희", "성율", "성하", "성한", "성은", "성일", "성혁", "성엽", "성수", "성재", "성진", "성준", "성찬", "성민", "성빈", "세영", "세은", "세완", "세원", "세윤", "세현", "세훈", "세호", "세후", "세율", "세희", "세한", "세일", "세혁", "세준", "세진", "세민", "세빈", "세명", "지운", "송후", "송혁", "송민", "승연", "수헌", "수한", "수혁", "수성", "수민", "수빈", "승현", "승완", "승우", "승원", "승윤", "승헌", "승훈", "승한", "승호", "승환", "승후", "승일", "승엽", "승혁", "승진", "승준", "승민", "승빈", "시훈", "시윤", "시환", "시율", "시우", "시원", "시후", "시혁", "시헌", "시진", "시준", "시민", "장완", "장원", "장우", "장윤", "장헌", "장훈", "장현", "장호", "장혁", "재영", "재우", "재원", "재헌", "재훈", "재현", "재호", "재환", "재율", "재일", "재혁", "재준", "재진", "재찬", "재성", "재민", "재빈", "정현", "정연", "정환", "정안", "정헌", "정후", "정혁", "정준", "정찬", "정수", "정민", "정빈", "제영", "제우", "제헌", "제현", "제훈", "제환", "제민", "제빈", "조영", "조원", "종연", "종완", "종우", "종원", "종윤", "종헌", "종훈", "종현", "종호", "종환", "종후", "종한", "종일", "종혁", "종수", "종성", "종찬", "종민", "종재", "종빈", "종명", "주호", "주환", "주한", "주헌", "주훈", "주혁", "주찬", "주성", "주빈", "준영", "준아", "준연", "준완", "준우", "준원", "준현", "준헌", "준호", "준후", "준일", "준혁", "준상", "준성", "지완", "지한", "지헌", "지율", "지환", "지호", "지후", "지혁", "지민", "지빈", "지명", "진영", "진혁", "차훈", "차민", "차빈", "진호", "차헌", "재연", "주원", "찬혁", "채언", "채헌", "채훈", "채호", "채혁", "채민", "채빈", "청현", "청헌", "정훈", "청호", "청하", "청환", "청완", "초한", "초헌", "청연", "초훈", "선우", "성연", "서훈", "대율", "대인", "대한", "대현", "대형", "대환", "대훈", "대경", "대권", "대규", "대융", "대우", "대후", "도영", "도원", "도윤", "도율", "도헌", "도현", "도훈", "동언", "동연", "동영", "동예", "동완", "동원", "동운", "동윤", "동은", "동율", "동인", "동한", "동해", "동헌", "동현", "동혜", "동환", "동훈", "동희", "동후", "동우", "두윤", "두율", "두환", "두훈", "래헌", "래환", "두영", "태영", "태원", "태윤", "태은", "태율", "태한", "태헌", "태현", "태환", "태훈", "세연", "소율", "시현", "조현", "정원", "제연", "관호", "관훈", "관율", "관희", "관후", "권우", "권호", "권율", "광현", "광연", "광헌", "보성", "범성", "기영", "광우", "필준", "민승", "권영", "도연", "민건", "민겸", "민국", "민관", "민규", "민기", "민상", "민세", "민성", "민종", "민진", "민창", "범찬", "범교", "범준", "범규", "범기", "범상", "범세", "범창" };

    [Header("User Age Data")]
    [SerializeField] private int minAge = 20;
    [SerializeField] private int maxAge = 60;

    [Header("User Gender Data")]
    [SerializeField] private string[] genders = new string[] { "남", "여", "선택안함" };
    [Header("User Hobby Data")]
    [SerializeField] private string[] hobbies = new string[] { "독서", "등산", "영화감상", "게임", "요리", "운동", "여행", "음악감상", "그림 그리기", "코딩" };

    [Header("User Job Data")]
    [SerializeField] private string[] jobs = new string[] { "프로그래밍", "기획", "QA" };

    List<UserData> cachedUserDataList;

    public UserData selectedUserData { get; private set; }

    public static UserManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // private void Start()
    // {
    //     CreateUserData();
    // }

    public void RefreshData()
    {
        cachedUserDataList = null;
    }

    public List<UserData> GetUserData()
    {
        if (cachedUserDataList != null) return cachedUserDataList;

        List<UserData> userDatas = new();

        string filePath = Path.Combine(Application.persistentDataPath, "UserData.csv");
        if (!File.Exists(filePath))
        {
            return userDatas;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] data = lines[i].Split(',');

            UserData userData = new()
            {
                name = data[0],
                age = int.Parse(data[1]),
                gender = data[2],
                hobby = data[3],
                job = data[4]
            };

            userDatas.Add(userData);
        }

        cachedUserDataList = userDatas;
        return userDatas;
    }

    public void SetUserData(UserData userData)
    {
        selectedUserData = userData;
    }

    public void CreateUserData()
    {
        List<UserData> userDatas = new();

        for (int i = 0; i < userCount; i++)
        {
            string lastName = lastNames[Random.Range(0, lastNames.Length)];
            string firstName = firstNames[Random.Range(0, firstNames.Length)];
            string name = lastName + firstName;

            int age = Random.Range(minAge, maxAge + 1);

            string gender = genders[Random.Range(0, genders.Length)];

            string hobby = hobbies[Random.Range(0, hobbies.Length)];

            string job = jobs[Random.Range(0, jobs.Length)];

            userDatas.Add(new UserData { name = name, age = age, gender = gender, hobby = hobby, job = job });
        }

        SaveToCSV(userDatas);
    }

    private void SaveToCSV(List<UserData> userDatas)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "UserData.csv");
        Debug.Log(filePath);

        StringBuilder sb = new("이름,나이,성별,취미,직업\n");
        userDatas.ForEach(userData =>
        {
            sb.Append($"{userData.name},{userData.age},{userData.gender},{userData.hobby},{userData.job}\n");
        });

        File.WriteAllText(filePath, sb.ToString());
    }
}
