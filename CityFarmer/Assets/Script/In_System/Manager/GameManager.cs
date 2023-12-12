using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;
    public static GameManager Instance { get { Init(); return _instance; } }

    private static EncyclopediaManager _encyclopediaManager;
    private static SoundManager _soundManager;
    private static InventoryManager _inventoryManager;
  
    public static EncyclopediaManager EncyclopediaManager { get { Init(); return _encyclopediaManager; } }
    public static SoundManager SoundManager { get{ Init();  return _soundManager; } }
    public static InventoryManager InventoryManager { get { Init(); return _inventoryManager; } }

    private float _gameTime = 0.0f;

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject gameObject = FindObjectOfType(typeof(GameManager)) as GameObject;

            if (gameObject == null)
            {
                gameObject = new GameObject("GameManager");
            }

            _instance = GetOrAddComponent<GameManager>(gameObject);
            DontDestroyOnLoad(gameObject);

            _soundManager = CreateManager<SoundManager>();
            _encyclopediaManager = CreateManager<EncyclopediaManager>();
            _inventoryManager= CreateManager<InventoryManager>();
            _inventoryManager.Init();
            _soundManager.Init();
            _encyclopediaManager.Init();
        }

    }

    private static T CreateManager<T>() where T : UnityEngine.Component
    {
        GameObject go = new GameObject($"@{typeof(T)}");
        T result = go.AddComponent<T>();
        go.transform.SetParent(_instance.transform);

        return result;
    }

    public  static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }
    private void Update()
    {
        _gameTime += Time.deltaTime;
        if(_gameTime >= 3.0f)
        {
            InfoManager.Instance.UpdateSQL(InfoManager.Instance.WaterUpdateString());
            _gameTime = 0.0f;
        }
    }
}
