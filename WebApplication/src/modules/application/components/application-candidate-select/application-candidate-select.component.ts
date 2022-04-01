import { ChangeDetectorRef, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingIndicatorService } from 'src/app/loading-indicator/loading-indicator.service';
import { CandidateModificationModalComponent } from 'src/infrastructure/components/candidate-modification-modal/candidate-modification-modal.component';
import { UserRoleAliases } from 'src/infrastructure/constants/constants';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { RoleService } from 'src/infrastructure/services/role.service';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { CandidateNewModalComponent } from 'src/modules/candidate/components/candidate-new/candidate-new-modal.component';
import { CandidateApplicationDataDto } from 'src/modules/candidate/models/candidate-application-data.dto';
import { CandidateCommitDto } from 'src/modules/candidate/models/candidate-commit.dto';
import { CandidateSearchFilter } from 'src/modules/candidate/services/candidate-search.filter';
import { CandidateResource } from 'src/modules/candidate/services/candidate.resource';

@Component({
  selector: 'app-application-candidate-select',
  templateUrl: './application-candidate-select.component.html',
  providers: [CandidateSearchFilter]
})
export class ApplicationCandidateSelectComponent implements OnInit {
  @Output() candidateCommitSelected: EventEmitter<CandidateCommitDto> = new EventEmitter();
  @Output() candidateApplicationData: EventEmitter<CandidateApplicationDataDto> = new EventEmitter();

  canAddNewCandidate: boolean;
  model: CandidateCommitDto[] = [];
  canLoadMore: boolean;
  showTable: boolean = false;
  modelCounts = 0;
  totalCounts = 0;

  candidateCountryNotFound: string;
  candidateBirthDateNotFound: Date;

  maxYearDate = { year: new Date().getFullYear() - 10, month: 1, day: 1 };
  minYearDate = { year: new Date().getFullYear() - 70, month: 1, day: 1 };

  ngOnInit(): void {
    this.canAddNewCandidate = this.roleService.hasRole(UserRoleAliases.ADMINISTRATOR, UserRoleAliases.UNIVERSITY_USER);
    this.cdRef.detectChanges();
  }

  constructor(
    public filter: CandidateSearchFilter,
    private resource: CandidateResource,
    private loadingIndicator: LoadingIndicatorService,
    private roleService: RoleService,
    private modal: NgbModal,
    private cdRef: ChangeDetectorRef,
    public sharedService: SharedService
  ) { }

  search(loadMore?: boolean): Subscription {
    this.candidateCountryNotFound = this.filter.country;
    this.candidateBirthDateNotFound = this.filter.birthDate;

    if (!loadMore) {
      this.filter.offset = 0;
    }

    if (!this.filter.countryId || !this.filter.birthDate) {
      return;
    }

    this.loadingIndicator.show();
    return this.resource.selectCandidates(this.filter)
      .pipe(
        finalize(() => this.loadingIndicator.hide())
      )
      .subscribe((model: SearchResultItemDto<CandidateCommitDto>) => {
        this.totalCounts = model.totalCount;

        if (!this.filter.offset) {
          this.model = model.items;
        } else {
          this.model = [...this.model, ...model.items];
        }

        this.canLoadMore = model.items.length === this.filter.limit;
        this.filter.offset = this.model.length;
        this.showTable = true;
        this.modelCounts = model.items.length;
      });
  }

  addNewCandidate(): void {
    if (!this.canAddNewCandidate) {
      return;
    }

    const candidateModal = this.modal.open(CandidateNewModalComponent, { backdrop: 'static', size: 'xl' });
    candidateModal.componentInstance.country = this.filter.country;
    candidateModal.componentInstance.birthDate = this.filter.birthDate;
    candidateModal.result
      .then((result: CandidateCommitDto) => {
        if (result) {
          this.candidateCommitSelected.emit(result);
        }
      });
  }

  loadMore(): void {
    this.filter.offset = this.model.length;
    this.search(true);
  }

  clearFilter(): void {
    this.filter.clear();
    this.model = [];
    this.showTable = false;
  }

  selectCandidate(candidate: CandidateCommitDto): void {
    if (candidate.state == CommitState.modification) {
      const warningModal = this.modal.open(CandidateModificationModalComponent, { backdrop: 'static' });
      warningModal.componentInstance.confirmationMessage = 'Личните данни на кандидата са в процес на промяна. Моля, впишете промените преди да продължите.';
    } else {
      this.resource.getCandidateApplicationData(candidate.id)
        .subscribe((candidateApplicationData: CandidateApplicationDataDto) => {
          if (candidateApplicationData != null) {
            this.candidateApplicationData.emit(candidateApplicationData);
          }
          this.candidateCommitSelected.emit(candidate);
        })
    }
  }
}
