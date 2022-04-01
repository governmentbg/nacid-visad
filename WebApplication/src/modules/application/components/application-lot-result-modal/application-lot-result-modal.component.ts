import { Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { ApplicationLotResultType } from '../../enums/application-lot-result-type.enum';
import { ApplicationLotResultDto } from '../../models/application-lot-result.dto';
import { CreateApplicationLotResultDto } from '../../models/create-application-lot-result.dto';
import { ApplicationResource } from '../../services/application.resource';

@Component({
  selector: 'app-application-lot-result-modal',
  templateUrl: './application-lot-result-modal.component.html'
})
export class ApplicationLotResultModalComponent {
  model = new CreateApplicationLotResultDto();
  resultTypes = ApplicationLotResultType;

  @Input() lotId: number;
  @Input() commitId: number;

  @Input() lotResult: ApplicationLotResultDto;

  @ViewChild('resultForm') resultForm: NgForm;

  public validationTexts: { key: string, value: string }[] = [
    { key: 'required', value: 'Полето е задължително' }
  ];

  constructor(
    public modal: NgbActiveModal,
    private resource: ApplicationResource,
    private loadingIndicator: LoadingIndicatorService
  ) { }

  save(): void {
    if (!this.resultForm.form.valid) {
      return;
    }

    this.model.lotId = this.lotId;
    this.loadingIndicator.show();
    this.resource.addApplicationLotResult(this.lotId, this.model)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((result: ApplicationLotResultDto) => {
        this.lotResult = result;
        this.modal.close(result)
      });
  }
}
