import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SendNotificationSingleComponent } from '../../../notifications/send-notification-single/send-notification-single.component';
import { EmployeeService } from '../../employee.service';
import { IEmployeePersonal } from '../../interfaces';

@Component({
  selector: 'app-employee-personal-data',
  templateUrl: './employee-personal-data.component.html',
  styleUrls: ['./employee-personal-data.component.scss']
})
export class EmployeePersonalDataComponent implements OnInit {
  @Input()
  id: string;

  isLoading = true;
  data: IEmployeePersonal;

  constructor(private service: EmployeeService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.service.getPersonal(this.id).subscribe(data => {
      this.data = data;
      this.isLoading = false;
    });
  }

  openModal() {
    this.dialog.open(SendNotificationSingleComponent, { width: '620px', data: this.data });
  }
}
