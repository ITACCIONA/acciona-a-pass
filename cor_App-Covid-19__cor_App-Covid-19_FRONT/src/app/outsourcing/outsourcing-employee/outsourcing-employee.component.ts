import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { IOutsourcingEmployee, OutsourcingService } from '../outsourcing.service';

@Component({
  selector: 'app-outsourcing-employee',
  templateUrl: './outsourcing-employee.component.html',
  styleUrls: ['./outsourcing-employee.component.scss']
})
export class OutsourcingEmployeeComponent implements OnInit {
  @ViewChild('dialog') dialogTmp: TemplateRef<any>;
  @ViewChild('dialogNewPass') dialogNewPass: TemplateRef<any>;

  isLoading = true;
  modalLoading = false;
  data: IOutsourcingEmployee;

  password: string;

  id: string;

  constructor(
    public dialog: MatDialog,
    private service: OutsourcingService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params.id;
    this.service.getEmployee(this.id).subscribe(
      data => {
        this.isLoading = false;
        this.data = data;
      },
      () => {
        this.router.navigate(['/']);
      }
    );
  }

  openReset() {
    this.dialog.open(this.dialogTmp, { width: '525px' });
  }

  resetPassword() {
    this.modalLoading = true;
    this.service.resetPassword(this.data.idResetear).subscribe(
      passwd => {
        this.password = passwd;
        this.modalLoading = false;
        this.dialog.closeAll();
        this.dialog.open(this.dialogNewPass, { width: '525px' });
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }
}
