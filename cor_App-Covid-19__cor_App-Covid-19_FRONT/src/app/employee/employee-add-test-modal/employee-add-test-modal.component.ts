import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { EmployeeAddTestModalService } from './employee-add-test-modal.service';

interface ModalData {
  tests: { [field: string]: string[] };
  employeeId: number;
}

@Component({
  selector: 'app-employee-add-test-modal',
  templateUrl: './employee-add-test-modal.component.html',
  styleUrls: ['./employee-add-test-modal.component.scss']
})
export class EmployeeAddTestModalComponent implements OnInit {
  form: FormGroup;
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<EmployeeAddTestModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModalData,
    private fb: FormBuilder,
    private service: EmployeeAddTestModalService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      date: new Date(),
      type: '',
      ...Object.keys(this.data.tests).reduce((map, obj) => {
        map[obj] = this.fb.group(
          this.data.tests[obj].reduce((acum, el) => {
            acum[el] = ['', Validators.required];
            return acum;
          }, {})
        );
        return map;
      }, {})
    });
    this.form.get('type').valueChanges.subscribe(data => {
      this.data.tests[data].forEach(value => {
        this.form
          .get(data)
          .get(value)
          .enable();
      });
      Object.keys(this.data.tests)
        .filter(el => el !== data)
        .forEach(test => {
          this.data.tests[test].forEach(value => {
            this.form
              .get(test)
              .get(value)
              .disable();
          });
        });
    });
    this.form.get('type').setValue('fast');
  }

  get controls() {
    if (this.form.get(this.form.get('type').value)) {
      return Object.keys((this.form.get(this.form.get('type').value) as FormGroup).controls);
    }
    return [];
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  getFieldI18n(field: string) {
    return `EMPLOYEES.ADDTESTMODAL.${field.toString().toUpperCase()}`;
  }

  submit() {
    this.isLoading = true;
    this.service.saveFastTest(this.data.employeeId, this.form.getRawValue()).subscribe(
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
