import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbDateAdapter, NgbDateParserFormatter, NgbDatepickerI18n, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { ActionConfirmationModalComponent } from './components/action-confirmation-modal/action-confirmation-modal.component';
import { AsyncSelectComponent } from './components/async-select/async-select.component';
import { CandidateModificationModalComponent } from './components/candidate-modification-modal/candidate-modification-modal.component';
import { ExportComponent } from './components/export/export.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { HelpTooltipComponent } from './components/help-tooltip/help-tooltip.component';
import { ImageSelectComponent } from './components/img-selector/image-select.component';
import { SvgIconComponent } from './components/svg-icon/svg-icon.component';
import { ValidationErrorMessageComponent } from './components/validation-error-message/validation-error-message.component';
import { AutofocusDirective } from './directives/auto-focus.directive';
import { AwaitableDirective } from './directives/awaitable.directive';
import { BlockCopyPasteDirective } from './directives/block-copy-paste.directive';
import { ValidDateDirective } from './directives/valid-date.directive';
import { FilterPipe } from './pipes/filter.pipe';
import { CustomDatepickerI18n } from './services/datepicker-i18n.service';
import { MomentDateFormatter } from './services/moment-date.formatter';
import { StringDateAdapter } from './services/string-date.adapter';

@NgModule({
  imports: [
    NgbModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule
  ],
  declarations: [
    SvgIconComponent,
    AsyncSelectComponent,
    HelpTooltipComponent,
    FileUploadComponent,
    FilterPipe,
    ValidationErrorMessageComponent,
    ActionConfirmationModalComponent,
    CandidateModificationModalComponent,
    AwaitableDirective,
    ExportComponent,
    ImageSelectComponent,
    BlockCopyPasteDirective,
    ValidDateDirective,
    AutofocusDirective
  ],
  exports: [
    SvgIconComponent,
    AsyncSelectComponent,
    HelpTooltipComponent,
    FileUploadComponent,
    FilterPipe,
    ValidationErrorMessageComponent,
    ActionConfirmationModalComponent,
    AwaitableDirective,
    ExportComponent,
    NgbModule,
    ImageSelectComponent,
    BlockCopyPasteDirective,
    ValidDateDirective,
    AutofocusDirective
  ],
  providers: [
    { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n },
    { provide: NgbDateAdapter, useClass: StringDateAdapter },
    { provide: NgbDateParserFormatter, useClass: MomentDateFormatter }
  ]
})
export class CommonBootstrapModule { }
