import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DistrictDto, DistrictService } from '@proxy/districts';

@Component({
  selector: 'app-district',
  templateUrl: './district.component.html',
  styleUrl: './district.component.scss',
  providers: [ListService],
})
export class DistrictComponent implements OnInit {
  district = { totalCount: 0, items: [] } as PagedResultDto<DistrictDto>;
  query = {
    skipCount: 0,
    maxResultCount: 20,
    sorting: '',
    search: '',
    pageIndex: 1,
  };

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  districtForm: FormGroup; // Reactive form for create/update
  selectedDistrict: DistrictDto;

  constructor(public list: ListService,
              private districtService: DistrictService,
              private fb: FormBuilder) {
    this.districtForm = this.fb.group({
      id: [''],
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      englishName: [''],
      level: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    const districtStreamCreator = () => this.districtService.getList(this.query);

    this.list.hookToQuery(districtStreamCreator).subscribe((response) => {
      this.district = response;
      console.log(this.district);
    });
  }

  onPageIndexChange(pageIndex: number) {
    this.query.pageIndex = pageIndex;
    this.query.skipCount = (pageIndex - 1) * this.query.maxResultCount;

    this.list.get();
  }

  onPageSizeChange(pageSize: number) {
    this.query.maxResultCount = pageSize;

    this.list.get();
  }

  handleModalCancel() {
    console.log("Cancel");
    this.closeModal();
  }

  handleModalConfirm(): void {
    if (this.modalMode === 'create' || this.modalMode === 'update') {
      if (this.districtForm.valid) {
        const formData = this.districtForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.districtService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.districtService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.districtService.delete(this.selectedDistrict.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', district?: DistrictDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create District';
      this.districtForm.reset();
      this.selectedDistrict = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update District';
      this.districtForm.patchValue(district); // Populate form with existing data
      this.selectedDistrict = district;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete District';
      this.deleteMessage = `Are you sure you want to delete the district "${district?.name}"?`;
      this.selectedDistrict = district;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedDistrict = null;
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
