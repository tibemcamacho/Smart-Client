using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IAppAuthorization
    {
        List<AppAuthorizationEntity> GetAll();
        bool Exist(string CodeAction);

        AppAuthorizationEntity Contains(string CodeAction);
    }
}
