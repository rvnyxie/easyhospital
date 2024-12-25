import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';

@Component({
  selector: 'app-province',
  templateUrl: './province.component.html',
  styleUrl: './province.component.scss',
  providers: [ListService],
})
export class ProvinceComponent implements OnInit {
  province = { totalCount: 0, items: [] } as PagedResultDto<ProvinceDto>;
  query = {
    skipCount: 0,
    maxResultCount: 0,
    sorting: '',
    search: '',
  } as LocalityPagedAndSortedResultRequestDto;

  // Modal
  isModalOpen = false;

  constructor(public list: ListService,
              private provinceService: ProvinceService) {
  }

  ngOnInit() {
    const provinceStreamCreator = (query: LocalityPagedAndSortedResultRequestDto) => this.provinceService.getList(query);

    this.list.hookToQuery(provinceStreamCreator).subscribe((response) => {
      this.province = response;
      console.log(this.province);
    });
  }

  onQueryParamsChange(params: { pageIndex: number; pageSize: number; sort: { key: string; value: string }[] }) {
    this.list.get();
  }

  editProvince(province: ProvinceDto) {
    console.log("Edit province", province);
    this.openModal();

  }

  deleteProvince(province: ProvinceDto) {
    console.log("Deleting province", province);
    this.openModal();

  }

  handleModalCancel() {
    console.log("Cancel");
    this.closeModal();
  }

  handleModelConfirm() {
    console.log("Confirm");
    this.openModal();
  }

  handleAdding() {
    this.openModal();
  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }
}
