using Refined.EasyHospital.Base;
using System;
using System.Threading.Tasks;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// Dapper repository interface for user-hospital
    /// </summary>
    public interface IUserHospitalDapperRepository : IBaseDapperRepository<UserHospital, Guid>
    {
        /// <summary>
        /// Asynchronously get user-hospital by user id and hospital id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns></returns>
        Task<UserHospital> GetByUserIdAndHospitalIdAsync(Guid userId, Guid hospitalId);

        /// <summary>
        /// Asynchronously get user-hospital by user id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        Task<UserHospital> GetByUserIdAsync(Guid userId);
    }
}
