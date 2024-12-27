import type { AuditedEntityDto } from '@abp/ng.core';
import type { ExtendedPagedAndSortedResultRequestDto } from '../base/models';

export interface HospitalCreateDto {
  name: string;
  provinceCode: string;
  districtCode: string;
  communeCode: string;
}

export interface HospitalDto extends AuditedEntityDto<string> {
  name?: string;
  provinceCode?: string;
  provinceName?: string;
  districtCode?: string;
  districtName?: string;
  communeCode?: string;
  communeName?: string;
}

export interface HospitalPagedAndSortedResultRequestDto extends ExtendedPagedAndSortedResultRequestDto {
  provinceCode?: string;
  districtCode?: string;
  communeCode?: string;
}

export interface HospitalUpdateDto {
  name: string;
  provinceCode: string;
  districtCode: string;
  communeCode: string;
}
