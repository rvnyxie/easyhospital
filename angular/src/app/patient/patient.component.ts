import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PatientDto, PatientService } from '@proxy/patients';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';
import { DistrictDto, DistrictService } from '@proxy/districts';
import { CommuneDto, CommuneService } from '@proxy/communes';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.scss',
  providers: [ListService],
})
export class PatientComponent implements OnInit {
  // Default queries
  readonly defaultLocalityQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: null,
    provinceCode: null,
    districtCode: null,
    communeCode: null,
  }
  readonly defaultPatientQuery = {
    ...this.defaultLocalityQuery,
    pageIndex: 1,
    maxResultCount: 10
  };

  query = { ...this.defaultPatientQuery };
  provinceQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  districtQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  communeQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };

  patient = { totalCount: 0, items: [] } as PagedResultDto<PatientDto>;
  provinces = { totalCount : 0, items: [] } as PagedResultDto<ProvinceDto>;
  districts = { totalCount: 0, items: [] } as PagedResultDto<DistrictDto>;
  communes = { totalCount : 0, items: [] } as PagedResultDto<CommuneDto>;

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  patientForm: FormGroup; // Reactive form for create/update
  selectedPatient: PatientDto;

  constructor(public list: ListService,
              private patientService: PatientService,
              private provinceService: ProvinceService,
              private districtService: DistrictService,
              private communeService: CommuneService,
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

    // Load initial provinces
    this.loadProvinces();
  }

  // Province-related methods

  loadProvinces() {
    this.provinceService.getList(this.provinceQuery).subscribe((response) => {
      this.provinces = response;
      console.log('Provinces:', this.provinces);

      // Reset dependent queries
      this.query.districtCode = null;
      this.query.communeCode = null;

      this.districts.totalCount = 0;
      this.districts.items = [];

      this.communes.totalCount = 0;
      this.communes.items = [];
    });
  }

  onProvinceSearch(search: string) {
    this.provinceQuery.search = search;
    this.loadProvinces();
  }

  onProvinceChange(provinceCode: string) {
    if (provinceCode) {
      // Select province will need to update the queries
      this.query.provinceCode = provinceCode;
      this.districtQuery.provinceCode = provinceCode;

      this.loadDistricts();
    }
  }

  // District-related methods

  loadDistricts() {
    if (!this.districtQuery.provinceCode) return;

    this.districtService.getList(this.districtQuery).subscribe((response) => {
      this.districts = response;
      console.log('Districts:', this.districts);

      // Reset dependent query
      this.query.communeCode = null;

      this.communes.totalCount = 0;
      this.communes.items = [];
    });
  }

  onDistrictSearch(search: string) {
    this.districtQuery.search = search;
    this.loadDistricts();
  }

  onDistrictChange(districtCode: string) {
    if (districtCode) {
      // Select district will need to update the queries
      this.query.districtCode = districtCode;
      this.communeQuery.districtCode = districtCode;

      this.loadCommunes();
    }
  }

  // Commune-related methods

  loadCommunes() {
    if (!this.communeQuery.districtCode) return;

    this.communeService.getList(this.communeQuery).subscribe((response) => {
      this.communes = response;
      console.log('Communes:', this.communes);
    });
  }

  onCommuneSearch(search: string) {
    this.communeQuery.search = search;
    this.loadCommunes();
  }

  onCommuneChange(communeCode: string) {
    if (communeCode) {
      this.query.communeCode = communeCode
    }
  }

  // Search form methods
  resetSearch() {
    this.query = { ...this.defaultPatientQuery };
    this.provinceQuery = { ...this.defaultLocalityQuery };
    this.districtQuery = { ...this.defaultLocalityQuery };
    this.communeQuery = { ...this.defaultLocalityQuery };

    this.loadProvinces();
    this.list.get();
  }

  performSearch() {
    console.log("Search query:", this.query);
    this.list.get();
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
