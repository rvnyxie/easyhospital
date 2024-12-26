import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HospitalDto, HospitalService } from '@proxy/hospitals';

@Component({
  selector: 'app-hospital',
  templateUrl: './hospital.component.html',
  styleUrl: './hospital.component.scss',
  providers: [ListService],
})
export class HospitalComponent implements OnInit {
  hospital = { totalCount: 0, items: [] } as PagedResultDto<HospitalDto>;
  query = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: '',
    search: '',
    pageIndex: 1,
  };

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  hospitalForm: FormGroup; // Reactive form for create/update
  selectedHospital: HospitalDto;

  constructor(public list: ListService,
              private hospitalService: HospitalService,
              private fb: FormBuilder) {
    this.hospitalForm = this.fb.group({
      id: [''],
      name: ['', [Validators.required]],
      address: [''],
      provinceCode: ['', [Validators.required]],
      districtCode: ['', [Validators.required]],
      communeCode: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    const communeStreamCreator = () => this.hospitalService.getList(this.query);

    this.list.hookToQuery(communeStreamCreator).subscribe((response) => {
      this.hospital = response;
      console.log(this.hospital);
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
    this.closeModal();
  }

  handleModalConfirm(): void {
    if (this.modalMode === 'create' || this.modalMode === 'update') {
      if (this.hospitalForm.valid) {
        const formData = this.hospitalForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.hospitalService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.hospitalService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.hospitalService.delete(this.selectedHospital.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', hospital?: HospitalDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create Hospital';
      this.hospitalForm.reset();
      this.selectedHospital = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update Hospital';
      this.hospitalForm.patchValue(hospital); // Populate form with existing data
      this.selectedHospital = hospital;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete Hospital';
      this.deleteMessage = `Are you sure you want to delete the hospital "${hospital?.name}"?`;
      this.selectedHospital = hospital;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedHospital = null;
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
