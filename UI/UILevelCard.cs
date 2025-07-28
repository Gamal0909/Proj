using UnityEngine;
using UnityEngine.UI;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/UI/UI Level Card")]
    public class UILevelCard : MonoBehaviour
    {
        [Header("Texts")]
        public Text title;
        public Text description;

        [Header("Images")]
        public Image image;

        [Header("Buttons")]
        public Button play;

        [Header("Containers")]
        public GameObject playContainer;
        public GameObject missingStarsContainer;

        protected bool m_locked;

        public string scene { get; set; }

        public bool locked
        {
            get { return m_locked; }

            set
            {
                m_locked = value;
                play.interactable = !m_locked;
            }
        }

        public virtual void Play()
        {
            GameLoader.instance.Load(scene);
        }

        public virtual void Fill(GameLevel level)
        {
            if (level != null)
            {
                scene = level.scene;
                title.text = level.name;
                description.text = level.description;
      
                image.sprite = level.image;

                
                HandleLocking(level);
            }
        }

        protected virtual void HandleLocking(GameLevel level)
        {
            if (level.requiredItems > 0)
            {
                var totalStars = Game.instance.GetTotalItems();
                locked = level.requiredItems > totalStars;

                if (locked)
                {
                    playContainer?.SetActive(false);
                    missingStarsContainer?.SetActive(true);
                }

                return;
            }

            locked = level.locked;
        }

        protected virtual void Start()
        {
            play.onClick.AddListener(Play);
        }
    }
}
