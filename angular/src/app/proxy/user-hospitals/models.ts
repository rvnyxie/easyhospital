import type { AuditedEntityDto } from '@abp/ng.core';

export interface UserHospitalCreateDto {
  userId: string;
  hospitalId: string;
}

export interface UserHospitalDto extends AuditedEntityDto<string> {
  userId?: string;
  userName?: string;
  hospitalId?: string;
  hospitalName?: string;
}

export interface UserHospitalUpdateDto {
  id: string;
  userId: string;
  hospitalId: string;
}
