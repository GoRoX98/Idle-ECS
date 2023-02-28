using System.Collections.Generic;

namespace Client 
{
    [System.Serializable]
    struct SaveComponent 
    {
        public int Money;
        public List<BuisnessComponent> Buisnesses;
    }
}