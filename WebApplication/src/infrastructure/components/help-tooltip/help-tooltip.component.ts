import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-help-tooltip',
  templateUrl: './help-tooltip.component.html',
  styleUrls: ['./help-tooltip.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HelpTooltipComponent {
  @Input() icon = 'question-circle';
  @Input() tooltipText: string;
}
