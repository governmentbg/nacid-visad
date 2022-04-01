import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { ApplicationFileType } from '../models/application-file-type.model';

@Component({
  selector: 'app-file-type',
  templateUrl: './application-file-type.component.html',
})
export class ApplicationFileTypeComponent extends BaseNomenclatureComponent<ApplicationFileType> implements OnInit {

  ngOnInit(): void {
    this.init(ApplicationFileType, 'ApplicationFileType');
  }
}
