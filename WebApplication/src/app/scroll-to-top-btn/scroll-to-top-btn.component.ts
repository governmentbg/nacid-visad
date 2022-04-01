import { ChangeDetectionStrategy, Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-scroll-to-top-btn',
  templateUrl: './scroll-to-top-btn.component.html',
  styleUrls: ['./scroll-to-top-btn.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScrollToTopBtnComponent {
  isBtnVisible = false;
  private window: Window;

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    this.window = window;
    this.isBtnVisible = this.window.pageYOffset !== 0;
  }

  scrollToTop(behavior: 'auto' | 'smooth' = 'smooth'): void {
    window.scrollTo({
      top: 0,
      left: 0,
      behavior
    });
  }
}
