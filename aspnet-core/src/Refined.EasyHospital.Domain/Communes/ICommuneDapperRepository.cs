using Refined.EasyHospital.Base;
using System;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Dapper repository interface for Commune
    /// </summary>
    public interface ICommuneDapperRepository : IBaseDapperRepository<Commune, Guid>
    {
    }
}
