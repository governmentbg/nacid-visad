import { Component } from '@angular/core';
import { RegExps } from 'src/infrastructure/constants/constants';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { RepresentativeType } from 'src/modules/application/enums/representative-type.enum';
import { RepresentativeDto } from 'src/modules/application/models/representative.dto';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-representative-form',
  templateUrl: './representative-form.component.html'
})
export class RepresentativeFormComponent extends CommonFormComponent<RepresentativeDto> {
  representativeTypes = RepresentativeType;
  nameRegex = RegExps.LATIN_AND_CYRILLIC_NAMES_REGEX;
  phoneRegex = RegExps.PHONE_NUMBER_REGEX;
  mailRegex = RegExps.EMAIL_REGEX;
  cyrillicRegex = RegExps.CYRILLIC_NAMES_REGEX;

  invalidEgn: boolean = false;

  date: Date = new Date();

  minSubmissionDate = { year: new Date().getFullYear() - 1, month: this.date.getMonth() + 1, day: this.date.getDate() };
  maxSubmissionDate = { year: new Date().getFullYear(), month: this.date.getMonth() + 1, day: this.date.getDate() };

  constructor(public sharedService: SharedService) {
    super()
  }

  onChange(type: RepresentativeType): void {
    const copiedRequestDocument = this.model.applicationForCertificate;
    const copiedDate = this.model.submissionDate;
    this.model = new RepresentativeDto();
    this.model.applicationForCertificate = copiedRequestDocument;
    this.model.submissionDate = copiedDate;
    this.model.type = type;
    this.model.hasRepresentative = true;
    this.modelChange.emit(this.model);
  }

  validateEgn(egn: string) {
    if (egn.length == 10) {
      let monthNumbers = +egn.substring(2, 4);
      let dayNumbers = +egn.substring(4, 6);
      if ((monthNumbers > 12 && monthNumbers <= 20) || (monthNumbers > 32 && monthNumbers <= 40) || dayNumbers > 31) {
        this.invalidEgn = true;
        return;
      }

      const weights: number[] = [2, 4, 8, 5, 10, 9, 7, 3, 6];
      const mod = 11;

      let sum = 0;
      let checkSum = +egn.substring(9, 10);

      for (let i = 0; i < 9; i++) {
        sum += +egn.substring(i, i + 1) * weights[i];
      }

      var validCheckSum = sum % mod;

      if (validCheckSum >= 10) {
        validCheckSum = 0;
      }

      if (validCheckSum == checkSum) {
        this.invalidEgn = false;
      }
      else {
        this.invalidEgn = true;
      }
    }
  }
}
