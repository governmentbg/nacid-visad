import { Directive, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';
import * as moment from 'moment';

@Directive({
	selector: '[validDate]',
	providers: [
		{
			provide: NG_VALIDATORS,
			useExisting: forwardRef(() => ValidDateDirective),
			multi: true
		}
	]
})

export class ValidDateDirective implements Validator {
	private valFn = ValidDateValidator();

	validate(control: AbstractControl): { [key: string]: any } | null {
		return this.valFn(control);
	}
}


export function ValidDateValidator(): ValidatorFn {
	return (control: AbstractControl): { [key: string]: any } | null => {
		const value = control.value as string || '';

		if (!value) {
			return null;
		}

		const mdt = moment.utc(value);
		return mdt.isValid() ? null : { 'invalidDate': true };
	};
}
