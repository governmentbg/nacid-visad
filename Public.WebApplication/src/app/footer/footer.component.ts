import { Component, HostListener } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: 'app-footer',
  templateUrl: 'footer.component.html',
  styleUrls: ['footer.component.css']
})
export class FooterComponent {
  isBtnVisible = false;
  showAboutSection = false;
  firstClick = false;
  currentYear: number;

  ngAfterViewChecked(behavior: 'auto' | 'smooth' = 'smooth'): void {
    if (this.showAboutSection && this.firstClick) {
      window.scrollTo({
        top: window.scrollY + 500,
        left: 0,
        behavior
      });
      this.firstClick = false;
    }
  }

  constructor(public translate: TranslateService) {
    translate.currentLang = 'bg';
    this.currentYear = new Date().getFullYear();
  }
}
