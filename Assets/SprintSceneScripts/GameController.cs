using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Button saveButton;
    public Button loadButton;

    public GameObject playerPrefab;
    public const string playerPath = "Prefabs/JustaPrefab";

    private static string dataPath = string.Empty;

    void Awake()
    {
        dataPath = System.IO.Path.Combine(Application.persistentDataPath, "actors.json");
    }

    void Start()
    {
        //CreateActor(playerPath, new Vector3(0, 1.6f, 0), Quaternion.identity);
        //CreateActor(playerPath, new Vector3(5, 1.6f, 0), Quaternion.identity);
        //CreateActor(playerPath, new Vector3(-5, 1.6f, 0), Quaternion.identity);
    }

    public static SaveStuff CreateActor(string path, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>(path);

        GameObject go = Instantiate(prefab, position, rotation) as GameObject;

        SaveStuff actor = go.GetComponent<SaveStuff>() ?? go.AddComponent<SaveStuff>();

        return actor;
    }

    public static SaveStuff CreateActor(ActorData data, string path, Vector3 position, Quaternion rotation)
    {
        SaveStuff actor = CreateActor(path, position, rotation);

        actor.data = data;

        return actor;
    }

    public void Save()
    {
        SaveData.Save(dataPath, SaveData.actorContainer);
    }

    /*public void Load()
    {
        SaveData.Load(dataPath);
    }*/

    void OnEnable()
    {
        saveButton.onClick.AddListener(Save);
        //loadButton.onClick.AddListener(Load);
    }
    void OnDisable()
    {
        saveButton.onClick.RemoveListener(Save);
        //loadButton.onClick.RemoveListener(Load);
    }
}
