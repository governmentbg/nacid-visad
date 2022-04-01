import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ApplicationLotResultTypeEnumLocalization } from 'src/modules/enum-localization.const';
import { ApplicationLotResultType } from '../../enums/application-lot-result-type.enum';
import { ApplicationLotResultDto } from '../../models/application-lot-result.dto';

@Component({
  selector: 'app-application-lot-result-info',
  templateUrl: './application-lot-result-info.component.html',
  styleUrls: ['./application-lot-result-info.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ApplicationLotResultInfoComponent {
  model: ApplicationLotResultDto;
  @Input('model') set modelSetter(value: ApplicationLotResultDto) {
    if (!value) {
      return;
    }

    this.model = value;
  }

  resultTypes = ApplicationLotResultType;
  resultTypeLocalization = ApplicationLotResultTypeEnumLocalization;
}
