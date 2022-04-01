import { AbDocsUtilsModule } from '@abb/ab-docs-utils';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ToastrModule } from 'ngx-toastr';
import { LoadingIndicatorComponent } from 'src/app/loading-indicator/loading-indicator.component';
import { CommonBootstrapModule } from 'src/infrastructure/common-bootstrap.module';
import { Configuration, configurationFactory } from 'src/infrastructure/configuration/configuration';
import { AuthGuard } from 'src/infrastructure/guards/auth.guard';
import { InterceptorsModule } from 'src/infrastructure/interceptors/interceptors.module';
import { PartModule } from 'src/infrastructure/part.module';
import { RoleService } from 'src/infrastructure/services/role.service';
import { SharedService } from 'src/infrastructure/services/shared-service';
import { ApplicationModule } from 'src/modules/application/application.module';
import { NomenclatureModule } from 'src/modules/nomenclature/nomenclature.module';
import { UserModule } from 'src/modules/user/user.module';
import { AppMenuComponent } from './app-menu/app-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { AppUserComponent } from './app-user/app-user.component';
import { AppComponent } from './app.component';
import { LoadingIndicatorService } from './loading-indicator/loading-indicator.service';
import { ScrollToTopBtnComponent } from './scroll-to-top-btn/scroll-to-top-btn.component';

@NgModule({
  declarations: [
    AppComponent,
    AppMenuComponent,
    AppUserComponent,
    ScrollToTopBtnComponent,
    LoadingIndicatorComponent
  ],
  imports: [
    AbDocsUtilsModule,
    BrowserModule,
    FormsModule,
    CommonModule,
    CommonBootstrapModule,
    ApplicationModule,
    NomenclatureModule,
    UserModule,
    PartModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    AuthGuard,
    LoadingIndicatorService,
    Configuration,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      deps: [Configuration],
      multi: true
    },
    InterceptorsModule,
    RoleService,
    SharedService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
