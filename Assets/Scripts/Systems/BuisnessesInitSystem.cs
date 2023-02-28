using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class BuisnessesInitSystem : IEcsInitSystem 
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;
        private EcsFilter<SaveComponent> _save;

        public void Init () 
        {
            RectTransform bpanel = _sceneData.BuisnessesPanel.GetComponent<RectTransform>();
            float cardHeight = _staticData.Prefab.GetComponent<RectTransform>().rect.height + _sceneData.BuisnessesPanel.GetComponent<VerticalLayoutGroup>().spacing;

            foreach (var buis in _staticData.Buienesses)
            {
                EcsEntity buisnessEntity = _world.NewEntity();
                ref BuisnessComponent bc = ref buisnessEntity.Get<BuisnessComponent>();

                if (_save.Get1(0).Buisnesses == null)
                    FirstStart(ref bc, buis);
                else
                    LoadBuisnessData(ref bc, buis);

                GameObject buisObj = Object.Instantiate(_staticData.Prefab, _sceneData.BuisnessesPanel.transform);
                BuisnessCard card = buisObj.GetComponent<BuisnessCard>();
                UpdateBuisnessCard(card, bc);

                bpanel.sizeDelta += new Vector2(0, cardHeight);
            }
        }

        private void FirstStart(ref BuisnessComponent bc, Buisness buisness)
        {
            bc.Buisness = buisness;
            bc.CurrentLVL = buisness.BuisnessConfiguration.BaseBuisness ? 1 : 0;
            bc.RevenueProgress = 0;
            bc.FirstUpgraded = false;
            bc.SecondUpgraded = false;
        }

        private void LoadBuisnessData(ref BuisnessComponent bc, Buisness buisness)
        {
            bc = _save.Get1(0).Buisnesses.Find(match => match.Buisness == buisness);
        }

        private void UpdateBuisnessCard(BuisnessCard card, BuisnessComponent buis)
        {
            card.BuisnessName.text = buis.Buisness.name;
            card.CurrentLvl.text = buis.CurrentLVL.ToString();

            float revenutMultiplyer = 1;
            if (buis.FirstUpgraded)
                revenutMultiplyer += buis.Buisness.BuisnessConfiguration.FirstUpgrade.RevenueMultiplier;
            if(buis.SecondUpgraded)
                revenutMultiplyer += buis.Buisness.BuisnessConfiguration.SecondUpgrade.RevenueMultiplier;

            card.CurrentIncome.text = (buis.CurrentLVL * buis.Buisness.BuisnessConfiguration.BaseRevenue * revenutMultiplyer).ToString();
            card.LvlUP.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + ((buis.CurrentLVL + 1) * buis.Buisness.BuisnessConfiguration.BaseCost).ToString();

            card.RevenueProgress.value = buis.RevenueProgress;

            card.Upgrade1.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + buis.Buisness.BuisnessConfiguration.FirstUpgrade.Cost;
            card.Upgrade2.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + buis.Buisness.BuisnessConfiguration.SecondUpgrade.Cost;
            
            card.Upgrade1.transform.Find("Revenue").GetComponent<TextMeshProUGUI>().text = $"Revenue: +{buis.Buisness.BuisnessConfiguration.FirstUpgrade.RevenueMultiplier * 100}%";
            card.Upgrade2.transform.Find("Revenue").GetComponent<TextMeshProUGUI>().text = $"Revenue: +{buis.Buisness.BuisnessConfiguration.SecondUpgrade.RevenueMultiplier * 100}%";
        }
    }
}