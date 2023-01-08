  //https://www.talkingdotnet.com/store-complex-objects-in-asp-net-core-session/#:~:text=In%20ASP.NET%20Core%2C%20the,the%20serialization%20to%20byte%20arrays.&text=That's%20it.
    using Newtonsoft.Json;
    
    namespace quizApp.Session;
    
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }