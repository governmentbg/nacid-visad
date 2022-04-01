import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HelpTooltipComponent } from './components/help-tooltip/help-tooltip.component';
import { SvgIconComponent } from './components/svg-icon/svg-icon.component';
import { ValidationErrorMessageComponent } from './components/validation-error-message/validation-error-message.component';

@NgModule({
  declarations: [
    SvgIconComponent,
    HelpTooltipComponent,
    ValidationErrorMessageComponent
  ],
  imports: [
    NgbModule,
    ReactiveFormsModule,
    CommonModule,
    FormsModule
  ],
  exports: [
    SvgIconComponent,
    HelpTooltipComponent,
    ValidationErrorMessageComponent,
    NgbModule,
  ]
})
export class CommonBootstrapModule { }
