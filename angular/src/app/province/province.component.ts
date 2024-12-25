import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
    maxResultCount: 10,
    sorting: '',
    search: '',
  } as LocalityPagedAndSortedResultRequestDto;

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  provinceForm: FormGroup; // Reactive form for create/update
  selectedProvince: ProvinceDto;

  constructor(public list: ListService,
              private provinceService: ProvinceService,
              private fb: FormBuilder) {
    this.provinceForm = this.fb.group({
      id: [''],
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      englishName: [''],
      level: ['', [Validators.required]]
    });
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

  handleModalCancel() {
    console.log("Cancel");
    this.closeModal();
  }

  handleModalConfirm(): void {
    if (this.modalMode === 'create' || this.modalMode === 'update') {
      if (this.provinceForm.valid) {
        const formData = this.provinceForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.provinceService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.provinceService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.provinceService.delete(this.selectedProvince.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', province?: ProvinceDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create Province';
      this.provinceForm.reset();
      this.selectedProvince = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update Province';
      this.provinceForm.patchValue(province); // Populate form with existing data
      this.selectedProvince = province;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete Province';
      this.deleteMessage = `Are you sure you want to delete the province "${province?.name}"?`;
      this.selectedProvince = province;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedProvince = null;
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
