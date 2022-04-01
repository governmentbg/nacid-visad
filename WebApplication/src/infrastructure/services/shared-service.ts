import { Injectable } from '@angular/core';
import { RegExps } from '../constants/constants';

@Injectable()
export class SharedService {
	constructor(
	) { }

	public trackByFn(item: any): void {
		return item.name
	}

	public filterDate(event: any, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}
		const pattern = new RegExp(RegExps.NUMBER_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if (inputChar === '.' || inputChar === ',' || inputChar === '/' || inputChar === '-') {
			return;
		} else {
			if (!pattern.test(inputChar)) {
				event.preventDefault();
			}
		}
	}

	public filterMail(event: any, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}
		const pattern = new RegExp(RegExps.CYRILLIC_NAMES_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if (inputChar === '-' || inputChar === '.') {
			return;
		} else {
			if (pattern.test(inputChar)) {
				event.preventDefault();
			}
		}
	}

	public filterPhone(event: any, model: string, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}
		const pattern = new RegExp(RegExps.PHONE_NUMBER_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if ((!model || model.length === 0) && inputChar === '+' || inputChar === ' ') {
			return;
		} else {
			if (!pattern.test(inputChar)) {
				event.preventDefault();
			}
		}
	}

	public onlyLatinLetters(event: any, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}

		const pattern = new RegExp(RegExps.LATIN_NAMES_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if (!pattern.test(inputChar)) {
			event.preventDefault();
		}
	}

	public onlyCyrillicLetters(event: any, shouldFilter = true) {
		if (!shouldFilter) {
			return;
		}

		const pattern = new RegExp(RegExps.CYRILLIC_NAMES_REGEX);
		const inputChar = String.fromCharCode(event.charCode);

		if (!pattern.test(inputChar)) {
			event.preventDefault();
		}
	}
}
