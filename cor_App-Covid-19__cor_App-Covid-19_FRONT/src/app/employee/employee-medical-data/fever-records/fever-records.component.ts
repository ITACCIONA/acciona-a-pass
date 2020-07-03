import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfigService, DateFormatPipe } from '@commons';
import { BehaviorSubject } from 'rxjs';
import { EmployeeService } from '../../employee.service';
import { ITemperature } from '../../interfaces';
import { FeverModalComponent } from '../fever-modal/fever-modal.component';

@Component({
  selector: 'app-fever-records',
  templateUrl: './fever-records.component.html',
  styleUrls: ['./fever-records.component.scss']
})
export class FeverRecordsComponent implements OnInit {
  @ViewChild('dialog') dialogTmp: TemplateRef<any>;

  @Input()
  id: string;

  isLoading = true;

  meditions: ITemperature[] = [];
  meditionsStepper = new BehaviorSubject(null);

  last: ITemperature;

  threshold = this.config.get('feverThreshold');

  constructor(
    private dateFormat: DateFormatPipe,
    private service: EmployeeService,
    private config: ConfigService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.fetch();
  }

  private fetch() {
    this.isLoading = true;
    this.service.getTemperatures(this.id).subscribe(data => {
      this.meditions = data.filter(r => r.value === true);
      this.last = this.meditions.length ? this.meditions[0] : null;

      this.meditionsStepper.next(
        this.meditions.map(medition => ({
          values: this.dateFormat.transform(medition.date),
          date: ''
        }))
      );

      this.isLoading = false;
    });
  }

  openModal() {
    this.dialog.open(FeverModalComponent, {
      width: '525px',
      data: {
        id: this.id,
        showRecords: true,
        records$: this.meditionsStepper,
        onUpdate: () => this.fetch()
      }
    });
  }
}
