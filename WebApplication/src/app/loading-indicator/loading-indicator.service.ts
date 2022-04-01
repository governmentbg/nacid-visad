import { Injectable } from '@angular/core';
import { Subject, Subscription } from 'rxjs';

@Injectable()
export class LoadingIndicatorService {
  private subject = new Subject();

  subscribe(next: (value: boolean) => void): Subscription {
    return this.subject.subscribe(next);
  }

  show(): void {
    this.subject.next(true);
  }

  hide(): void {
    this.subject.next(false);
  }
}
