import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { passportColorMapper } from '@commons';
import { I18NEXT_SERVICE, ITranslationService } from 'angular-i18next';
import moment from 'moment';
import { MastersService } from '../../../masters.service';
import { ToastrService } from '../../../ui/toastr/toastr.service';
import { EmployeeChangeStatusModalComponent } from '../../employee-change-status-modal/employee-change-status-modal.component';
import { EmployeeService } from '../../employee.service';
import { IEmployee } from '../../interfaces';

@Component({
  selector: 'app-passport',
  templateUrl: './passport.component.html',
  styleUrls: ['./passport.component.scss']
})
export class PassportComponent implements OnInit {
  @Input()
  id: string;

  @Input() showAge = false;

  @Input() showStatusName = false;

  @Input() showChange = false;

  @Input() showGoRed = false;

  @Input() showExpire = false;

  @Input() showInfo = false;

  isLoading = true;

  data: IEmployee;

  constructor(
    private service: EmployeeService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private masters: MastersService,
    @Inject(I18NEXT_SERVICE) private i18NextService: ITranslationService
  ) {}

  ngOnInit(): void {
    this.fetch();
    if (this.i18NextService.language && this.i18NextService.language !== 'en') {
      require('moment/locale/' + this.i18NextService.language);
      moment.locale(this.i18NextService.language);
    }
  }

  private fetch() {
    this.service.getPassport(this.id).subscribe(
      data => {
        this.isLoading = false;
        this.data = data;
      },
      () => {
        // when passport is still not created
        this.isLoading = false;
      }
    );
  }

  changeStatus() {
    const dialogRef = this.dialog.open(EmployeeChangeStatusModalComponent, {
      width: '450px',
      data: { idEmployee: Number(this.id), status: this.data.estadoPasaporte }
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.isLoading = false;
        this.fetch();
      }
    });
  }

  expire() {
    this.isLoading = true;
    this.service.expirePassport(this.id).subscribe(
      () => this.fetch(),
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }

  goRed() {
    this.isLoading = true;
    this.service.goRedPassport(this.id).subscribe(
      () => this.fetch(),
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }

  get age() {
    return this.data?.edadEmpleado !== -1 ? this.data?.edadEmpleado : null;
  }

  get status() {
    return this.data && this.data.estadoPasaporte;
  }

  get colorClass() {
    if (!this.expires) {
      return 'grey';
    } else {
      return passportColorMapper(this.data.colorPasaporte);
    }
  }

  get expires() {
    if (!this.data) {
      return null;
    }

    const expireStatus = this.masters.passportStates.filter(s => s.exp);
    if (expireStatus.length && expireStatus[0].id === this.data.estadoDatabaseId) {
      return null;
    }

    const diff = moment(this.data.fechaExpiracion).diff();
    return diff > 0 ? moment(this.data.fechaExpiracion).fromNow() : null;
  }
}
