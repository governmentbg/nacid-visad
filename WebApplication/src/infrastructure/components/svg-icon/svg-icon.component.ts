import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-icon',
  templateUrl: './svg-icon.component.html',
  // styles: [
  //   `
  //    :host {
  //      display: inline-flex;
  //     align-items: center;
  //     justify-content: center;
  //    }
  //   `
  // ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SvgIconComponent {
  @Input() width = 16;
  @Input() height = 16;
  @Input() icon: string;
  @Input() color: string;
}
