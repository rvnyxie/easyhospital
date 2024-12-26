import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { LocalityPagedAndSortedResultRequestDto } from '@proxy/base';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommuneDto, CommuneService } from '@proxy/communes';

@Component({
  selector: 'app-commune',
  templateUrl: './commune.component.html',
  styleUrl: './commune.component.scss',
  providers: [ListService],
})
export class CommuneComponent implements OnInit {
  commune = { totalCount: 0, items: [] } as PagedResultDto<CommuneDto>;
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
  communeForm: FormGroup; // Reactive form for create/update
  selectedCommune: CommuneDto;

  constructor(public list: ListService,
              private communeService: CommuneService,
              private fb: FormBuilder) {
    this.communeForm = this.fb.group({
      id: [''],
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      englishName: [''],
      level: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    const communeStreamCreator = () => this.communeService.getList(this.query);

    this.list.hookToQuery(communeStreamCreator).subscribe((response) => {
      this.commune = response;
      console.log(this.commune);
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
  }

  get isFormMode(): boolean {
    return this.modalMode === 'create' || this.modalMode === 'update';
  }
}
