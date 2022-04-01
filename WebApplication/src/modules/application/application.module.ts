import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { PartModule } from 'src/infrastructure/part.module';
import { CandidateModule } from '../candidate/candidate.module';
import { ApplicationRoutingModule } from './application-routing.module';
import { ApplicationCandidateCommitComponent } from './components/application-candidate-commit/application-candidate-commit.component';
import { ApplicationCandidateSelectComponent } from './components/application-candidate-select/application-candidate-select.component';
import { ApplicationCommitHistoryComponent } from './components/application-commit-history/application-commit-history.component';
import { ApplicationCommitComponent } from './components/application-commit/application-commit.component';
import { ApplicationDraftComponent } from './components/application-draft/application-draft.component';
import { ApplicationLotResultInfoComponent } from './components/application-lot-result-info/application-lot-result-info.component';
import { ApplicationLotResultModalComponent } from './components/application-lot-result-modal/application-lot-result-modal.component';
import { ApplicationNewComponent } from './components/application-new/application-new.component';
import { ApplicationReportComponent } from './components/application-report/application-report.component';
import { ApplicationSearchComponent } from './components/application-search/application-search.component';
import { ApplicantFormComponent } from './components/common/applicant-form/applicant-form.component';
import { DiplomaFormComponent } from './components/common/diploma-form/diploma-form.component';
import { DocumentsFormComponent } from './components/common/documents-form/documents-form.component';
import { EducationFormComponent } from './components/common/education-form/education-form.component';
import { MedicalCertificateComponent } from './components/common/medical-certificate-form/medical-certificate-form.component';
import { PreviousApplicationFormComponent } from './components/common/previous-application-form/previous-application-form.component';
import { RepresentativeFormComponent } from './components/common/representative-form/representative-form.component';
import { TaxesFormComponent } from './components/common/taxes-form/taxes-form.component';
import { TrainingFormComponent } from './components/common/training-form/training-form.component';
import { ApplicationCommitHistoryResolver } from './services/application-commit-history.resolver';
import { ApplicationCommitResolver } from './services/application-commit.resolver';
import { ApplicationDraftResolver } from './services/application-draft.resolver';
import { ApplicationDraftResource } from './services/application-draft.resource';
import { ApplicationReportSearchFilter } from './services/application-report-search.filter';
import { ApplicationReportResource } from './services/application-report.resource';
import { ApplicationSearchFilter } from './services/application-search.filter';
import { ApplicationResource } from './services/application.resource';

@NgModule({
  declarations: [
    ApplicationSearchComponent,
    ApplicationNewComponent,
    ApplicantFormComponent,
    ApplicationCandidateCommitComponent,
    ApplicationCandidateSelectComponent,
    EducationFormComponent,
    TrainingFormComponent,
    TaxesFormComponent,
    ApplicationCommitComponent,
    ApplicationCommitHistoryComponent,
    DocumentsFormComponent,
    ApplicationLotResultModalComponent,
    ApplicationLotResultInfoComponent,
    DiplomaFormComponent,
    RepresentativeFormComponent,
    PreviousApplicationFormComponent,
    MedicalCertificateComponent,
    ApplicationDraftComponent,
    ApplicationReportComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PartModule,
    CandidateModule,
    CommonBootstrapModule,
    ApplicationRoutingModule,
    TranslateModule
  ],
  providers: [
    ApplicationResource,
    ApplicationCommitResolver,
    ApplicationCommitHistoryResolver,
    ApplicationSearchFilter,
    ApplicationDraftResource,
    ApplicationDraftResolver,
    ApplicationReportSearchFilter,
    ApplicationReportResource
  ]
})
export class ApplicationModule { }
