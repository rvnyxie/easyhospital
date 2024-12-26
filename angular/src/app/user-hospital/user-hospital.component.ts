import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserHospitalDto, UserHospitalService } from '@proxy/user-hospitals';

@Component({
  selector: 'app-user-hospital',
  templateUrl: './user-hospital.component.html',
  styleUrl: './user-hospital.component.scss',
  providers: [ListService],
})
export class UserHospitalComponent implements OnInit {
  userHospital = { totalCount: 0, items: [] } as PagedResultDto<UserHospitalDto>;
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
  userHospitalForm: FormGroup; // Reactive form for create/update
  selectedUserHospital: UserHospitalDto;

  constructor(public list: ListService,
              private userHospitalService: UserHospitalService,
              private fb: FormBuilder) {
    this.userHospitalForm = this.fb.group({
      id: [''],
      userId: ['', [Validators.required]],
      hospitalId: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    const userHospitalStreamCreator = () => this.userHospitalService.getList(this.query);

    this.list.hookToQuery(userHospitalStreamCreator).subscribe((response) => {
      this.userHospital = response;
      console.log(this.userHospital);
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
      if (this.userHospitalForm.valid) {
        const formData = this.userHospitalForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.userHospitalService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.userHospitalService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.userHospitalService.delete(this.selectedUserHospital.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', userHospital?: UserHospitalDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create User-Hospital';
      this.userHospitalForm.reset();
      this.selectedUserHospital = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update User-Hospital';
      this.userHospitalForm.patchValue(userHospital); // Populate form with existing data
      this.selectedUserHospital = userHospital;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete User-Hospital';
      this.deleteMessage = `Are you sure you want to delete the user-hospital "${userHospital?.id}"?`;
      this.selectedUserHospital = userHospital;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedUserHospital = null;
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
