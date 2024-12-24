import { mapEnumToOptions } from '@abp/ng.core';

export enum ProvinceLevel {
  Province = 0,
  MunicipalCity = 1,
}

export const provinceLevelOptions = mapEnumToOptions(ProvinceLevel);
