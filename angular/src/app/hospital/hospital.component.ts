import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HospitalDto, HospitalService } from '@proxy/hospitals';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';
import { DistrictDto, DistrictService } from '@proxy/districts';
import { CommuneDto, CommuneService } from '@proxy/communes';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';

@Component({
  selector: 'app-hospital',
  templateUrl: './hospital.component.html',
  styleUrl: './hospital.component.scss',
  providers: [ListService],
})
export class HospitalComponent implements OnInit {
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
  readonly defaultHospitalQuery = {
    ...this.defaultLocalityQuery,
    pageIndex: 1,
    maxResultCount: 10
  };

  // Queries
  query = { ...this.defaultHospitalQuery };
  provinceQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  districtQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  communeQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };

  // Result variables
  hospital = { totalCount: 0, items: [] } as PagedResultDto<HospitalDto>;
  provinces = { totalCount : 0, items: [] } as PagedResultDto<ProvinceDto>;
  districts = { totalCount: 0, items: [] } as PagedResultDto<DistrictDto>;
  communes = { totalCount : 0, items: [] } as PagedResultDto<CommuneDto>;

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  hospitalForm: FormGroup; // Reactive form for create/update
  selectedHospital: HospitalDto;

  constructor(public list: ListService,
              private hospitalService: HospitalService,
              private provinceService: ProvinceService,
              private districtService: DistrictService,
              private communeService: CommuneService,
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
    // Define stream creator
    const hospitalStreamCreator = () => this.hospitalService.getList(this.query);

    // Hook hospital stream creators to list service
    this.list.hookToQuery(hospitalStreamCreator).subscribe((response) => {
      this.hospital = response;
      console.log(this.hospital);
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

  onPageIndexChange(pageIndex: number) {
    this.query.pageIndex = pageIndex;
    this.query.skipCount = (pageIndex - 1) * this.query.maxResultCount;

    this.list.get();
  }

  // Search form methods
  resetSearch() {
    this.query = { ...this.defaultHospitalQuery };
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
