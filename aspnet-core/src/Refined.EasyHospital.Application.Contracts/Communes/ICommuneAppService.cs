using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// App service interface for Commune
    /// </summary>
    public interface ICommuneAppService :
        ICrudAppService<CommuneDto, Guid, PagedAndSortedResultRequestDto, CommuneCreateDto, CommuneUpdateDto>
    {
    }
}
