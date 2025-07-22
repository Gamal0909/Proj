using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Series.Core
{
    public class PlayerEquipmentSystem : MonoBehaviour
    {
        [SerializeField] GameObject swordHolder;
        [SerializeField] GameObject swordWithEffects;
        [SerializeField] GameObject swordNormal;
        [SerializeField] GameObject swordSheath;

        GameObject currentSwordInHand;
        GameObject currentSwordInSheath;

        void Start()
        {
            currentSwordInSheath = Instantiate(swordNormal, swordSheath.transform);
        }

        public void DrawSword()
        {
            currentSwordInHand = Instantiate(swordWithEffects, swordHolder.transform);
            Destroy(currentSwordInSheath);
        }

        public void SheathSword()
        {
            Destroy(currentSwordInSheath);
            currentSwordInSheath = Instantiate(swordNormal, swordSheath.transform);
            Destroy(currentSwordInHand);
        }
    }

}
