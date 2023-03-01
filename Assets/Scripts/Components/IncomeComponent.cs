namespace Client 
{
    struct IncomeComponent 
    {
        private int _money;
        public int Money => _money;

        public IncomeComponent(int income)
        {
            _money = income;
        }
    }
}