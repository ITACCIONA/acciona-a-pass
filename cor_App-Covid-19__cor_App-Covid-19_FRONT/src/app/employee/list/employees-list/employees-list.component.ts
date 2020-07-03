import { Component, OnDestroy, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { DateFormatPipe, passportColorMapper } from '@commons';
import { I18NextPipe } from 'angular-i18next';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MastersService } from '../../../masters.service';
import { ITableHeaders } from '../../../ui/table/table.component';
import { EmployeeService } from '../../employee.service';
import { IEmployeeListSingle } from '../../interfaces';
import { FiltersService } from '../employees-filters/filters.service';
import {
  EMPLOYEES_LIST_HEADERS_DEFAULT,
  EMPLOYEES_LIST_HEADERS_MEDICAL,
  EMPLOYEES_LIST_HEADERS_PRL,
  IComplementary
} from './headers';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.scss']
})
export class EmployeesListComponent implements OnInit, OnDestroy {
  private destroyed$ = new Subject();

  sortOrder = '';
  filterTxt = '';
  orderByDescending = true;
  page = 1;

  isLoading = true;
  data: IEmployeeListSingle[];
  totalResults = 0;
  pageSize: number;

  tableDef: Array<ITableHeaders & IComplementary> = [];

  constructor(
    private dateFormat: DateFormatPipe,
    private service: EmployeeService,
    private masters: MastersService,
    private router: Router,
    private i18n: I18NextPipe,
    private filters: FiltersService
  ) {}

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.filters.clean();
  }

  ngOnInit(): void {
    const role = this.masters.getRole();
    if (role === 'RRHHCent' || role === 'RRHHDesc') {
      this.isLoading = false;
    } else {
      this.getList();
    }
    this.filters.current.pipe(takeUntil(this.destroyed$)).subscribe(filter => {
      if (Object.keys(filter).length !== 0) {
        this.getList();
      }
    });

    if (role === 'ServicioMedico') {
      this.tableDef = EMPLOYEES_LIST_HEADERS_MEDICAL(this.router);
    } else if (role === 'PRL') {
      this.tableDef = EMPLOYEES_LIST_HEADERS_PRL(this.router, this.i18n);
    } else {
      this.tableDef = EMPLOYEES_LIST_HEADERS_DEFAULT(this.router);
    }
  }

  private canViewResults() {
    return !(this.filterTxt === '' && (this.masters.getRole() === 'RRHHCent' || this.masters.getRole() === 'RRHHDesc'));
  }

  pageChange(page: PageEvent) {
    this.page = page.pageIndex + 1;
    this.getList();
  }

  filter(search: string) {
    this.filterTxt = search.trim();

    if (!this.canViewResults()) {
      this.data = [];
      this.totalResults = 0;
      this.page = 1;
    } else {
      this.page = 1;
      this.sortOrder = '';
      this.orderByDescending = true;
      this.getList();
    }
  }

  sort([sortCol, order]) {
    if (!this.canViewResults()) {
      this.data = [];
      this.totalResults = 0;
      this.page = 1;
    } else {
      this.page = 1;
      if (order) {
        const elementFromHeaders = this.tableDef.find(el => el.field === sortCol);
        if (elementFromHeaders) {
          this.sortOrder = elementFromHeaders.sortField;
        }

        this.orderByDescending = order === 'desc';
      } else {
        this.orderByDescending = true;
        this.sortOrder = 'nombre';
      }
      this.getList();
    }
  }

  export() {
    if (this.canViewResults()) {
      const filters = this.filters.current.value;
      this.service.exportList(this.sortOrder, this.filterTxt, this.orderByDescending, filters).subscribe(response => {
        const type = response.type;
        const fakeLink = document.createElement('a');
        fakeLink.href = window.URL.createObjectURL(new Blob([response], { type }));
        fakeLink.setAttribute('download', 'export.csv');
        document.body.appendChild(fakeLink);
        fakeLink.click();
      });
    }
  }

  private getList() {
    this.isLoading = true;
    const filters = this.filters.current.value;
    this.service
      .getList(this.sortOrder, this.filterTxt, this.orderByDescending, this.page, filters)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(data => {
        this.data = data.employees.map(e => ({
          ...e,
          genero: e.genero ? `GENDERS.${e.genero.toUpperCase()}` : null,
          ultiModifi: this.dateFormat.transform(e.ultiModifi),
          colorPasaporte: passportColorMapper(e.colorPasaporte),
          colorTxt: `PASSPORT.COLORS.${passportColorMapper(e.colorPasaporte).toUpperCase()}`
        }));

        this.totalResults = data.numElements;
        this.pageSize = data.elementsPerPage;
        this.isLoading = false;
      });
  }
}
