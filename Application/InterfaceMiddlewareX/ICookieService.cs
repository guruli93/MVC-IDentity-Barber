
using Microsoft.AspNetCore.Http;

namespace Application.InterfaceMiddlewareX
{
    public interface ICookieService
    {
        string Get(string key);
        void Set(string key, string value, CookieOptions options);
        void Delete(string key);
    }

}
