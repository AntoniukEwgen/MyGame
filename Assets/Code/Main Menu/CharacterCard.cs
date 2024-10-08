using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

namespace Vampire
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private RectTransform characterImageRect;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI mvspdText;
        [SerializeField] private TextMeshProUGUI luckText;
        [SerializeField] private Button BuyButton;
        [SerializeField] private Button SelectButton;
        [SerializeField] private TextMeshProUGUI SelectButtonText;
        [SerializeField] private TextMeshProUGUI BuyButtonText;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Color selectColor, buyColor;
        [SerializeField] private RectTransform startingAbilitiesParent;
        [SerializeField] private GameObject startingAbilityContainerPrefab;
        [SerializeField] private Vector2 startingAbilitiesRectSize = new Vector2(365, 85);
        private CharacterSelector characterSelector;
        private CharacterBlueprint characterBlueprint;
        private CoinDisplay coinDisplay;
        private StartingAbilityContainer[] startingAbilityContainers;
        private bool initialized;

        public void Init(CharacterSelector characterSelector, CharacterBlueprint characterBlueprint, CoinDisplay coinDisplay)
        {
            this.characterSelector = characterSelector;
            this.characterBlueprint = characterBlueprint;
            this.coinDisplay = coinDisplay;

            characterImage.sprite = characterBlueprint.walkSpriteSequence[0];

            nameText.text = characterBlueprint.name.ToString();
            hpText.text = characterBlueprint.hp.ToString();
            armorText.text = characterBlueprint.armor.ToString();
            mvspdText.text = Mathf.RoundToInt(characterBlueprint.movespeed / 1.15f * 100f).ToString() + "%";
            luckText.text = characterBlueprint.luck.ToString();
            UpdateButtonText();
            buttonImage.color = characterBlueprint.owned ? selectColor : buyColor;

            // Instantiate the images
            startingAbilityContainers = new StartingAbilityContainer[characterBlueprint.startingAbilities.Length];
            for (int i = 0; i < characterBlueprint.startingAbilities.Length; i++)
            {
                startingAbilityContainers[i] = Instantiate(startingAbilityContainerPrefab, startingAbilitiesParent).GetComponent<StartingAbilityContainer>();
                startingAbilityContainers[i].AbilityImage.sprite = characterBlueprint.startingAbilities[i].GetComponent<Ability>().Image;
            }

            initialized = true;

            UpdateButtonText();
        }

        public void UpdateLayout()
        {
            // Character image layout
            float yHeight = Mathf.Abs(characterImageRect.sizeDelta.y);
            float xWidth = characterBlueprint.walkSpriteSequence[0].textureRect.width / (float)characterBlueprint.walkSpriteSequence[0].textureRect.height * yHeight;
            if (xWidth > Mathf.Abs(characterImageRect.sizeDelta.x))
            {
                xWidth = Mathf.Abs(characterImageRect.sizeDelta.x);
                yHeight = characterBlueprint.walkSpriteSequence[0].textureRect.height / (float)characterBlueprint.walkSpriteSequence[0].textureRect.width * xWidth;
            }
           // ((RectTransform)characterImage.transform).sizeDelta = new Vector2(xWidth, yHeight);

            // Character abilities layout
            float maxImageWidth = startingAbilitiesRectSize.x / startingAbilityContainers.Length;
            for (int i = 0; i < startingAbilityContainers.Length; i++)
            {
                StartingAbilityContainer startingAbilityContainer = startingAbilityContainers[i];
                float imageHeight = startingAbilitiesRectSize.y;
                float imageWidth = startingAbilityContainer.AbilityImage.sprite.textureRect.width / (float)startingAbilityContainer.AbilityImage.sprite.textureRect.height * imageHeight;
                if (imageWidth > maxImageWidth)
                {
                    imageWidth = maxImageWidth;
                    imageHeight = startingAbilityContainer.AbilityImage.sprite.textureRect.height / (float)startingAbilityContainer.AbilityImage.sprite.textureRect.width * imageWidth;
                }
                startingAbilityContainer.ImageRect.sizeDelta = new Vector2(imageWidth, imageHeight);
            }
        }

        public void Selected()
        {
            SoundPlayer.Instance.PlaySound("Click");
            if (!characterBlueprint.owned)
            {
                int coinCount = PlayerPrefs.GetInt("Coins");
                if (coinCount >= characterBlueprint.cost)
                {
                    PlayerPrefs.SetInt("Coins", coinCount - characterBlueprint.cost);
                    characterBlueprint.owned = true;
                    UpdateButtonText();
                    buttonImage.color = selectColor;
                    coinDisplay.UpdateDisplay();
                }
            }
            else
            {
                characterSelector.StartGame(characterBlueprint);
            }
        }

        private void UpdateButtonText()
        {
            if (!initialized) return;

            if (characterBlueprint.owned)
            {
                BuyButton.gameObject.SetActive(false);
                SelectButton.gameObject.SetActive(true);
            }
            else
            {
                BuyButton.gameObject.SetActive(true);
                SelectButton.gameObject.SetActive(false);
                BuyButtonText.text = String.Format("{0}", characterBlueprint.cost);
            }
        }
    }
}
