import { Component, OnDestroy, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FiltersService } from '../../employee/list/employees-filters/filters.service';
import { IComplementary } from '../../employee/list/employees-list/headers';
import { ITableHeaders } from '../../ui/table/table.component';
import { OutsourcingService } from '../outsourcing.service';

@Component({
  selector: 'app-outsourcing-list',
  templateUrl: './outsourcing-list.component.html',
  styleUrls: ['./outsourcing-list.component.scss']
})
export class OutsourcingListComponent implements OnInit, OnDestroy {
  isLoading = true;
  headers: Array<ITableHeaders & IComplementary> = [
    {
      field: 'nombre',
      sortField: 'Nombre',
      label: `EMPLOYEE.TABLE.NAME`,
      click: row => this.router.navigate(['/outsourcing/employee', row.idEmpleado])
    },
    {
      field: 'dni',
      sortField: 'DNI',
      label: `EMPLOYEES.PERSONAL.DNI`
    },
    {
      field: 'localizacion',
      sortField: 'Localizacion',
      label: `EMPLOYEES.PERSONAL.WORKCENTER`
    }
  ];
  page = 1;
  totalResults = 0;
  pageSize: number;
  data: any[];

  orderByDescending = true;
  sortOrder = '';
  filterTxt = '';

  private destroyed$ = new Subject();

  constructor(private service: OutsourcingService, private router: Router, private filters: FiltersService) {}

  ngOnInit(): void {
    this.get();

    this.filters.current.pipe(takeUntil(this.destroyed$)).subscribe(() => this.get());
  }

  private get() {
    this.isLoading = true;
    const filters = this.filters.current.value;
    this.service.list(this.sortOrder, this.filterTxt, this.orderByDescending, this.page, filters).subscribe(data => {
      this.data = data.employees;
      this.totalResults = data.numElements;
      this.pageSize = data.elementsPerPage;
      this.isLoading = false;
    });
  }

  filter(search: string) {
    this.filterTxt = search.trim();
    this.page = 1;
    this.sortOrder = '';
    this.orderByDescending = true;
    this.get();
  }

  sort([sortCol, order]) {
    const sort = this.headers.find(el => el.field === sortCol);
    this.sortOrder = sort ? sort.sortField : 'nombre';
    this.orderByDescending = order === 'asc' ? false : true;
    this.get();
  }

  pageChange(page: PageEvent) {
    this.page = page.pageIndex + 1;
    this.get();
  }

  export() {
    this.service
      .export(this.sortOrder, this.filterTxt, this.orderByDescending, this.filters.current.value)
      .subscribe(response => {
        const type = response.type;
        const fakeLink = document.createElement('a');
        fakeLink.href = window.URL.createObjectURL(new Blob([response], { type }));
        fakeLink.setAttribute('download', 'export.csv');
        document.body.appendChild(fakeLink);
        fakeLink.click();
      });
  }

  ngOnDestroy() {
    this.destroyed$.next();
    this.filters.clean();
  }
}
