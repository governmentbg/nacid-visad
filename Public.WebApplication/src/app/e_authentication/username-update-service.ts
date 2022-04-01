import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsernameUpdateService {

  emitter: EventEmitter<string> = new EventEmitter<string>();

  emit(username?: string) {
    this.emitter.emit(username);
  }

  subscribe(fn: (username?: string) => void) {
    this.emitter.subscribe(fn);
  }
}
