import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { passportColorMapper } from '@commons';
import { IPassportState, MastersService } from '../../masters.service';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { IModalChangeStatusData } from '../employee-change-status-modal/employee-change-status-modal.component';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-change-status-simple',
  templateUrl: './change-status-simple.component.html',
  styleUrls: ['./change-status-simple.component.scss']
})
export class ChangeStatusSimpleComponent implements OnInit {
  isLoading = true;
  form: FormGroup;

  statuses: IPassportState[];

  constructor(
    private fb: FormBuilder,
    private masters: MastersService,
    @Inject(MAT_DIALOG_DATA) public data: IModalChangeStatusData,
    private dialogRef: MatDialogRef<ChangeStatusSimpleComponent>,
    private toastr: ToastrService,
    private service: EmployeeService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      status: [null, Validators.required]
    });

    this.masters.fetchPassportStatesSimple().subscribe(data => {
      this.isLoading = false;
      this.statuses = data;
    });
  }

  getClassColor(color: string) {
    return passportColorMapper(color);
  }

  colorLabel(color: string) {
    return `PASSPORT.COLORS.${passportColorMapper(color)}`.toUpperCase();
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
