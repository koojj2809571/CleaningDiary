using System.Collections.Generic;
using Base;
using GameContent.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace GameContent
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [HideInInspector] public Andrew andrew;

        public SaveLoadUtils recordRunData;

        [HideInInspector] public List<BaseEnemy> enemies = new();

        public List<GameObject> levelDoor = new();

        public bool anthonyIsDead;

        public int EnemyCount => enemies.Count;

        private void Awake()
        {
            Instance = this;
            recordRunData = new SaveLoadUtils();
            DontDestroyOnLoad(gameObject);
            recordRunData.InitRecord();
            recordRunData.InitWeapons();
        }

        private void SaveByJson()
        {
        
        }

        public void SwitchScene(int index, float delay)
        {
            switch (index)
            {
                case 0:
                    Invoke(nameof(LoadMain),delay);
                    break;
            }
        }

        private void LoadMain()
        {
            SceneManager.LoadScene(0);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}