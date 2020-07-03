import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-header-notifications',
  templateUrl: './header-notifications.component.html',
  styleUrls: ['./header-notifications.component.scss']
})
export class HeaderNotificationsComponent implements OnInit {
  @Output()
  openNotifications: EventEmitter<void> = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  openBtn() {
    this.openNotifications.emit();
  }
}
