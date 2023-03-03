using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class BuisnessesSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;
        
        private EcsFilter<SaveComponent> _filterSave;
        private EcsFilter<BuisnessComponent> _filterBuisnesses;
        private EcsFilter<BuisCardComponent> _filterCards;
        private EcsFilter<UpgradeSuccessComponentFlag> _filterUpgrade;

        public void Init()
        {
            RectTransform bpanel = _sceneData.BuisnessesPanel.GetComponent<RectTransform>();
            float cardHeight = _staticData.Prefab.GetComponent<RectTransform>().rect.height + _sceneData.BuisnessesPanel.GetComponent<VerticalLayoutGroup>().spacing;

            foreach (var buis in _staticData.Buienesses)
            {
                EcsEntity buisnessEntity = _world.NewEntity();
                ref BuisnessComponent bc = ref buisnessEntity.Get<BuisnessComponent>();

                EcsEntity cardEntity = _world.NewEntity();
                ref BuisCardComponent cardComponent = ref cardEntity.Get<BuisCardComponent>();

                if (_filterSave.Get1(0).Buisnesses == null)
                    FirstStart(ref bc, buis);
                else
                    LoadBuisnessData(ref bc, buis);

                GameObject buisObj = Object.Instantiate(_staticData.Prefab, _sceneData.BuisnessesPanel.transform);
                BuisnessCard card = buisObj.GetComponent<BuisnessCard>();
                card.CardID = bc.BuisnessID;
                CacheCard(card, ref cardComponent);

                UpdateBuisnessCard(cardComponent, bc);

                bpanel.sizeDelta += new Vector2(0, cardHeight);
            }
        }

        public void Run()
        {
            RevenueProgress();
            if(!_filterUpgrade.IsEmpty())
            {
                for(int i = 0; i < _filterBuisnesses.GetEntitiesCount(); i++)
                {
                    UpdateBuisnessCard(_filterCards.Get1(i), _filterBuisnesses.Get1(i));
                }
                _filterUpgrade.GetEntity(0).Destroy();
            }
        }

        #region WorkWithData

        private void FirstStart(ref BuisnessComponent bc, Buisness buisness)
        {
            bc.BuisnessID = buisness.ID;
            bc.CurrentLVL = buisness.BuisnessConfiguration.BaseBuisness ? 1 : 0;
            bc.RevenueProgress = 0;
            bc.FirstUpgraded = false;
            bc.SecondUpgraded = false;
        }

        private void LoadBuisnessData(ref BuisnessComponent bc, Buisness buisness)
        {
            bc = _filterSave.Get1(0).Buisnesses.Find(match => match.BuisnessID == buisness.ID);
        }

        private void CacheCard(BuisnessCard card, ref BuisCardComponent buisCC)
        {
            buisCC.Card = card;
            buisCC.BuisnessName = card.BuisnessName;
            buisCC.CurrentLvl = card.CurrentLvl;
            buisCC.CurrentIncome = card.CurrentIncome;
            buisCC.LvlUpCost = card.LvlUP.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            buisCC.Upgrade1Cost = card.Upgrade1.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            buisCC.Upgrade1Revenue = card.Upgrade1.transform.Find("Revenue").GetComponent<TextMeshProUGUI>();
            buisCC.Upgrade2Cost = card.Upgrade2.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            buisCC.Upgrade2Revenue = card.Upgrade2.transform.Find("Revenue").GetComponent<TextMeshProUGUI>();
        }

        private void UpdateBuisnessCard(BuisCardComponent card, BuisnessComponent buis)
        {
            BuisnessConfig config = _staticData.Buienesses.Find(match => match.ID == buis.BuisnessID).BuisnessConfiguration;
            card.BuisnessName.text = _staticData.Buienesses.Find(match => match.ID == buis.BuisnessID).Name;
            card.CurrentLvl.text = "LVL\n" + buis.CurrentLVL.ToString();

            int income = CalculateIncome(buis);

            card.CurrentIncome.text = "Income\n" + income.ToString() + "$";
            card.LvlUpCost.text = "Cost: " + ((buis.CurrentLVL + 1) * config.BaseCost).ToString();

            card.Card.RevenueProgress.value = buis.RevenueProgress;

            card.Upgrade1Cost.text = "Cost: " + config.FirstUpgrade.Cost;
            card.Upgrade2Cost.text = "Cost: " + config.SecondUpgrade.Cost;
            
            card.Upgrade1Revenue.text = $"Revenue: +{config.FirstUpgrade.RevenueMultiplier * 100}%";
            card.Upgrade2Revenue.text = $"Revenue: +{config.SecondUpgrade.RevenueMultiplier * 100}%";

            if (buis.FirstUpgraded)
                card.Card.Upgrade1.interactable = false;
            if (buis.SecondUpgraded)
                card.Card.Upgrade2.interactable = false;
        }

        private int CalculateIncome(BuisnessComponent bc)
        {
            float revenutMultiplyer = 1;
            if (bc.FirstUpgraded)
                revenutMultiplyer += _staticData.Buienesses[bc.BuisnessID].BuisnessConfiguration.FirstUpgrade.RevenueMultiplier;
            if (bc.SecondUpgraded)
                revenutMultiplyer += _staticData.Buienesses[bc.BuisnessID].BuisnessConfiguration.SecondUpgrade.RevenueMultiplier;

            return (int)(bc.CurrentLVL * _staticData.Buienesses[bc.BuisnessID].BuisnessConfiguration.BaseRevenue * revenutMultiplyer);
        }

        private void UpdateRevenue(BuisnessComponent bc, int index)
        {
            _filterCards.Get1(index).Card.RevenueProgress.value = bc.RevenueProgress;
        }

        #endregion

        #region GameProcess

        private void RevenueProgress()
        {
            foreach(var i in _filterBuisnesses)
            {
                ref BuisnessComponent bc = ref _filterBuisnesses.Get1(i);

                if (bc.CurrentLVL <= 0)
                    continue;

                bc.RevenueProgress += Time.deltaTime / _staticData.Buienesses.Find(match => match.ID == _filterBuisnesses.Get1(i).BuisnessID).BuisnessConfiguration.RevenueDelay;
                if(bc.RevenueProgress >= 1)
                {
                    bc.RevenueProgress = 0;
                    UpdateRevenue(bc, i);
                    EcsEntity incomeEntity = _world.NewEntity();
                    ref IncomeComponent income = ref incomeEntity.Get<IncomeComponent>();
                    income = new IncomeComponent(CalculateIncome(bc));
                }
                else
                    UpdateRevenue(bc, i);
            }
        }

        #endregion
    }
}