using UnityEngine;

namespace Client
{
    public class CardEvents : MonoBehaviour
    {
        public delegate void BuisnessUpgrade(int id, EventEnum type);
        public static BuisnessUpgrade ClickLvlUpgrade;
        public static BuisnessUpgrade ClickFirstUpgrade;
        public static BuisnessUpgrade ClickSecondUpgrade;

        public void LvlUP(BuisnessCard card)
        {
            ClickLvlUpgrade?.Invoke(card.CardID, EventEnum.LvlUP);
        }

        public void FirstUpgrade(BuisnessCard card)
        {
            ClickFirstUpgrade?.Invoke(card.CardID, EventEnum.FirstUpgrade);
        }

        public void SecondUpgrade(BuisnessCard card)
        {
            ClickSecondUpgrade?.Invoke(card.CardID, EventEnum.SecondUpgrade);
        }
    }
}
