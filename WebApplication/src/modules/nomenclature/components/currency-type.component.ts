import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { CurrencyType } from '../models/currency-type.model';

@Component({
  selector: 'app-currency-type',
  templateUrl: './../common/base-nomenclature.component.html',
})
export class CurrencyTypeComponent extends BaseNomenclatureComponent<CurrencyType> implements OnInit {

  ngOnInit(): void {
    this.init(CurrencyType, 'CurrencyType');
  }

}
