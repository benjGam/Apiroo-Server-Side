using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApirooServer.Utils
{
    public class Singleton<T> : IDisposable
    {
        protected static object? _instance;
        protected Singleton()
        {
            if (_instance == null)
                _instance = this;
            else
                throw new Exception($"{typeof(T).Name} is singleton class");
        }
        public static T Instance => (T)_instance!;
        public void Dispose()
        {
            GC.SuppressFinalize((T)_instance!);
            GC.SuppressFinalize(this);
        }
    }
}
