import { ChangeDetectorRef, Directive } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { BaseNomenclatureFilterDto } from './models/base-nomenclature-filter.dto';
import { Nomenclature } from './models/nomenclature.model';
import { NomenclatureResource } from './services/nomenclature.resource';

@Directive({})
export abstract class BaseNomenclatureComponent<T extends Nomenclature> {
  model: T[] = [];
  canLoadMore: boolean;
  currentPrefix: string;

  filter = new BaseNomenclatureFilterDto();

  private type: (new () => T);

  constructor(
    protected resource: NomenclatureResource<T>,
    private cd: ChangeDetectorRef,
    private modal: NgbModal,
    private translateService: TranslateService,
    private toastrService: ToastrService
  ) { }

  protected init(type: (new () => T), prefix: string): void {
    if (!this.loadMore) {
      this.filter.offset = 0;
    }

    this.type = type;
    this.currentPrefix = prefix;
    this.resource.setSuffix(prefix);

    this.filter.includeInactive = true;
    this.resource.getFiltered(this.filter)
      .subscribe((model: T[]) => {
        if (!this.filter.offset) {
          this.model = model;
        } else {
          this.model = [...this.model, ...model];
        }

        this.canLoadMore = model.length === this.filter.limit;
        this.filter.offset = this.model.length;
        this.cd.markForCheck();
      });
  }

  loadMore(): void {
    this.filter.offset = this.model.length;
    this.init(this.type, this.currentPrefix);
  }

  add(): void {
    if (this.model.filter(e => e.isEditMode).length) {
      return;
    }

    const newEntity = new this.type();
    newEntity.isActive = true;
    newEntity.isEditMode = true;

    this.model.unshift(newEntity);
  }

  edit(item: T): void {
    item.originalObject = Object.assign({}, item);
    item.isEditMode = true;
  }

  cancel(item: T, index: number): void {
    if (!item.id) {
      this.model.splice(index, 1);
    } else {
      Object.keys(item).forEach(key => {
        if (key !== 'originalObject') {
          item[key] = item.originalObject[key];
        }
      });

      item.isEditMode = false;
      item.originalObject = null;
    }
  }

  save(item: T, index: number): void {
    item.originalObject = null;

    if (!item.id) {
      this.resource.add(item)
        .subscribe((result: T) => {
          this.model[index] = result;
          this.cd.markForCheck();
        });
    } else {
      this.resource.update(item.id, item)
        .subscribe((result: T) => {
          this.model[index] = result;
          this.cd.markForCheck();
        });
    }
  }

  delete(id: number, index: number): void {
    const confirmationModal = this.modal.open(ActionConfirmationModalComponent, { backdrop: 'static' });
    const confirmationMessage = "Сигурни ли сте, че искате да изтриете стойността?";
    confirmationModal.componentInstance.confirmationMessage = confirmationMessage;
    confirmationModal.result
      .then((result: boolean) => {
        if (result) {
          this.resource.delete(id)
            .subscribe(() => {
              this.model.splice(index, 1);
              this.cd.markForCheck();
            },
              (err) => handleDomainError(
                err,
                [{ code: 'Nomenclature_CannotDelete', text: this.translateService.instant('nomenclature.cannotDelete') }],
                this.toastrService
              )
            );
        }
      });
  }
}
