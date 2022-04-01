import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { SchoolYear } from '../models/school-year.model';

@Component({
  selector: 'app-school-year',
  templateUrl: './school-year.component.html',
})
export class SchoolYearComponent extends BaseNomenclatureComponent<SchoolYear> implements OnInit {
  ngOnInit(): void {
    this.init(SchoolYear, 'SchoolYear');
  }

  yearChanged(item: SchoolYear): void {
    item.name = `${item.fromYear}/${item.toYear}`;
  }
}
