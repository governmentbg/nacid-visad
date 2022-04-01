import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { NomenclatureResource } from './common/services/nomenclature.resource';
import { SchoolYearResource } from './common/services/school-year.resource';
import { ApplicationFileTypeComponent } from './components/application-file-type.component';
import { CurrencyTypeComponent } from './components/currency-type.component';
import { DiplomaTypeComponent } from './components/diploma-type.component';
import { DurationTypeComponent } from './components/duration-type.component';
import { EducationFormTypeComponent } from './components/education-form-type.component';
import { FileTemplateComponent } from './components/file-template/file-template.component';
import { LanguageDegreeComponent } from './components/language-degree.component';
import { LanguageComponent } from './components/language.component';
import { NomenclatureHostComponent } from './components/nomenclature-host/nomenclature-host.component';
import { SchoolYearComponent } from './components/school-year.component';
import { TrainingTypeComponent } from './components/training-type.component';
import { NomenclatureRoutingModule } from './nomenclature-routing.module';
import { ApplicationFileTypeResource } from './services/application-file-type.resource';
import { FileTemplateResource } from './services/file-template.resource';

@NgModule({
  declarations: [
    NomenclatureHostComponent,
    DurationTypeComponent,
    CurrencyTypeComponent,
    ApplicationFileTypeComponent,
    FileTemplateComponent,
    EducationFormTypeComponent,
    LanguageDegreeComponent,
    TrainingTypeComponent,
    SchoolYearComponent,
    DiplomaTypeComponent,
    LanguageComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    CommonBootstrapModule,
    RouterModule,
    NomenclatureRoutingModule,
    TranslateModule
  ],
  providers: [
    NomenclatureResource,
    FileTemplateResource,
    ApplicationFileTypeResource,
    SchoolYearResource
  ]
})
export class NomenclatureModule { }
