import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfigService } from '@commons';
import { MastersService } from '../../masters.service';
import { EmployeeAddTestModalComponent } from '../employee-add-test-modal/employee-add-test-modal.component';
import { FeverModalComponent } from '../employee-medical-data/fever-modal/fever-modal.component';

@Component({
  selector: 'app-prl',
  templateUrl: './prl.component.html',
  styleUrls: ['./prl.component.scss']
})
export class PrlComponent implements OnInit {
  @Input()
  id: string;
  threshold = this.config.get('feverThreshold');

  @ViewChild('dialog') dialogTmp: TemplateRef<any>;

  isLoading = false;
  date = new FormControl(new Date());

  showButtons = false;
  showGoRed = false;
  showExpire = false;

  constructor(public dialog: MatDialog, private config: ConfigService, private masters: MastersService) {}

  ngOnInit(): void {
    // this.showButtons = this.masters.getRole() === 'PRL';
    const role = this.masters.getRole();
    this.showGoRed = role === 'PRL' || role === 'RRHHDesc';
    this.showExpire = role === 'PRL' || role === 'RRHHCent';
  }

  openModalTest(): void {
    this.dialog.open(EmployeeAddTestModalComponent, {
      width: '525px',
      data: { tests: { fast: ['control', 'igg', 'igm'], pcr: ['pcr'] }, employeeId: this.id }
    });
  }

  openModalFever() {
    this.dialog.open(FeverModalComponent, {
      width: '525px',
      data: { id: this.id }
    });
  }
}
