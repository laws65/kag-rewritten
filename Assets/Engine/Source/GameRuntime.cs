using UnityEngine;

namespace KAG
{
    public class GameRuntime : Singleton<GameRuntime>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
