import { ProvinceLevel } from '@proxy/provinces';
import { DistrictLevel } from '@proxy/districts';
import { CommuneLevel } from '@proxy/communes';

/**
 * Mapping from entity enums to corresponding text
 */

export const ProvinceLevelText = {
  [ProvinceLevel.Province]: "Province",
  [ProvinceLevel.MunicipalCity]: "Municipal City",
}

export const DistrictLevelText = {
  [DistrictLevel.District]: "District",
  [DistrictLevel.City]: "City",
  [DistrictLevel.County]: "County",
  [DistrictLevel.Town]: "Town",
}

export const CommuneLevelText = {
  [CommuneLevel.Commune]: "Commune",
  [CommuneLevel.Ward]: "Ward",
  [CommuneLevel.Town]: "Town",
}
