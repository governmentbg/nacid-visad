import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { TaxType } from '../enums/tax-type.model';

export class TaxDto {
  id: number;

  type: TaxType;

  iban: string;
  accountHolder: string;
  amount: number | null;
  currencyType: NomenclatureDto;
  additionalInfo: string;
  bank: string;
  bic: string;

  invalidIban = false;

  constructor(type: TaxType) {
    this.type = type;
  }
}
