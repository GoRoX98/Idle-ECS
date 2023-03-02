namespace Client 
{
    struct UpgradeComponent 
    {
        private int _buisnessID;
        private EventEnum _type;

        public int BuisnessID => _buisnessID;
        public EventEnum TypeUpgrade => _type;

        public UpgradeComponent(int ID, EventEnum type)
        {
            _buisnessID = ID;
            _type = type;
        }
    }
}