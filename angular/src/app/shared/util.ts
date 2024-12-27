/**
 * Map from enum to array of usable object
 * @param enumType
 */
export function getEnumOptions(enumType: any): { value: number; label: string }[] {
  return Object.keys(enumType)
    .filter((key) => !isNaN(Number(enumType[key])))
    .map((key) => ({
      value: enumType[key],
      label: key,
    }))
}
