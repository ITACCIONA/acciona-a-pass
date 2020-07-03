import { Component, OnInit, ViewChild } from '@angular/core';
import { GeneralFormComponent } from '../general-form/general-form.component';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  @ViewChild(GeneralFormComponent)
  form: GeneralFormComponent;

  constructor() {}

  ngOnInit(): void {}

  send() {
    this.form.send();
  }

  isValid() {
    return this.form?.isValid();
  }


}
