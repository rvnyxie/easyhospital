import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommuneDto, CommuneLevel, CommuneService } from '@proxy/communes';
import { CommuneLevelText } from '../shared/enum-mapping';
import { getEnumOptions } from '../shared/util';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';
import { DistrictDto, DistrictService } from '@proxy/districts';
import { ProvinceDto, ProvinceService } from '@proxy/provinces';

@Component({
  selector: 'app-commune',
  templateUrl: './commune.component.html',
  styleUrl: './commune.component.scss',
  providers: [ListService],
})
export class CommuneComponent implements OnInit {
  protected readonly CommuneLevelText = CommuneLevelText;

  readonly defaultLocalityQuery: LocalityPagedAndSortedResultRequestDto = {
    skipCount: 0,
    maxResultCount: 100,
    sorting: '',
    search: '',
  }

  commune = { totalCount: 0, items: [] } as PagedResultDto<CommuneDto>;
  query = {
    ...this.defaultLocalityQuery,
    maxResultCount: 10,
    pageIndex: 1,
  };

  communeLevels = getEnumOptions(CommuneLevel);

  provinceQuery = { ...this.defaultLocalityQuery };
  districtQuery = { ...this.defaultLocalityQuery };
  provinces: ProvinceDto[] = [];
  districts: DistrictDto[] = [];

  // Modal
  isModalOpen = false;
  modalMode: 'create' | 'update' | 'delete' = 'create'; // Current mode
  modalTitle = 'Default modal title';
  deleteMessage = '';
  communeForm: FormGroup; // Reactive form for create/update
  selectedCommune: CommuneDto;

  constructor(public list: ListService,
              private provinceService: ProvinceService,
              private districtService: DistrictService,
              private communeService: CommuneService,
              private fb: FormBuilder) {
    this.communeForm = this.fb.group({
      id: [''],
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      englishName: [''],
      level: ['', [Validators.required]],
      provinceCode: ['', [Validators.required]],
      districtCode: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    const communeStreamCreator = () => this.communeService.getList(this.query);

    this.list.hookToQuery(communeStreamCreator).subscribe((response) => {
      this.commune = response;
      console.log(this.commune);
    });

    // Load initial provinces
    this.provinceService.getList(this.provinceQuery).subscribe((response) => {
      this.provinces = response.items;
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
      if (this.communeForm.valid) {
        const formData = this.communeForm.value;
        console.log(formData);
        if (this.modalMode === 'create') {
          this.communeService.create(formData).subscribe((response) => {
            this.isModalOpen = false;
            console.log(response);
            this.list.get();
          });
        } else if (this.modalMode === 'update') {
          this.communeService.update(formData.id, formData).subscribe(() => {
            this.isModalOpen = false;
            this.list.get();
          });
        }
      }
    } else if (this.modalMode === 'delete') {
      this.communeService.delete(this.selectedCommune.id).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  openModal(mode: 'create' | 'update' | 'delete', commune?: CommuneDto): void {
    this.isModalOpen = true;
    this.modalMode = mode;

    if (mode === 'create') {
      this.modalTitle = 'Create Commune';
      this.communeForm.reset();
      this.selectedCommune = null;
    } else if (mode === 'update') {
      this.modalTitle = 'Update Commune';
      this.communeForm.patchValue(commune); // Populate form with existing data
      this.selectedCommune = commune;
    } else if (mode === 'delete') {
      this.modalTitle = 'Delete Commune';
      this.deleteMessage = `Are you sure you want to delete the commune "${commune?.name}"?`;
      this.selectedCommune = commune;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedCommune = null;

    // Reset query
    this.districtQuery = { ...this.defaultLocalityQuery };
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }

  handleProvinceChange(provinceCode: string) {
    if (provinceCode) {
      this.districtQuery.provinceCode = provinceCode;
      this.loadDistricts();
    }
  }

  loadDistricts() {
    this.districtService.getList(this.districtQuery).subscribe((response) => {
      this.districts = response.items;
    })
  }
}
