import { mapEnumToOptions } from '@abp/ng.core';

export enum DistrictLevel {
  District = 0,
  County = 1,
  City = 2,
  Town = 3,
}

export const districtLevelOptions = mapEnumToOptions(DistrictLevel);
