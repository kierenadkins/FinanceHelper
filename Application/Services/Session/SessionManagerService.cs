using Application.Services.Sterializer;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Core.Services.Session
{
    public class SessionManagerService : ISessionManagerService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ISterializerService _serializer;

        private const string SessionNotConfigured = "Session has not been configured for this application or request.";

        public SessionManagerService(IHttpContextAccessor contextAccessor, ISterializerService serializer)
        {
            _serializer = serializer;
            _context = contextAccessor;
        }

        public void Set<T>(T obj, string key)
        {
            var value = _serializer.SerializeObject(obj);

            try
            {
                _context.HttpContext?.Session?.SetString(key, value);
            }
            catch (Exception ex) when (string.Equals(ex.Message, SessionNotConfigured, StringComparison.InvariantCultureIgnoreCase))
            {
                // ignored
            }
        }

        public T Get<T>(string key)
        {
            if (!IsSet(key))
                return default(T);

            var value = string.Empty;

            try
            {
                value = _context.HttpContext?.Session?.GetString(key);
            }
            catch (Exception ex) when (string.Equals(ex.Message, SessionNotConfigured, StringComparison.InvariantCultureIgnoreCase))
            {
                // ignored
            }

            var item = _serializer.DeserializeObject<T>(value);

            return item;
        }

        public void ClearAll()
        {
            try
            {
                _context.HttpContext?.Session?.Clear();
            }
            catch (Exception ex) when (string.Equals(ex.Message, SessionNotConfigured, StringComparison.InvariantCultureIgnoreCase))
            {
                // ignored
            }
        }

        public bool IsSet(string key)
        {
            try
            {
                return (_context.HttpContext?.Session?.Keys.Contains(key)) ?? false;
            }
            catch (Exception ex) when (string.Equals(ex.Message, SessionNotConfigured, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
        }

        public void Remove(string key)
        {
            try
            {
                _context.HttpContext?.Session?.Remove(key);
            }
            catch (Exception ex) when (string.Equals(ex.Message, SessionNotConfigured, StringComparison.InvariantCultureIgnoreCase))
            {
                // ignored
            }
        }
    }
}