using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vampire
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] protected CharacterBlueprint[] characterBlueprints;
        [SerializeField] protected GameObject characterCardPrefab;
        [SerializeField] protected CoinDisplay coinDisplay;

        [SerializeField] DialogBox levelMenu;

        private CharacterCard[] characterCards;
        
        public void Init()
        {
            //levelMenu.Close();
            characterCards = new CharacterCard[characterBlueprints.Length];
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i] = Instantiate(characterCardPrefab, this.transform).GetComponent<CharacterCard>();
                characterCards[i].Init(this, characterBlueprints[i], coinDisplay);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i].UpdateLayout();
            }
        }
        
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            CrossSceneData.CharacterBlueprint = characterBlueprint;
            levelMenu.Open();
        }
    }
}
