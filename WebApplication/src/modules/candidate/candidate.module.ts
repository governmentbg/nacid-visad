import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { PartModule } from 'src/infrastructure/part.module';
import { CandidateApplicationsComponent } from '../application/components/application-candidate-commit/candidate-applications/candidate-applications.component';
import { CandidateRoutingModule } from './candidate-routing.module';
import { CandidateCommitHistoryComponent } from './components/candidate-commit-history/candidate-commit-history.component';
import { CandidateCommitComponent } from './components/candidate-commit/candidate-commit.component';
import { CandidateFormComponent } from './components/candidate-form/candidate-form.component';
import { CandidateNewModalComponent } from './components/candidate-new/candidate-new-modal.component';
import { CandidateNewComponent } from './components/candidate-new/candidate-new.component';
import { CandidateSearchComponent } from './components/candidate-search/candidate-search.component';
import { CandidateCommitHistoryResolver } from './services/candidate-commit-history.resolver';
import { CandidateCommitResolver } from './services/candidate-commit.resolver';
import { CandidateSearchFilter } from './services/candidate-search.filter';
import { CandidateResource } from './services/candidate.resource';

@NgModule({
  declarations: [
    CandidateSearchComponent,
    CandidateNewComponent,
    CandidateFormComponent,
    CandidateCommitComponent,
    CandidateCommitHistoryComponent,
    CandidateFormComponent,
    CandidateNewModalComponent,
    CandidateApplicationsComponent
  ],
  exports: [
    CandidateFormComponent,
    CandidateNewModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PartModule,
    CommonBootstrapModule,
    CandidateRoutingModule,
    TranslateModule
  ],
  providers: [
    CandidateResource,
    CandidateCommitResolver,
    CandidateCommitHistoryResolver,
    CandidateSearchFilter
  ]
})
export class CandidateModule { }
