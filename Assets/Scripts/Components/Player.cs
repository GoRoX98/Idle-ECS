namespace Client 
{
    public struct Player
    {
        private int _money;
        public int Money => _money;

        public Player(int startMoney)
        {
            _money = startMoney;
        }

        public void Income(int income)
        {
            _money += income;
        }
    }
}