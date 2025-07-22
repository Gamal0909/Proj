using UnityEngine;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Level/Level Controller")]
    public class LevelController : MonoBehaviour
    {
        protected LevelFinisher m_finisher => LevelFinisher.instance;
        protected LevelRespawner m_respawner => LevelRespawner.instance;
        protected LevelPauser m_pauser => LevelPauser.instance;

        public virtual void Finish() => m_finisher.Finish();
        public virtual void Exit() => m_finisher.Exit();

        public virtual void Respawn(bool consumeRetries) => m_respawner.Respawn(consumeRetries);
        public virtual void Restart() => m_respawner.Restart();
        

        public virtual void Pause(bool value) => m_pauser.Pause(value);
    }
}

