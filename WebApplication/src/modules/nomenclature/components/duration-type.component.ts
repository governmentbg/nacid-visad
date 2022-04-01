import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { DurationType } from '../models/duration-type.model';

@Component({
  selector: 'app-duration-type',
  templateUrl: './../common/base-nomenclature.component.html',
})
export class DurationTypeComponent extends BaseNomenclatureComponent<DurationType> implements OnInit {

  ngOnInit(): void {
    this.init(DurationType, 'DurationType');
  }

}
