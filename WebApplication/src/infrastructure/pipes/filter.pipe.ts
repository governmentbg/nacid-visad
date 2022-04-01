import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'filter' })
export class FilterPipe implements PipeTransform {
	transform(items: any[], key: string, value: any): any[] {
		return items.filter((e: any) => e[key] === value);
	}
}
