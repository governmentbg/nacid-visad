import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { CommitInfoDto } from 'src/modules/application/models/commit-info.dto';
import { CandidateApplicationDataDto } from '../models/candidate-application-data.dto';
import { CandidateApplicationsDto } from '../models/candidate-applications.dto';
import { CandidateCommitHistoryItemDto } from '../models/candidate-commit-history-item.dto';
import { CandidateCommitDto } from '../models/candidate-commit.dto';
import { CandidateCreateDto } from '../models/candidate-create.dto';
import { CandidateSearchResultItemDto } from '../models/candidate-search-result-item.dto';
import { CandidateDto } from '../models/candidate.dto';
import { CandidateSearchFilter } from './candidate-search.filter';

@Injectable()
export class CandidateResource extends BaseResource
  implements IFilterable<CandidateSearchFilter, SearchResultItemDto<CandidateSearchResultItemDto>>{
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'Candidate');
  }

  getFiltered(filter?: CandidateSearchFilter): Observable<SearchResultItemDto<CandidateSearchResultItemDto>> {
    return this.http.get<SearchResultItemDto<CandidateSearchResultItemDto>>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  selectCandidates(filter: CandidateSearchFilter): Observable<SearchResultItemDto<CandidateCommitDto>> {
    return this.http.get<SearchResultItemDto<CandidateCommitDto>>(`${this.baseUrl}/Select${this.composeQueryString(filter)}`);
  }

  getCommit(lotId: number, commitId: number): Observable<CandidateApplicationsDto> {
    return this.http.get<CandidateApplicationsDto>(`${this.baseUrl}/lot/${lotId}/commit/${commitId}`);
  }

  getHistory(lotId: number): Observable<{
    commits: CandidateCommitHistoryItemDto[], actualCommitId: number
  }> {
    return this.http.get<{ commits: CandidateCommitHistoryItemDto[], actualCommitId: number }>(`${this.baseUrl}/lot/${lotId}/history`);
  }

  createCandidate(model: CandidateCreateDto): Observable<CommitInfoDto> {
    return this.http.post<CommitInfoDto>(this.baseUrl, model);
  }

  createApplicationCandidate(model: CandidateCreateDto): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/ApplicationCandidate`, model);
  }

  startModification(lotId: number, changeStateDescription: string): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/lot/${lotId}/startmodification?changeStateDescription=${changeStateDescription}`, null);
  }

  finishApplicationCommitModification(commitId: number, entity: CandidateDto, applicationCommitId: number): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/commit/${commitId}/finishApplicationCandidateModification`, { commitId, candidateDto: entity, applicationCommitId });
  }

  finishModification(lotId: number, partId: number, entity: any): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/lot/${lotId}/part/${partId}/finishmodification`, entity);
  }

  cancelModification(lotId: number): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/lot/${lotId}/cancelmodification`, null);
  }

  eraseApplication(lotId: number, changeStateDescription: string): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/lot/${lotId}/erase?changeStateDescription=${changeStateDescription}`, null);
  }

  revertErasedApplication(lotId: number): Observable<CandidateCommitDto> {
    return this.http.post<CandidateCommitDto>(`${this.baseUrl}/lot/${lotId}/revertErased`, null);
  }

  deleteLot(lotId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/lot/${lotId}`);
  }

  getCandidateApplicationData(candidateCommitId: number): Observable<CandidateApplicationDataDto> {
    return this.http.get<CandidateApplicationDataDto>(`${this.baseUrl}/candidateData?candidateCommitId=${candidateCommitId}`);
  }
}
