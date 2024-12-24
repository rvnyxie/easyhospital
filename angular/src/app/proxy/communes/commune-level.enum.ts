import { mapEnumToOptions } from '@abp/ng.core';

export enum CommuneLevel {
  Commune = 0,
  Ward = 1,
  Town = 2,
}

export const communeLevelOptions = mapEnumToOptions(CommuneLevel);
