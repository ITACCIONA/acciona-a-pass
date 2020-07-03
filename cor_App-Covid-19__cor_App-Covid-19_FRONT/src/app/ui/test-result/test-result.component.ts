import { Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeService } from '../../employee/employee.service';
import { ToastrService } from '../toastr/toastr.service';

export interface ITestResult {
  id?: number;
  date: string;
  name?: string;
  positive: boolean;
  breakdown: { name: string; positive: boolean }[];
}
@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.scss']
})
export class TestResultComponent implements OnInit {
  @Output() fetch: EventEmitter<any> = new EventEmitter();
  @Input() data: ITestResult;
  @Input() id: number;
  @ViewChild('dialog') dialogTmp: TemplateRef<any>;
  isLoading = false;

  constructor(public dialog: MatDialog, private service: EmployeeService, private toastr: ToastrService) {}

  ngOnInit(): void {}

  openModal() {
    this.dialog.open(this.dialogTmp, { width: '525px' });
  }

  removeTest() {
    this.isLoading = true;
    this.service.removeTest(this.id, this.data.id, this.data.name).subscribe(
      r => {
        this.isLoading = false;
        if (r.success) {
          this.toastr.success('COMMONS.SUCCESS');
          this.dialog.closeAll();
          this.fetch.emit();
        } else {
          this.toastr.error('COMMONS.ERROR');
          this.fetch.emit();
        }
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
        this.fetch.emit();
      }
    );
  }
}
