import type { DistrictLevel } from './district-level.enum';
import type { AuditedEntityDto } from '@abp/ng.core';
import type { ProvinceLevel } from '../provinces/province-level.enum';

export interface DistrictCreateDto {
  code: string;
  name: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: DistrictLevel;
  provinceCode?: string;
}

export interface DistrictDto extends AuditedEntityDto<string> {
  id?: string;
  code?: string;
  name?: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: DistrictLevel;
  provinceCode?: string;
}

export interface DistrictUpdateDto {
  id?: string;
  code: string;
  name: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: ProvinceLevel;
  provinceCode?: string;
}
