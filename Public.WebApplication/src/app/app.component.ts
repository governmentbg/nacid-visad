import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { EAuthService } from './e_authentication/eAuth-service';
import { UsernameUpdateService } from './e_authentication/username-update-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  langSuffix: string = "EN";
  imgSuffix: string = "bg";
  currentYear: number;

  username: string | undefined;

  constructor(
    private translate: TranslateService,
    private usernameUpdateService: UsernameUpdateService,
    private eAuthService: EAuthService
  ) {
    this.currentYear = new Date().getFullYear();
    this.translate.setDefaultLang('bg');

    this.usernameUpdateService.subscribe((username: string) => {
      this.username = username
    });

    const savedUsername = sessionStorage.getItem('userName');
    if (savedUsername != null) {
      this.usernameUpdateService.emit(savedUsername);
    }
  }

  switchLang() {
    this.langSuffix === "БГ" ? this.langSuffix = "EN" : this.langSuffix = "БГ";
    this.translate.use(this.langSuffix === "БГ" ? 'en' : 'bg');
    this.imgSuffix === "bg" ? this.imgSuffix = "en" : this.imgSuffix = "bg";
  }

  scrollToTop(behavior: 'auto' | 'smooth' = 'smooth'): void {
    window.scrollTo({
      top: 0,
      left: 0,
      behavior
    });
  }

  logIn(): void {
    this.eAuthService.authenticate('default', '/', 'navigate');
  }

  logOut() {
    this.eAuthService.logOut();
  }
}
