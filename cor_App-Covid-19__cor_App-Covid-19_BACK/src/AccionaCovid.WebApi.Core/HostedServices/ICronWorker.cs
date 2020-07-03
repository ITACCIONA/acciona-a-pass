using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICronWorker
    {
        Task DoWork();
    }
}
