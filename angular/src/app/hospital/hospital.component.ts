import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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

  // Queries used in page and search
  query = { ...this.defaultHospitalQuery };
  provinceSearchQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  districtSearchQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  communeSearchQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };

  // Queries used in form modal
  provinceUCQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  districtUCQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };
  communeUCQuery: LocalityPagedAndSortedResultRequestDto = { ...this.defaultLocalityQuery };

  // Results used in page and search
  hospitals = { totalCount: 0, items: [] } as PagedResultDto<HospitalDto>;
  provinces = { totalCount : 0, items: [] } as PagedResultDto<ProvinceDto>;
  districts = { totalCount: 0, items: [] } as PagedResultDto<DistrictDto>;
  communes = { totalCount : 0, items: [] } as PagedResultDto<CommuneDto>;

  // Results used in modal
  provinceResultsInModal = { totalCount : 0, items: [] } as PagedResultDto<ProvinceDto>;
  districtResultsInModal = { totalCount: 0, items: [] } as PagedResultDto<DistrictDto>;
  communeResultsInModal = { totalCount : 0, items: [] } as PagedResultDto<CommuneDto>;

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' | 'undefined' = 'undefined'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  hospitalForm: FormGroup; // Reactive form for create/update
  selectedHospital: HospitalDto;

  constructor(public list: ListService,
              private hospitalService: HospitalService,
              private provinceService: ProvinceService,
              private districtService: DistrictService,
              private communeService: CommuneService,
              private fb: FormBuilder,
              private cdr: ChangeDetectorRef) {
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
      this.hospitals = response;
    });

    // Load initial provinces
    this.loadProvinces();
  }

  // Province-related methods

  loadProvinces() {
    // Open modal
    if (this.modalMode !== 'undefined') {
      this.provinceService.getList(this.provinceUCQuery).subscribe((response) => {
        this.provinceResultsInModal = response;

        // Reset dependent queries
        if (this.modalMode === 'create') {
          this.districtUCQuery.provinceCode = null
          this.communeUCQuery.districtCode = null

          this.districtResultsInModal.totalCount = 0;
          this.districtResultsInModal.items = [];

          this.communeResultsInModal.totalCount = 0;
          this.communeResultsInModal.items = [];
        }
      })
    } else { // Not loading for opening modal
      this.provinceService.getList(this.provinceSearchQuery).subscribe((response) => {
        this.provinces = response;

        // Reset dependent queries
        this.query.districtCode = null;
        this.query.communeCode = null;

        this.districts.totalCount = 0;
        this.districts.items = [];

        this.communes.totalCount = 0;
        this.communes.items = [];
      });
    }

  }

  onProvinceSearch(search: string) {
    this.provinceSearchQuery.search = search;
    this.loadProvinces();
  }

  onProvinceChange(provinceCode: string) {
    if (provinceCode) {
      // Select province will need to update the queries
      this.query.provinceCode = provinceCode;
      this.districtSearchQuery.provinceCode = provinceCode;

      this.loadDistricts();
    }
  }

  onProvinceUCFormChange(provinceCode: string) {
    if (provinceCode) {
      this.districtUCQuery.provinceCode = provinceCode;
      this.loadDistricts();
    }
  }

  // District-related methods

  loadDistricts() {
    // Load districts when opening modal
    if (this.modalMode !== 'undefined') {
      this.districtService.getList(this.districtUCQuery).subscribe((response) => {
        this.districtResultsInModal = response;
        console.log("District results in modal:", this.districtResultsInModal);

        // Reset dependent query
        this.communeUCQuery.communeCode = null;

        this.communeResultsInModal.totalCount = 0;
        this.communeResultsInModal.items = [];
      })
    } else { // Not when opening modal
      if (!this.districtSearchQuery.provinceCode) return;

      this.districtService.getList(this.districtSearchQuery).subscribe((response) => {
        this.districts = response;
        console.log('District results for search:', this.districts);

        // Reset dependent query
        this.query.communeCode = null;

        this.communes.totalCount = 0;
        this.communes.items = [];
      });
    }

  }

  onDistrictSearch(search: string) {
    this.districtSearchQuery.search = search;
    this.loadDistricts();
  }

  onDistrictChange(districtCode: string) {
    if (districtCode) {
      // Select district will need to update the queries
      this.query.districtCode = districtCode;
      this.communeSearchQuery.districtCode = districtCode;

      this.loadCommunes();
    }
  }

  onDistrictUCFormChange(districtCode: string) {
    if (districtCode) {
      this.communeUCQuery.districtCode = districtCode;
      this.loadCommunes();
    }
  }

  // Commune-related methods

  loadCommunes() {
    // Load communes when opening modal
    if (this.modalMode !== 'undefined') {
      this.communeService.getList(this.communeUCQuery).subscribe((response) => {
        this.communeResultsInModal = response;
        console.log("Commune results in modal:", this.communeResultsInModal);
      })
    } else { // Load when not in modal
      if (!this.communeSearchQuery.districtCode) return;

      this.communeService.getList(this.communeSearchQuery).subscribe((response) => {
        this.communes = response;
        console.log('Commune results for search:', this.communes);
      });
    }
  }

  onCommuneSearch(search: string) {
    this.communeSearchQuery.search = search;
    this.loadCommunes();
  }

  onCommuneChange(communeCode: string) {
    if (communeCode) {
      this.query.communeCode = communeCode
    }
  }

  onCommuneUCFormChange(communeCode: string) {
    if (communeCode) {
      this.communeUCQuery.communeCode = communeCode;
    }
  }

  // Handle page index change
  onPageIndexChange(pageIndex: number) {
    this.query.pageIndex = pageIndex;
    this.query.skipCount = (pageIndex - 1) * this.query.maxResultCount;

    this.list.get();
  }

  // Search form methods
  resetSearch() {
    this.query = { ...this.defaultHospitalQuery };
    this.provinceSearchQuery = { ...this.defaultLocalityQuery };
    this.districtSearchQuery = { ...this.defaultLocalityQuery };
    this.communeSearchQuery = { ...this.defaultLocalityQuery };

    this.loadProvinces();
    this.list.get();
  }

  performSearch() {
    console.log("Search query:", this.query);
    this.list.get();
  }

  // Handle page size change
  onPageSizeChange(pageSize: number) {
    this.query.maxResultCount = pageSize;

    this.list.get();
  }

  // Handle modal cancel
  handleModalCancel() {
    this.closeModal();
  }

  // Handle modal confirm
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

  // Handle open modal
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

      // Load dependent dropdowns
      this.loadProvinces();
      this.districtUCQuery.provinceCode = hospital.provinceCode;
      this.loadDistricts();
      this.communeUCQuery.districtCode = hospital.districtCode;
      this.loadCommunes();

    } else if (mode === 'delete') {
      this.modalTitle = 'Delete Hospital';
      this.deleteMessage = `Are you sure you want to delete the hospital "${hospital?.name}"?`;
      this.selectedHospital = hospital;
    }

    // Detect changes manually to avoid ExpressionChangedAfterItHasBeenCheckedError
    this.cdr.detectChanges();
  }

  // Handle close modal
  closeModal() {
    this.isModalOpen = false;
    this.modalMode = 'undefined';
    this.selectedHospital = null;

    // Reset UC queries
    this.provinceUCQuery = { ...this.defaultLocalityQuery };
    this.districtUCQuery = { ...this.defaultLocalityQuery };
    this.communeUCQuery = { ...this.defaultLocalityQuery };
  }

  // Get form mode
  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
