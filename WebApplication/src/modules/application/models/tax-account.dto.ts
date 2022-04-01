import { TaxDto } from './tax.dto';

export class TaxAccountDto {
  taxes: TaxDto[];

  constructor() {
    this.taxes = [];
  }
}
