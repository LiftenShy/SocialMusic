using DataLayer.Models.BaseModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccessControl.Buisness.Interfaces
{
    public interface IConnector<T>
        where T : BaseEntity
    {
        Task<HttpResponseMessage> Get(string requestUri);

        Task<HttpResponseMessage> Post(string requestUri, T value);

        Task<HttpResponseMessage> Put(string requestUri, T value);

        Task<HttpResponseMessage> Patch(string requestUri, T value);

        Task<HttpResponseMessage> Delete(string requestUri);
    }
}
