using Refined.EasyHospital.Base;
using System;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// Dapper repository interface for user-hospital
    /// </summary>
    public interface IUserHospitalDapperRepository : IBaseDapperRepository<UserHospital, Guid>
    {
    }
}
