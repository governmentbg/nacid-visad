import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[awaitable]'
})
export class AwaitableDirective {
  @Input() click: Function;
  @Input('disabled') set disabledSetter(value: boolean | null) {
    this.disabled = value;
    this.changeDisabledState(value);
  }
  @Input() clickParams: any[];
  @Input() clickContext;

  private disabled = false;
  private isLoading = false;

  constructor(
    private elementRef: ElementRef,
    renderer: Renderer2
  ) {
    renderer.listen(elementRef.nativeElement, 'click', (event: any) => {
      if (this.isLoading || this.disabled) {
        return;
      }

      if (event.which !== 1 || !this.click) {
        return;
      }

      event.preventDefault();

      const onClickBound = this.click.bind(this.clickContext);
      const result = this.clickParams?.length ? onClickBound(...this.clickParams, event) : onClickBound(event);

      const that = this;

      // For subscribe
      if (result && result.next && typeof (result.next) === 'function') {
        this.setIsLoading(true);
        result.complete = that.completeLoading;
        result.error = that.completeLoading;
      }

      // For observable
      if (result && result.subscribe && typeof (result.subscribe) === 'function') {
        this.setIsLoading(true);
        result.subscribe(
          that.completeLoading,
          that.completeLoading
        );
      }

      // For promise
      if (result && result.then && typeof (result.then) === 'function') {
        this.setIsLoading(true);
        result.then(that.completeLoading)
          .catch(that.completeLoading);
      }
    });
  }

  private completeLoading = () => this.setIsLoading(false);

  private setIsLoading(isLoading: boolean): void {
    this.isLoading = isLoading;
    this.changeDisabledState(this.isLoading);
  }

  private changeDisabledState(isDisabled: boolean): void {
    this.elementRef.nativeElement.disabled = isDisabled;
  }
}
