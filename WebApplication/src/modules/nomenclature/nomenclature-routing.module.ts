import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
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

const routes: Routes = [
  {
    path: 'nomenclature',
    component: NomenclatureHostComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'currencyType',
        pathMatch: 'full'
      },
      {
        path: 'durationType',
        component: DurationTypeComponent
      },
      {
        path: 'currencyType',
        component: CurrencyTypeComponent
      },
      {
        path: 'applicationFileType',
        component: ApplicationFileTypeComponent
      },
      {
        path: 'template',
        component: FileTemplateComponent
      },
      {
        path: 'educationFormType',
        component: EducationFormTypeComponent
      },
      {
        path: 'languageDegree',
        component: LanguageDegreeComponent
      },
      {
        path: 'trainingType',
        component: TrainingTypeComponent
      },
      {
        path: 'schoolYear',
        component: SchoolYearComponent
      },
      {
        path: 'diplomaType',
        component: DiplomaTypeComponent
      },
      {
        path: 'language',
        component: LanguageComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NomenclatureRoutingModule { }
