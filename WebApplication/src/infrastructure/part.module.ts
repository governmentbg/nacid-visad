import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PartsEditWarningModalComponent } from 'src/modules/application/components/parts-edit-warning-modal/parts-edit-warning-modal.component';
import { PartPanelComponent } from './components/part-panel/part-panel.component';
import { PartResource } from './services/part.resource';

@NgModule({
  declarations: [
    PartPanelComponent,
    PartsEditWarningModalComponent
  ],
  imports: [
    CommonModule,
    TranslateModule
  ],
  exports: [
    PartPanelComponent,
    PartsEditWarningModalComponent
  ],
  providers: [
    PartResource
  ],
})
export class PartModule { }
