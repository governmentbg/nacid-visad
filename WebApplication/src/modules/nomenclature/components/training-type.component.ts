import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { TrainingType } from '../models/training-type.model';

@Component({
  selector: 'app-training-type',
  templateUrl: './../common/base-nomenclature.component.html',
})
export class TrainingTypeComponent extends BaseNomenclatureComponent<TrainingType> implements OnInit {
  ngOnInit(): void {
    this.init(TrainingType, 'TrainingType');
  }
}
