import { Component, Input, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeAddTestModalComponent } from '../employee-add-test-modal/employee-add-test-modal.component';
import { TestsComponent } from './tests/tests.component';

@Component({
  selector: 'app-employee-medical-data',
  templateUrl: './employee-medical-data.component.html',
  styleUrls: ['./employee-medical-data.component.scss']
})
export class EmployeeMedicalDataComponent {
  @Input()
  id: string;

  @ViewChild(TestsComponent) testComp: TestsComponent;

  constructor(public dialog: MatDialog) {}

  openModal(): void {
    const dialogRef = this.dialog.open(EmployeeAddTestModalComponent, {
      width: '525px',
      data: { tests: { fast: ['control', 'igg', 'igm'], pcr: ['pcr'] }, employeeId: this.id }
    });

    dialogRef.afterClosed().subscribe(() => this.testComp.fetch());
  }
}
