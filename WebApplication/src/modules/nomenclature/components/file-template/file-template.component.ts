import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionConfirmationModalComponent } from 'src/infrastructure/components/action-confirmation-modal/action-confirmation-modal.component';
import { AttachedFile } from 'src/infrastructure/models/attached-file.model';
import { FileTemplate } from '../../models/file-template.model';
import { FileTemplateResource } from '../../services/file-template.resource';

@Component({
  selector: 'app-file-template',
  templateUrl: './file-template.component.html',
})
export class FileTemplateComponent implements OnInit {
  model: FileTemplate[] = [];

  constructor(
    protected resource: FileTemplateResource,
    private modal: NgbModal,
  ) {
  }

  ngOnInit(): void {
    this.resource.getall()
      .subscribe((model: FileTemplate[]) => {
        this.model = model;
      });
  }

  add(): void {
    if (this.model.filter(e => e.isEditMode).length) {
      return;
    }

    const newEntity = new FileTemplate();
    newEntity.isActive = true;
    newEntity.isEditMode = true;

    this.model.push(newEntity);
  }

  edit(item: FileTemplate): void {
    item.originalObject = Object.assign({}, item);
    item.isEditMode = true;
  }

  cancel(item: FileTemplate, index: number): void {
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

  save(item: FileTemplate, index: number): void {
    item.originalObject = null;

    if (!item.id) {
      this.resource.add(item)
        .subscribe((result: FileTemplate) => {
          this.model[index] = result;
        });
    } else {
      this.resource.update(item)
        .subscribe((result: FileTemplate) => {
          this.model[index] = result;
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
            });
        }
      });
  }

  attachedFileChanged(index: number, file: AttachedFile | null): void {
    Object.keys(new AttachedFile())
      .forEach((key: string) => {
        this.model[index][key] = file ? file[key] : null;
      });
  }
}
