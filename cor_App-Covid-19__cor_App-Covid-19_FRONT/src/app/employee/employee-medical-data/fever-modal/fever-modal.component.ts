import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfigService } from '@commons';
import { BehaviorSubject } from 'rxjs';
import { ToastrService } from '../../../ui/toastr/toastr.service';
import { EmployeeService } from '../../employee.service';

interface IFeverModalData {
  id: string;
  showRecords?: boolean;
  records$?: BehaviorSubject<any[]>;
  onUpdate?: () => void;
}
@Component({
  selector: 'app-fever-modal',
  templateUrl: './fever-modal.component.html',
  styleUrls: ['./fever-modal.component.scss']
})
export class FeverModalComponent implements OnInit {
  isLoading = false;
  date = new FormControl(new Date());

  showRecords = false;
  records$: BehaviorSubject<any[]>;
  id: string;

  threshold = this.config.get('feverThreshold');

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IFeverModalData,
    private config: ConfigService,
    private service: EmployeeService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.id = this.data.id;
    if (this.data.showRecords) {
      this.showRecords = true;
      this.records$ = this.data.records$;
    }
  }

  registerFever() {
    this.isLoading = true;
    this.service.registerFever(this.id, this.date.value).subscribe(
      () => {
        this.toastr.success('COMMONS.SUCCESS');
        this.isLoading = false;
        if (this.data.onUpdate) {
          this.data.onUpdate();
        }
      },
      () => {
        this.toastr.error('COMMONS.ERROR');
        this.isLoading = false;
      }
    );
  }
}
