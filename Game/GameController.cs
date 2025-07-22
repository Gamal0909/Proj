using UnityEngine;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Game/Game Controller")]
    public class GameController : MonoBehaviour
    {
        protected Game m_game => Game.instance;
        protected GameLoader m_loader => GameLoader.instance;

        public virtual void AddRetries(int amount) => m_game.retries += amount;

        public virtual void LoadScene(string scene) => m_loader.Load(scene);
    }
}
