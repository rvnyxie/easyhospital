import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';
import type { ExtendedPagedAndSortedResultRequestDto } from '../base/models';

export interface PatientCreateDto {
  code: string;
  name: string;
  provinceCode: string;
  districtCode: string;
  communeCode: string;
  hospitalId?: string;
}

export interface PatientDto extends AuditedEntityDto<string> {
  code?: string;
  name?: string;
  provinceCode?: string;
  districtCode?: string;
  communeCode?: string;
  hospitalId?: string;
}

export interface PatientPagedAndSortedResultRequestDto extends ExtendedPagedAndSortedResultRequestDto {
  provinceCode?: string;
  districtCode?: string;
  communeCode?: string;
}

export interface PatientUpdateDto extends EntityDto<string> {
  code: string;
  name: string;
  provinceCode: string;
  districtCode: string;
  communeCode: string;
  hospitalId?: string;
}
