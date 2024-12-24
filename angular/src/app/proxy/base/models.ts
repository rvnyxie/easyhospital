import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExtendedPagedAndSortedResultRequestDto extends PagedAndSortedResultRequestDto {
  search?: string;
}

export interface LocalityPagedAndSortedResultRequestDto extends ExtendedPagedAndSortedResultRequestDto {
}
