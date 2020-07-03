import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { I18NextPipe } from 'angular-i18next';
import { EmployeeService } from '../../employee.service';
import { IResponsibleStatement } from '../../interfaces';

@Component({
  selector: 'app-responsible-statement',
  templateUrl: './responsible-statement.component.html',
  styleUrls: ['./responsible-statement.component.scss']
})
export class ResponsibleStatementComponent implements OnInit {
  @ViewChild('dialog') dialogTmp: TemplateRef<any>;
  data: IResponsibleStatement<string>[] = [];
  dataPreview: IResponsibleStatement<string>[] = [];

  @Input() id: string;

  isLoading = true;

  constructor(private service: EmployeeService, private dialog: MatDialog, private i18n: I18NextPipe) {}

  ngOnInit(): void {
    this.fetch();
  }

  private fetch() {
    this.isLoading = true;
    this.service.getResponsibleStatement(this.id).subscribe(data => {
      this.data = data
        .map(day => {
          const symptoms = [
            ...new Set(
              day.values.filter(s => s.value).map(s => this.i18n.transform(`SYMPTOMS.${s.name.toUpperCase()}`))
            )
          ];
          return { date: day.date, values: symptoms.join(', ') };
        })
        .filter(d => d.values !== '');

      this.dataPreview = this.data.slice(0, 3);
      this.isLoading = false;
    });
  }

  openModal() {
    this.dialog.open(this.dialogTmp, { width: '385px' });
  }
}
