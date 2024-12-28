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
  // Default locality query
  readonly defaultLocalityQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: null,
    provinceCode: null,
    districtCode: null,
    communeCode: null,
  }

  // Default hospital query
  readonly defaultHospitalQuery = {
    ...this.defaultLocalityQuery,
    pageIndex: 1,
    maxResultCount: 10
  };

  // Default page result
  readonly defaultPageResultDto = { totalCount: 0, items: [] };

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
  hospitals = { ...this.defaultPageResultDto } as PagedResultDto<HospitalDto>;
  provinces = { ...this.defaultPageResultDto } as PagedResultDto<ProvinceDto>;
  districts = { ...this.defaultPageResultDto } as PagedResultDto<DistrictDto>;
  communes = { ...this.defaultPageResultDto } as PagedResultDto<CommuneDto>;

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
    const hospitalStreamCreator = () => this.hospitalService.getList(this.query);

    // Hook hospital stream creators to list service
    this.list.hookToQuery(hospitalStreamCreator).subscribe((response) => {
      this.hospitals = response;
    });

    // Load initial provinces
    this.loadProvinces();
  }

  /**
   * Load provinces, set response based on where it is called
   */
  loadProvinces() {
    const isInModal = this.modalMode !== 'undefined';

    // Load when opening modal
    if (isInModal) {
      this.provinceService.getList(this.provinceUCQuery).subscribe((response) => {
        this.provinceResultsInModal = response;
      })
    } else { // Load when not in modal
      this.provinceService.getList(this.provinceSearchQuery).subscribe((response) => {
        this.provinces = response;
      });
    }

    this.resetDependentAfterSelectProvince(isInModal);
  }

  /**
   * Reset dependents after selecting province
   * @param isInModal true if perform in modal
   */
  resetDependentAfterSelectProvince(isInModal: boolean) {
    this.resetDistrictAndCommuneQuery(isInModal);
    this.resetDistrictAndCommuneResults(isInModal);
  }

  /**
   * Reset district and commune queries
   * @param isInModal true if perform in modal
   */
  resetDistrictAndCommuneQuery(isInModal: boolean) {
    this.resetDistrictQuery(isInModal);
    this.resetCommuneQuery(isInModal);
  }

  /**
   * Reset district query
   * @param isInModal true if perform in modal
   */
  resetDistrictQuery(isInModal: boolean) {
    if (isInModal) {
      this.districtUCQuery = { ...this.defaultLocalityQuery };
    } else {
      this.districtSearchQuery = { ...this.defaultLocalityQuery };
    }
  }

  /**
   * Reset commune query
   * @param isInModal true if perform in modal
   */
  resetCommuneQuery(isInModal: boolean) {
    if (isInModal) {
      this.communeUCQuery = { ...this.defaultLocalityQuery };
    } else {
      this.communeSearchQuery = { ...this.defaultLocalityQuery };
    }
  }

  /**
   * Reset district and commune results
   * @param isInModal true if perform in modal
   */
  resetDistrictAndCommuneResults(isInModal: boolean) {
    this.resetDistrictResults(isInModal);
    this.resetCommuneResults(isInModal);
  }

  /**
   * Reset district results
   * @param isInModal true if perform in modal
   */
  resetDistrictResults(isInModal: boolean) {
    if (isInModal) {
      this.districtResultsInModal = { ...this.defaultPageResultDto };
    } else {
      this.districts = { ...this.defaultPageResultDto };
    }
  }

  /**
   * Reset commune results
   * @param isInModal true if perform in modal
   */
  resetCommuneResults(isInModal: boolean) {
    if (isInModal) {
      this.communeResultsInModal = { ...this.defaultPageResultDto };
    } else {
      this.communes = { ...this.defaultPageResultDto };
    }
  }

  /**
   * Handle on province search change
   * @param search Search term
   */
  onProvinceSearch(search: string) {
    this.provinceSearchQuery.search = search;
    this.loadProvinces();
  }

  /**
   * Handle when province change
   * @param provinceCode Code of changed province
   * @param isInModal True if is in modal
   */
  onProvinceChange(provinceCode: string, isInModal?: boolean) {
    if (!provinceCode) return;

    // Select province will need to update the queries
    if (isInModal) {
      this.districtUCQuery.provinceCode = provinceCode;
    } else {
      this.query.provinceCode = provinceCode;
      this.districtSearchQuery.provinceCode = provinceCode;
    }

    this.loadDistricts();
  }

  /**
   * Load districts, set response based on where it is called
   */
  loadDistricts() {
    const isInModal = this.modalMode !== 'undefined';

    // Load when opening modal
    if (isInModal) {
      this.districtService.getList(this.districtUCQuery).subscribe((response) => {
        this.districtResultsInModal = response;
      })
    } else { // Load when not opening modal
      if (!this.districtSearchQuery.provinceCode) return;

      this.districtService.getList(this.districtSearchQuery).subscribe((response) => {
        this.districts = response;
      });
    }

    this.resetCommuneDependentsAfterSelectingDistrict(isInModal);
  }

  /**
   * Reset dependents after selecting district
   * @param isInModal True if is in modal
   */
  resetCommuneDependentsAfterSelectingDistrict(isInModal: boolean) {
    this.resetCommuneQuery(isInModal);
    this.resetCommuneResults(isInModal);
  }

  /**
   * Handle on district search change
   * @param search Search term
   */
  onDistrictSearch(search: string) {
    this.districtSearchQuery.search = search;
    this.loadDistricts();
  }

  /**
   * Handler on district change
   * @param districtCode Code of changed district
   * @param isInModal True if is in modal
   */
  onDistrictChange(districtCode: string, isInModal?: boolean) {
    if (!districtCode) return;

    if (isInModal) {
      this.communeUCQuery.districtCode = districtCode;
    } else {
      this.query.districtCode = districtCode;
      this.communeSearchQuery.districtCode = districtCode;
    }

    this.loadCommunes();
  }

  /**
   * Load communes, set response based on where it is called
   */
  loadCommunes() {
    const isInModal = this.modalMode !== 'undefined';

    // Load when opening modal
    if (isInModal) {
      this.communeService.getList(this.communeUCQuery).subscribe((response) => {
        this.communeResultsInModal = response;
      })
    } else { // Load when not in modal
      if (!this.communeSearchQuery.districtCode) return;

      this.communeService.getList(this.communeSearchQuery).subscribe((response) => {
        this.communes = response;
      });
    }
  }

  /**
   * Handle on commune search change
   * @param search Commune search term
   */
  onCommuneSearch(search: string) {
    this.communeSearchQuery.search = search;
    this.loadCommunes();
  }

  /**
   * Handle commune change
   * @param communeCode Code of changed commune
   * @param isInModal True if is in modal
   */
  onCommuneChange(communeCode: string, isInModal?: boolean) {
    if (!communeCode) return;

    if (isInModal) {
      this.communeUCQuery.communeCode = communeCode;
    } else {
      this.query.communeCode = communeCode;
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
