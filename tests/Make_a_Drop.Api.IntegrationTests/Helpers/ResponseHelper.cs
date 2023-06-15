using System.Net.Http;
using System.Threading.Tasks;
using Make_a_Drop.Application.Models;
using Newtonsoft.Json;

namespace Make_a_Drop.Api.IntegrationTests.Helpers;

public static class ResponseHelper
{
    public static async Task<ApiResult<T>> GetApiResultAsync<T>(HttpResponseMessage responseMessage)
    {
        return JsonConvert.DeserializeObject<ApiResult<T>>(await responseMessage.Content.ReadAsStringAsync());
    }
}
