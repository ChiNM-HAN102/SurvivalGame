using UnityEngine;

namespace Game.Runtime
{
    public enum GameState
    {
        PAUSING = 0,
        END = 1,
        RUNNING = 2,
        WAITING = 3
    }
    
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private HeroBase[] characterList;

        private GameState _state;
        public GameState State { get => this._state; }
        
        public static GamePlayController Instance { get; set; }

        private void Awake()
        {
            Instance = this;

            InitCharacter();
        }

        void InitCharacter()
        {
            UIManager
        }
        
        
    }
}