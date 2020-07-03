import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GeneralFormComponent } from '../general-form/general-form.component';

@Component({
  selector: 'app-send-notification-general',
  templateUrl: './send-notification-general.component.html',
  styleUrls: ['./send-notification-general.component.scss']
})
export class SendNotificationGeneralComponent implements OnInit {
  @ViewChild(GeneralFormComponent)
  form: GeneralFormComponent;

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {}

  send() {
    this.form.send();
  }

  isValid() {
    return this.form?.isValid();
  }

  close() {
    this.dialog.closeAll();
  }
}
