using Refined.EasyHospital.Base;
using System;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Dapper repository interface for Hospital
    /// </summary>
    public interface IHospitalDapperRepository : IBaseDapperRepository<Hospital, Guid>
    {
    }
}
