import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PatientDto, PatientService } from '@proxy/patients';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.scss',
  providers: [ListService],
})
export class PatientComponent implements OnInit {
  patient = { totalCount: 0, items: [] } as PagedResultDto<PatientDto>;
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
  patientForm: FormGroup; // Reactive form for create/update
  selectedPatient: PatientDto;

  constructor(public list: ListService,
              private patientService: PatientService,
              private fb: FormBuilder) {
    this.patientForm = this.fb.group({
      id: [''],
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      provinceCode: ['', [Validators.required]],
      districtCode: ['', [Validators.required]],
      communeCode: ['', [Validators.required]],
      hospitalId: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    const patientStreamCreator = () => this.patientService.getList(this.query);

    this.list.hookToQuery(patientStreamCreator).subscribe((response) => {
      this.patient = response;
      console.log(this.patient);
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
      if (this.patientForm.valid) {
        const formData = this.patientForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.patientService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.patientService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.patientService.delete(this.selectedPatient.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', patient?: PatientDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create Patient';
      this.patientForm.reset();
      this.selectedPatient = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update Patient';
      this.patientForm.patchValue(patient); // Populate form with existing data
      this.selectedPatient = patient;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete Patient';
      this.deleteMessage = `Are you sure you want to delete the patient "${patient?.name}"?`;
      this.selectedPatient = patient;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedPatient = null;
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
