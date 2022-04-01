import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { ApplicationCommitHistoryComponent } from './components/application-commit-history/application-commit-history.component';
import { ApplicationCommitComponent } from './components/application-commit/application-commit.component';
import { ApplicationDraftComponent } from './components/application-draft/application-draft.component';
import { ApplicationNewComponent } from './components/application-new/application-new.component';
import { ApplicationReportComponent } from './components/application-report/application-report.component';
import { ApplicationSearchComponent } from './components/application-search/application-search.component';
import { ApplicationCommitHistoryResolver } from './services/application-commit-history.resolver';
import { ApplicationCommitResolver } from './services/application-commit.resolver';
import { ApplicationDraftResolver } from './services/application-draft.resolver';

const routes: Routes = [
  {
    path: 'application/search',
    component: ApplicationSearchComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'application/new',
    component: ApplicationNewComponent
  },
  {
    path: 'application/lot/:lotId/commit/:commitId',
    component: ApplicationCommitComponent,
    resolve: {
      commit: ApplicationCommitResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'application/lot/:lotId/history',
    component: ApplicationCommitHistoryComponent,
    resolve: {
      model: ApplicationCommitHistoryResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'application/draft/:draftId',
    component: ApplicationDraftComponent,
    resolve: {
      model: ApplicationDraftResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'reports',
    component: ApplicationReportComponent,
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicationRoutingModule { }
