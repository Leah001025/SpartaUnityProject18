using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static GameManager s_gameManager = new GameManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static SceneManagers s_sceneManager = new SceneManagers();
    private static SoundManager s_soundManager = new SoundManager();

    public static GameManager Game { get { Init(); return s_gameManager; } }
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static SceneManagers Scene { get { Init(); return s_sceneManager; } }
    public static SoundManager Sound { get { Init(); return s_soundManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);

            s_resourceManager.Init();
            //s_sceneManager.Init();
            s_soundManager.Init();

            Application.targetFrameRate = 60;
        }
    }
}
