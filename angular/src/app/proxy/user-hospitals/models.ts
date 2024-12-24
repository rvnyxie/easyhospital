import type { AuditedEntityDto } from '@abp/ng.core';

export interface UserHospitalCreateDto {
  userId: string;
  hospitalId: string;
}

export interface UserHospitalDto extends AuditedEntityDto<string> {
  userId?: string;
  hospitalId?: string;
}

export interface UserHospitalUpdateDto {
  id: string;
  userId: string;
  hospitalId: string;
}
