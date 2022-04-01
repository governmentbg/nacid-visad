import { Directive, ElementRef, OnInit } from '@angular/core';

@Directive({
	selector: '[myAutofocus]'
})
export class AutofocusDirective implements OnInit {

	constructor(private elementRef: ElementRef) { };

	ngOnInit(): void {
		this.elementRef.nativeElement.focus();
	}

}
