namespace TimelineHero.Core
{
    public class Singleton<T> where T : class
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                _instance = _instance ?? default(T);
                return _instance;
            }
        }
    }
}