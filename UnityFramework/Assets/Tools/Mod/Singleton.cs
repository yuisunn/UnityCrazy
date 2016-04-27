using System.Collections;

namespace SPSGame.Tools
{
    public class Singleton<T> where T : class,new()
    {
        private static T _Instance;
        public static T Instance
        {
            get
            {

                if (null == _Instance)
                    _Instance = new T();
                return _Instance;

            }
        }

    }
}