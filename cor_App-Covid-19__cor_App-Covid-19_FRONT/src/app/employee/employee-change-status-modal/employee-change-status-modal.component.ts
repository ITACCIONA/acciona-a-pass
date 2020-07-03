import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { passportColorMapper } from '@commons';
import { IPassportState, MastersService } from 'src/app/masters.service';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { EmployeeService } from '../employee.service';

export interface IModalChangeStatusData {
  idEmployee: number;
  status: string;
}

@Component({
  selector: 'app-employee-change-status-modal',
  templateUrl: './employee-change-status-modal.component.html',
  styleUrls: ['./employee-change-status-modal.component.scss']
})
export class EmployeeChangeStatusModalComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  statusList: IPassportState[];

  constructor(
    public dialogRef: MatDialogRef<EmployeeChangeStatusModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IModalChangeStatusData,
    private fb: FormBuilder,
    private service: EmployeeService,
    private mastersService: MastersService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.statusList = this.mastersService.passportStates;
    const currentStatus = this.statusList.find(el => el.name === this.data.status);
    this.form = this.fb.group({
      status: currentStatus ? currentStatus.id : null
    });
  }

  isSelected(status: number) {
    return status === this.form.get('status').value;
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  getStatusFromId(id: number) {
    return this.statusList.find(st => st.id === id);
  }

  getClassColor(color: string) {
    return passportColorMapper(color);
  }

  submit() {
    this.isLoading = true;
    this.service.savePassportState(this.form.get('status').value, this.data.idEmployee).subscribe(
      val => {
        if (val) {
          this.toastr.success('COMMONS.SUCCESS');
          this.isLoading = false;
          this.dialogRef.close(true);
        } else {
          this.isLoading = false;
          this.toastr.error('COMMONS.ERROR');
        }
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }
}
