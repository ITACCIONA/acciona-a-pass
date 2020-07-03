import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IEmployeePersonal } from '../../employee/interfaces';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { NotificationsService } from '../notifications.service';

@Component({
  selector: 'app-send-notification-single',
  templateUrl: './send-notification-single.component.html',
  styleUrls: ['./send-notification-single.component.scss']
})
export class SendNotificationSingleComponent implements OnInit {
  isLoading = false;

  form: FormGroup;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IEmployeePersonal,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private service: NotificationsService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      receiver: new FormControl({
        value: `${this.data.nombreEmpleado} ${this.data.apellidosEmpleado}`,
        disabled: true
      }),
      title: new FormControl('', Validators.required),
      comment: new FormControl('', Validators.required)
    });
  }

  send() {
    this.isLoading = true;
    this.service.sendSingle(this.form.value.title, this.form.value.comment, this.data.idEmpleado).subscribe(
      () => {
        this.toastr.success('NOTIFICATIONS.SUCCESS');
        this.isLoading = false;
        this.dialog.closeAll();
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }

  get age() {
    return this.data?.edadEmpleado !== -1 ? this.data?.edadEmpleado : null;
  }
}
