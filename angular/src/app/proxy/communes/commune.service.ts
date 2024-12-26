import type { CommuneCreateDto, CommuneDto, CommuneUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LocalityPagedAndSortedResultRequestDto } from '../base/models';

@Injectable({
  providedIn: 'root',
})
export class CommuneService {
  apiName = 'Default';
  

  create = (input: CommuneCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>({
      method: 'POST',
      url: '/api/app/commune',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/commune/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>({
      method: 'GET',
      url: `/api/app/commune/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: LocalityPagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CommuneDto>>({
      method: 'GET',
      url: '/api/app/commune',
      params: { provinceCode: input.provinceCode, districtCode: input.districtCode, communeCode: input.communeCode, search: input.search, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  importByInput = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>({
      method: 'POST',
      url: '/api/app/commune/import',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CommuneUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>({
      method: 'PUT',
      url: `/api/app/commune/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
