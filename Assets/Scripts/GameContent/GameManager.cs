using System.Collections.Generic;
using GameContent.Players;
using UnityEngine;
using Util;

namespace GameContent
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [HideInInspector] public Andrew andrew;

        public SaveLoadUtils recordRunData;

        [HideInInspector] public List<Enemy.Enemy> enemies = new();

        public List<GameObject> levelDoor = new();

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