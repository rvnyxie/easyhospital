import type { UserHospitalCreateDto, UserHospitalDto, UserHospitalUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExtendedPagedAndSortedResultRequestDto } from '../base/models';

@Injectable({
  providedIn: 'root',
})
export class UserHospitalService {
  apiName = 'Default';
  

  create = (userHospitalCreateDto: UserHospitalCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'POST',
      url: '/api/app/user-hospital',
      body: userHospitalCreateDto,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/user-hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'GET',
      url: `/api/app/user-hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: ExtendedPagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserHospitalDto>>({
      method: 'GET',
      url: '/api/app/user-hospital',
      params: { search: input.search, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UserHospitalUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'PUT',
      url: `/api/app/user-hospital/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
