import type { ProvinceLevel } from './province-level.enum';
import type { AuditedEntityDto } from '@abp/ng.core';

export interface ProvinceCreateDto {
  code: string;
  name: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: ProvinceLevel;
}

export interface ProvinceDto extends AuditedEntityDto<string> {
  id?: string;
  code?: string;
  name?: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: ProvinceLevel;
}

export interface ProvinceUpdateDto {
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
}
