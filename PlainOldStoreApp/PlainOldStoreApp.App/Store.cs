using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Store
    {
        private Dictionary<int, string> _stores = new Dictionary<int, string>();

        internal string GetStore(int key)
        {
            string value = "";
            if (_stores.ContainsKey(key))
            {
                value = _stores[key];
            }
            return value;
        }

        private readonly IStoreRepository _storeRepository;

        internal Store(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        internal Dictionary<int,string> GetStoresFromDatabase()
        {
           _stores = _storeRepository.RetriveStores();
            return _stores;
        }
    }
}
