import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { CandidateCommitHistoryComponent } from './components/candidate-commit-history/candidate-commit-history.component';
import { CandidateCommitComponent } from './components/candidate-commit/candidate-commit.component';
import { CandidateNewComponent } from './components/candidate-new/candidate-new.component';
import { CandidateSearchComponent } from './components/candidate-search/candidate-search.component';
import { CandidateCommitHistoryResolver } from './services/candidate-commit-history.resolver';
import { CandidateCommitResolver } from './services/candidate-commit.resolver';

const routes: Routes = [
  {
    path: 'candidate/search',
    component: CandidateSearchComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'candidate/new',
    component: CandidateNewComponent
  },
  {
    path: 'candidate/lot/:lotId/commit/:commitId',
    component: CandidateCommitComponent,
    resolve: {
      dto: CandidateCommitResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'candidate/lot/:lotId/history',
    component: CandidateCommitHistoryComponent,
    resolve: {
      model: CandidateCommitHistoryResolver
    },
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CandidateRoutingModule { }
