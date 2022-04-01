import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RegExps } from 'src/infrastructure/constants/constants';
import { handleDomainError } from 'src/infrastructure/utils/domain-error-handler.util';
import { TaxType } from 'src/modules/application/enums/tax-type.model';
import { TaxAccountDto } from 'src/modules/application/models/tax-account.dto';
import { TaxDto } from 'src/modules/application/models/tax.dto';
import { NomenclatureResource } from 'src/modules/nomenclature/common/services/nomenclature.resource';
import { Bank } from 'src/modules/nomenclature/models/bank-type.model';
import { CommonFormComponent } from '../../../../../infrastructure/components/common-form.component';

@Component({
  selector: 'app-taxes-form',
  templateUrl: './taxes-form.component.html',
  styleUrls: ['./taxes-form.component.css']
})
export class TaxesFormComponent extends CommonFormComponent<TaxAccountDto> implements OnInit {
  taxType = TaxType;
  invalidIban: boolean = false;

  constructor(
    private nomenclatureResource: NomenclatureResource<Bank>,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void {
    if (this.model.taxes.length < 1) {
      const educationTax = new TaxDto(TaxType.educationTax);
      this.model.taxes.push(educationTax);

      const trainingTax = new TaxDto(TaxType.trainingTax);
      this.model.taxes.push(trainingTax);
    }
  }

  getBankByIban(iban: string, index: number) {
    if (iban) {
      let correctIban = '';
      for (let i = 0; i < iban.length; i++) {
        if (iban[i] != ' ') {
          correctIban += iban[i].toUpperCase();
        }
      }
      iban = correctIban;
      this.model.taxes[index].iban = correctIban;

      if (iban.length == 22) {
        this.nomenclatureResource.getBankByIban(iban).subscribe((bank: Bank) => {
          this.model.taxes[index].invalidIban = false;
          this.model.taxes[index].bank = bank.name;
          this.model.taxes[index].bic = bank.bic;
        }, (err) => {
          this.model.taxes[index].invalidIban = true;
          this.model.taxes[index].bank = null;
          this.model.taxes[index].bic = null;
          handleDomainError(
            err,
            [
              { code: 'Application_InvalidIBAN', text: "Невалиден IBAN " },
            ],
            this.toastrService
          )
        })
      } else {
        this.model.taxes[index].invalidIban = true;
        this.model.taxes[index].bank = null;
        this.model.taxes[index].bic = null;
      }
    } else {
      this.model.taxes[index].invalidIban = false;
      this.model.taxes[index].bank = null;
      this.model.taxes[index].bic = null;
    }
  }

  validateIban(event: any, model: string) {
    const latinAndNumberRegex = new RegExp(RegExps.LATIN_AND_NUMBER_ONLY_REGEX);
    const inputChar = String.fromCharCode(event.charCode);

    if (!latinAndNumberRegex.test(inputChar)) {
      event.preventDefault();
    }
  }
}
