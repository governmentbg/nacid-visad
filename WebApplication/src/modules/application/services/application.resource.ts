import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { IFilterable } from 'src/infrastructure/interfaces/filterable.interface';
import { SearchResultItemDto } from 'src/infrastructure/models/search-result-item.dto';
import { BaseResource } from 'src/infrastructure/services/base.resource';
import { ApplicantDto } from '../models/applicant.dto';
import { ApplicationCommitHistoryItemDto } from '../models/application-commit-history-item.dto';
import { ApplicationCommitDto } from '../models/application-commit.dto';
import { ApplicationLotResultDto } from '../models/application-lot-result.dto';
import { ApplicationSearchResultItemDto } from '../models/application-search-result-item.dto';
import { ApplicationDto } from '../models/application.dto';
import { CommitInfoDto } from '../models/commit-info.dto';
import { CreateApplicationLotResultDto } from '../models/create-application-lot-result.dto';
import { ApplicationSearchFilter } from './application-search.filter';

@Injectable()
export class ApplicationResource extends BaseResource
  implements IFilterable<ApplicationSearchFilter, SearchResultItemDto<ApplicationSearchResultItemDto>>{
  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
    super(http, configuration, 'Application');
  }

  getFiltered(filter?: ApplicationSearchFilter): Observable<SearchResultItemDto<ApplicationSearchResultItemDto>> {
    return this.http.get<SearchResultItemDto<ApplicationSearchResultItemDto>>(`${this.baseUrl}${this.composeQueryString(filter)}`);
  }

  getCommit(lotId: number, commitId: number): Observable<ApplicationCommitDto> {
    return this.http.get<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/commit/${commitId}`);
  }

  getHistory(lotId: number): Observable<{
    hasResult: boolean, result: ApplicationLotResultDto, commits: ApplicationCommitHistoryItemDto[], actualCommitId: number
  }> {
    return this.http.get<{
      hasResult: boolean, result: ApplicationLotResultDto, commits: ApplicationCommitHistoryItemDto[], actualCommitId: number
    }>(`${this.baseUrl}/lot/${lotId}/history`);
  }

  createApplication(model: ApplicationDto): Observable<CommitInfoDto> {
    return this.http.post<CommitInfoDto>(this.baseUrl, model);
  }

  startModification(lotId: number, changeStateDescription: string): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/startmodification?changeStateDescription=${changeStateDescription}`, null);
  }

  finishModification(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/finishmodification`, null);
  }

  cancelModification(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/cancelmodification`, null);
  }

  eraseApplication(lotId: number, changeStateDescription: string): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/erase?changeStateDescription=${changeStateDescription}`, null);
  }

  revertErasedApplication(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/revertErased`, null);
  }

  approveApplication(lotId: number): Observable<ApplicationCommitDto> {
    return this.http.post<ApplicationCommitDto>(`${this.baseUrl}/lot/${lotId}/approve`, null);
  }

  addApplicationLotResult(lotId: number, model: CreateApplicationLotResultDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/lot/${lotId}/result`, model);
  }

  getApplicationLotResultSigningInformation(resultId: number): Observable<{ content: string, signatureLineIds: string[], filename: string }> {
    return this.http.get<{ content: string, signatureLineIds: string[], filename: string }>(`${this.baseUrl}/result/${resultId}/signingInformation`);
  };

  updateApplicationLotResultFile(resultId: number, content: string): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/result/${resultId}/signingInformation`, { content: content, resultId: resultId });
  }

  deleteLot(lotId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/lot/${lotId}`);
  }

  getApplicantData(): Observable<ApplicantDto> {
    return this.http.get<ApplicantDto>(`${this.baseUrl}/applicant`);
  }

  annulment(lotId: number, changeStateDescription: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/lot/${lotId}/annulment?changeStateDescription=${changeStateDescription}`, null);
  }

  refuseSign(lotId: number, changeStateDescription: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/lot/${lotId}/refusesign?changeStateDescription=${changeStateDescription}`, null);
  }
}
