import type { CommuneLevel } from './commune-level.enum';

export interface CommuneCreateDto {
  code: string;
  name: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: CommuneLevel;
  districtCode?: string;
}

export interface CommuneDto {
  id?: string;
  code?: string;
  name?: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: CommuneLevel;
  districtCode?: string;
}

export interface CommuneUpdateDto {
  id?: string;
  code: string;
  name: string;
  englishName?: string;
  decisionDate?: string;
  effectiveDate?: string;
  population: number;
  area: number;
  description?: string;
  level: CommuneLevel;
  districtCode?: string;
}
