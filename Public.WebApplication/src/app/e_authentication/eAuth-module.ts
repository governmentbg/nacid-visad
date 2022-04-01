import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateModule } from '@ngx-translate/core';
import { EAuthRedirectResponseComponent } from './eAuth-redirect-response.component';
import { EAuthSendRequestComponent } from './eAuth-send-request.component';

@NgModule({
  declarations: [
    EAuthSendRequestComponent,
    EAuthRedirectResponseComponent
  ],
  imports: [
    BrowserModule,
    TranslateModule
  ],
  providers: [
  ]
})
export class EAuthModule { }
