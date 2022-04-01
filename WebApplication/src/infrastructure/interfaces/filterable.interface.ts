import { Observable } from 'rxjs';

export interface IFilterable<TFilter, SearchResultItemDto> {
	getFiltered(filter?: TFilter): Observable<SearchResultItemDto>;
}
