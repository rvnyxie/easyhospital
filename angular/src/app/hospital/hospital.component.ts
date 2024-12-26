import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HospitalDto, HospitalService } from '@proxy/hospitals';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';
import { DistrictDto, DistrictService } from '@proxy/districts';
import { CommuneDto, CommuneService } from '@proxy/communes';
import { ExtendedPagedAndSortedResultRequestDto, LocalityPagedAndSortedResultRequestDto } from '@proxy/base';

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
    search: null,
    pageIndex: 1,
    provinceCode: null,
    districtCode: null,
    communeCode: null,
  };

  provinceQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: null,
  }

  districtQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: null,
  }

  communeQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: null,
  }

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
    // Define stream creators
    const hospitalStreamCreator = () => this.hospitalService.getList(this.query);
    const provinceStreamCreator = () => this.provinceService.getList(this.provinceQuery);
    const districtStreamCreator = () => this.districtService.getList(this.districtQuery);
    const communeStreamCreator = () => this.communeService.getList(this.communeQuery);

    // Hook stream creators to list service
    this.list.hookToQuery(hospitalStreamCreator).subscribe((response) => {
      this.hospital = response;
      console.log(this.hospital);
    });

    this.list.hookToQuery(provinceStreamCreator).subscribe((response) => {
      this.provinces = response;
      console.log(this.provinces);
    })

    this.list.hookToQuery(districtStreamCreator).subscribe((response) => {
      this.districts = response;
      console.log(this.districts);
    })

    this.list.hookToQuery(communeStreamCreator).subscribe((response) => {
      this.communes = response;
      console.log(this.communes);
    })

    // Load provinces when on init
    this.loadProvinces();
  }

  loadProvinces(search?: string) {
    this.provinceQuery.search = search;
    // TODO: load provinces
  }

  onProvinceSearch(search: string) {
    this.loadProvinces(search);
  }

  onProvinceChange(provinceCode: string) {
    this.query.districtCode = null;
    this.query.communeCode = null;

    this.districts.totalCount = 0;
    this.districts.items = [];

    this.communes.totalCount = 0;
    this.communes.items = [];

    if (provinceCode) {
      this.loadDistricts(provinceCode);
    }
  }

  // District-related methods

  loadDistricts(provinceCode: string, search?: string) {
  }

  onDistrictSearch(search: string) {
    if (this.query.provinceCode) {
      this.loadDistricts(this.query.provinceCode, search);
    }
  }

  onDistrictChange(districtCode: string) {
    this.query.communeCode = null;

    this.communes.totalCount = 0;
    this.communes.items = [];

    if (districtCode) {
      this.loadCommunes(districtCode);
    }
  }

  // Commune-related methods

  loadCommunes(districtCode: string, search?: string) {
  }

  onCommuneSearch(search: string) {
    if (this.query.districtCode) {
      this.loadCommunes(this.query.districtCode, search);
    }
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
    this.query.search = null;
    this.query.provinceCode = null;
    this.query.districtCode = null;
    this.query.communeCode = null;

    // TODO:
  }

  performSearch() {
    console.log(this.query);
    // TODO:
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
