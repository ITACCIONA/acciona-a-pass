import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidenavService {
  status: BehaviorSubject<boolean> = new BehaviorSubject(false);
  content: BehaviorSubject<boolean> = new BehaviorSubject(null);

  constructor() {}

  toggle() {
    this.status.next(!this.status.value);
  }

  open() {
    this.status.next(true);
  }

  close() {
    this.status.next(false);
  }

  setContent(component: any) {
    this.content.next(component);
  }

  set(status: boolean) {
    this.status.next(status);
  }
}
